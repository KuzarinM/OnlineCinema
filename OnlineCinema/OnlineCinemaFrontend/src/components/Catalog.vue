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
				objects:[]
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
				
				if(tags!=null && tags!=""){
					tags = tags.filter(x=>x!="")
				}

				console.log(tags)
				console.log(name)

				this.objects = await this.apiRequestJson("POST",this.IsFilm ? this.urlFilm : this.urlSeries,{
					withoutTags:[this.isRole("ADMIN")?"":"private"],
					name:name,
					hasTags:tags
				})
				console.log(this.objects)
			}
		},
		async mounted(){
			await this.LoadData()
		}
	}
</script>

<template>
	<article>
		<div class="d-flex flex-wrap">
			<UniversalObject :myObject = item :imageURL = "`${this._urlPrefix}/image/object/${item.name}`"
			:openObjectUrl = "this.IsFilm ? '/film/' : '/series/'"
			 v-for="item in this.objects" />
		</div>
	</article>
</template>

<style scoped>

</style>
