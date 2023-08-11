<script>
	import $ from "jquery"; 
	import APIHelper from "../mixins/APIHelper.js";
	import Vue3CanvasVideoPlayer from 'vue3-canvas-video-player';
	import 'vue3-canvas-video-player/dist/style.css';

    
	export default{
		mixins:[APIHelper],
		components:{Vue3CanvasVideoPlayer},
		data(){
			return{
				srcurl:"",
				url:"",
				Eurl:"/episodes",
				Furl:"/films",
				myObject:{}
			}
		},
		methods:{
			async LoadData(){
				this.url = this.$route.query.isFilm? this.Furl : this.Eurl;
				this.myObject = await this.apiRequestJson("GET", `${this.url}/${this.$route.params.id}/download`)
				console.log(this.myObject)
				this.srcurl = `${this._urlFilePrefix}/${this.myObject.path}`

				if(this.myObject.next != null){
					if(this.myObject.hasNext){
						$("#prepare_next_text").removeClass("hide")
					}
					else{
						$("#preare_next_btn").removeClass("hide")
					}
				}
			},
			async PrepareNext(){
				$("#preare_next_btn").addClass("hide")
				$("#prepare_next_wait").removeClass("hide")
				await this.apiRequest("GET", `${this.url}/${this.myObject.next.id}/download`)
				$("#prepare_next_wait").addClass("hide")
				$("#prepare_next_text").removeClass("hide")
				
			}
		},
		async mounted(){
			await this.LoadData()
		}
	}
</script>

<template>
	<article>
		<!-- <dev class="d-none d-md-flex panel w-100">
			<Vue3CanvasVideoPlayer class="w-100" :src = "this.srcurl" :autoplay="false" :loop="false" :messageTime="1000"/>
		</dev> -->
		<div v-if="this.srcurl!=''" class="d-flex flex-column">
			<h1 class="text-center">{{ this.myObject.model.name }}</h1>
			<div class="d-flex flex-row panel w-100 p-0">
				<a v-if="this.myObject.preveous" class="btn btn-dark d-flex align-items-center p-0 p-md-2 br-0"
				 :href= "this.myObject.preveous.id" role="button">
					{{ '<' }}
				</a>
				<video  class="w-100" controls  >
					<source :src="this.srcurl" >
				</video>
				<a v-if="this.myObject.next" class="btn btn-dark d-flex align-items-center p-0  p-md-2 br-0"
				 :href= "this.myObject.next.id" role="button">
					{{ '>' }}
				</a>
			</div>
		</div>
		<div v-else>
			<p class="text-center">
				Идёт конвертация и загрузка серии. Подождите пожалуйста(это может занять длительное время)
			</p>
		</div>
		<div class="d-flex flex-column">
			<button type="button" id="preare_next_btn" class="d-grid gap-2 btn btn-primary hide" @click="PrepareNext()">
				Подготовить следующую серию
			</button>
			<div id="prepare_next_wait" class="d-grid gap-2 psevdo-btn psevdo-btn-warning align-items-center hide">
				<p class="text-center text-light my-auto">Подготовка следующей серии</p>
			</div>
			<div id="prepare_next_text" class="d-grid gap-2 psevdo-btn align-items-center hide">
				<p class="text-center text-light my-auto">Следующая серия готова</p>
			</div>
		</div>

	</article>
</template>

<style scoped>

.br-0{
	border-radius: 0!important;
}
.psevdo-btn{
	border-radius: var(--bs-border-radius);
	background-color: #198754;
	border-color: #198754!important;
	border:solid;
	width: 400;
	height: 1.5;
	
}
.psevdo-btn-warning{
	background-color: #ffc107!important;
	border-color: #ffc107!important;
}
.hide{
	display: none!important;
}

</style>
