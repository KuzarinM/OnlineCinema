<script>
	import $ from "jquery"; 
	import APIHelper from "../mixins/APIHelper.js";
    
	export default{
		mixins:[APIHelper],
		data(){
			return{
				url:"/service"
			}
		},
		methods:{
			async syncronise(){
				if(!confirm("Вы уверены, что хотите начать синхронизацию?")) return
				
				const responce = await this.apiRequest("GET",`${this.url}/synchronization`)
				if(responce.status == 200){
					alert("Синхронизированно!")
				}
			},
			async truncateTMP(){
				if(!confirm("Вы уверены, что хотите удалить всё из tmp папки?")) return

				const responce = await this.apiRequest("DELETE",`${this.url}/tmpTruncate`)
				if(responce.status == 200){
					alert("Очищенно!")
				}
			},
		},
		mounted(){
			//Init component method
		}
	}
</script>

<template>
	<article class="d-flex flex-column">
		<h1 class="text-center">Админская панелька</h1>
		<div  class="d-flex flex-column flex-md-row align-items-start">
			<div class="m-3 d-flex flex-column align-items-start panel">
				<label class="form-label fw-bold">Синхронизироваться с диском</label>
				<button type="button" class="btn btn-warning" @click="syncronise">Начать синхронизацию</button>
			</div>
			<div class="m-3 d-flex flex-column align-items-start panel ">
				<label class="form-label fw-bold">Очистить папку temp</label>
				<button type="button" class="btn btn-danger" @click="truncateTMP">Приступить к очистке</button>
			</div>
		</div>

	</article>
</template>

<style scoped>

</style>
