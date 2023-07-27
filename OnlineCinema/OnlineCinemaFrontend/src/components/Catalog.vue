<script>
	import $ from "jquery"; 
	import APIHelper from "../mixins/APIHelper.js";
	import UniversalObject from "./UniversalObject.vue";
    
	export default{
		mixins:[APIHelper],
		components:{UniversalObject},
		data(){
			return{
				urlFilm:"/films",
				urlSeries:"/series",
				objects:[],
				currentTags:[],
				count:20
			}
		},
		props:{
			IsFilm:{
				default:true,
				type:Boolean
			}
		},
		methods:{
			async LoadData(){
				const name = this.$route.query.search;
				var tags = this.$route.query.tags;
				var page = this.$route.query.page

				if(page == null || page == ""){
					page = 0
				}
				else{
					page = parseInt(page)
				}
				
				if(tags!=null && tags!=""){
					tags = tags.filter(x=>x!="")
				}
				this.currentTags = tags
				console.log(tags)
				console.log(name)

				this.objects = await this.apiRequestJson("POST",this.IsFilm ? this.urlFilm : this.urlSeries,{
					withoutTags:[this.isRole("ADMIN")?"":"private", this.isRole("USER")?"":"internal"],
					name:name,
					hasTags:tags,
					count:this.count,
					page:page
				})

				console.log(this.objects)
			},
			goTo(delta){
				const pg = parseInt($("#pageIF").val())
				var newPg = pg+delta
				if(newPg<0 || (delta>0 && this.objects!=null && this.objects.count<=0))
					return
				$("#pageIF").val(newPg)

				$("#pageSubmit").click()
			},
			async getImage(object){
				const a = await this.apiRequestJson("GET",`/image/object/${object.name}`)
				const img = `${this._urlFilePrefix}/${a}`
				return img
			}
		},
		async mounted(){
			await this.LoadData()
		}
	}
</script>

<template>
	<article class="d-flex flex-column">
		<div class="d-flex flex-wrap justify-content-center">
			<UniversalObject :myObject = item :imageURL = "`${this._urlFilePrefix}/${item.posterPath}`"
			:openObjectUrl = "this.IsFilm ? '/film/' : '/series/'"
			 v-for="item in this.objects" />
		</div>
		<div class="d-flex justify-content-center">
			<form class="d-flex ">
				<input class="d-none" name="search" type="text" :value="this.$route.query.search" >
				<input type="text" class="d-none" name="tags" v-for="item in this.currentTags" :value="item">
				<input v-if="this.currentTags!=null && this.currentTags.length>0" type="text" class="d-none" name="tags" value="">
				<button type="button" class="btn btn-primary p-0" @click="goTo(-1)">
					<h1>
						&larr;
					</h1>
				</button>
				<input id="pageIF" type="number" class="form-control" name="page" :value="this.$route.query.page!=null ? this.$route.query.page : 0">
				<button type="button" class="btn btn-primary p-0" @click="goTo(1)">
					<h1>
						&rarr;
					</h1>
				</button>
				<button type="submit" class="hide" id="pageSubmit">Перейти</button>
			</form>
			
		</div>
	</article>
</template>

<style scoped>

</style>
