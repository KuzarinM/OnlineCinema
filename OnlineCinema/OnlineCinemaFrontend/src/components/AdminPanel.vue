<script>
	import $ from "jquery"; 
	import APIHelper from "../mixins/APIHelper.js";
    
	export default{
		mixins:[APIHelper],
		data(){
			return{
				url:"/service",
				tmpList:[]
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
			async getTMPData(){
				this.tmpList = await this.apiRequestJson("GET",`${this.url}/tmpGet`)
			}
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
				<label class="form-label fw-bold">Папка temp</label>
				<button type="button" class="btn btn-primary m-1" data-bs-toggle="modal" data-bs-target="#modaltmpId" @click="this.getTMPData">
					Текущее наполнение
				</button>
				<button type="button" class="btn btn-danger" @click="truncateTMP">Приступить к очистке</button>
			</div>
		</div>

	</article>
	<div class="modal fade" id="modaltmpId" tabindex="-1" role="dialog" aria-labelledby="modalTitleId" aria-hidden="true">
		<div class="modal-dialog modal-dialog-scrollable modal-dialog-centered modal-lg" role="document">
			<div class="modal-content">
				<div class="modal-header">
					<h5 class="modal-title" id="modalTitleId">Наполнение tmp папки</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
				</div>
				<div class="modal-body p-1">
					<div class="table-responsive">
						<table class="table table-primary">
							<thead>
								<tr>
									<th scope="col">Тип</th>
									<th scope="col">Название</th>
									<th scope="col">Размер</th>
								</tr>
							</thead>
							<tbody>
								<tr class="" v-for="item in this.tmpList">
									<td>{{ item.type }}</td>
									<td>{{ item.name.replaceAll("."," ").replaceAll("_"," ") }}</td>
									<td>{{ item.size.toFixed(2) }} Мб.</td>
								</tr>
							</tbody>
						</table>
					</div>
					
				</div>
			</div>
		</div>
	</div>
</template>

<style scoped>

</style>


