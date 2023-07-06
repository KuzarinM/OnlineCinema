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
				Furl:"/films"
			}
		},
		methods:{
			async LoadData(){
				this.url = this.$route.query.isFilm? this.Furl : this.Eurl;
				const myEpisode = await this.apiRequestJson("GET", `${this.url}/${this.$route.params.id}/download`)
				console.log(myEpisode)
				this.srcurl = `${this._urlFilePrefix}/${myEpisode.path}`
			},
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
		<div v-if="this.srcurl!=''" class="d-flex  panel w-100">
			<video  class="w-100" controls  >
				<source :src="this.srcurl" >
			</video>
		</div>
		<div v-else>
			<p class="text-center">
				Идёт конвертация и загрузка серии. Подождите пожалуйста(это может занять длительное время)
			</p>
		</div>

	</article>
</template>

<style scoped>

</style>
