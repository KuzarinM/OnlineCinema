<script>
	import $ from "jquery"; 
	import APIHelper from "../mixins/APIHelper.js";
	import {MultiSelect} from "../../node_modules/vue-search-select/dist/VueSearchSelect.js";
    
	export default{
		mixins:[APIHelper],
		components:{MultiSelect},
		data(){
			return{
				url:"/films",
				myObject: {},
				tagsURL:"/tags",
				tags:[],
				items:[]
			}
		},
		computed:{
			cssVars(){
				return{
					'--image-url' : 'url("'+`${this._urlFilePrefix}/${this.myObject.backgroundPath}`+'")',
				}
			}
		},
		methods:{
			async LoadData(){
				var myId = this.$route.params.id;
				this.myObject = await this.apiRequestJson("GET", `${this.url}/${myId}`)

				this.myObject.posterPath = this.myObject.posterPath.replaceAll('\\','/')
				this.myObject.backgroundPath = this.myObject.backgroundPath.replaceAll('\\','/')

				this.tags = await this.apiRequestJson("GET",this.tagsURL)
				this.items = this.myObject.tags
				if(this.items!=null){
					this.items = this.items.filter(x=>x!="").map(x=>({value:x, text:x}))
				}
				else{
					this.items = []
				}
				console.log(this.myObject)
			},
			onSelect (items, lastSelectItem) {
				this.items = items
				this.lastSelectItem = lastSelectItem
			},
			async UpdateFilm(){
				
				var description = $("#form_description").val()
				const dto = {
					id:this.myObject.id,
					name: $("#form_name").val(),
					description:description,
					tags : this.items.map(x=>x.value)
				}
				
				const responce = await this.apiRequest("PUT",this.url, dto)
				if(responce.status == 202){
					await this.LoadData();
					alert("Обновлено")
				}
			},
			CreateTag(){
				const tag =$("#create_tag").val()

				if(tag!=null && tag!=""){
					var tagObj = {text:tag, value:tag}
					this.tags.push(tagObj)
					this.items.push(tagObj)
					$("#create_tag").val("")
				}

			}
		},
		async mounted(){
			await this.LoadData()
		}
	}
</script>

<template>
	<article v-if="this.myObject!=null" class="d-flex flex-column justify-content-center" :style="cssVars">
		<div class="imgHeader">
		</div>
		<h1 class="text-center"> {{ this.myObject.name }}</h1>
		<div class="d-flex flex-column flex-md-row justify-content-center align-items-start"><!--Контейнер для страницы-->
			<div class="d-flex flex-column me-2 w-md-30 p-0">
				<img class="panel w-100 m-0 p-0" :src="`${this._urlFilePrefix}/${this.myObject.posterPath}`" ><!--Большая картинка справа-->
			</div>
			<div class="d-flex flex-column">
				<div class="d-flex flex-column panel" ><!--Информационная панель(раньше не было)-->
					<h4 class="text-center">Информация</h4>
					<div class="d-flex justify-content-between m-0"> <!--Контейнер вида |текст.......текст|-->
						<p class="m-0 fw-bold">Тип:</p>
						<p class="m-0 fs-5">Фильм</p>
					</div>
					<div v-if="this.myObject.pakageName!=null" class="d-flex justify-content-between m-0"> <!--Контейнер вида |текст.......текст|-->
						<p class="m-0 fw-bold">Группа:</p>
						<p class="m-0 fs-5">{{ this.myObject.pakageName }}</p>
					</div>
					<div class="d-flex justify-content-between m-0"> <!--Контейнер вида |текст.......текст|-->
						<p class="m-0 fw-bold">Тип файла:</p>
						<p class="m-0 fs-5">{{ this.myObject.extention }}</p>
					</div>
					<div class="d-flex flex-column justify-content-between m-0"> <!--Контейнер вида |текст.......текст|-->
						<p class="m-0 fw-bold">Теги:</p>
						<div class="d-flex flex-wrap">
							<a class="m-1 fs-5 text-success" :href="`/films?tags=${item}&tags=`" v-for="item in this.myObject.tags">{{item }}</a>
						</div>

					</div>
					<div class="d-flex flex-column justify-content-between m-0"> <!--Контейнер вида |текст.......текст|-->
						<p class="m-0 fw-bold">Описание:</p>
						<p class="m-0 fs-5">{{ this.myObject.description != null ? this.myObject.description : 
						"На данный момент описания ещё нет. Но оно скоро будет" }}</p>
					</div>
				</div>
				<a class="btn btn-success" :href="`${this._urlFilePrefix}/${this.myObject.path}`" role="button">Скачать</a>
				<a class="btn btn-primary my-2" :href="`/player/${this.myObject.id}?isFilm=true`" role="button" >Посмотреть </a>
				<div v-if="this.isRole('ADMIN')" class="d-flex flex-column panel" >
					<h4 class="text-center">Редактирование</h4>
					<form @submit.prevent="this.UpdateFilm()">
						<div class="mb-3">
							<label class="form-label">id</label>
							<input type="text" class="form-control" readonly :value="this.myObject.id">
						</div>
						<div class="mb-3">
							<label class="form-label">Название</label>
							<input type="text" class="form-control" id="form_name"  :value="this.myObject.name">
						</div>
						<div class="mb-3">
							<label class="form-label">Описание</label>
							<textarea id="form_description" class="form-control" rows="3" :value="this.myObject.description"></textarea>
						</div>
						<div class="mb-3">
							<label class="form-label">Теги</label>
							<MultiSelect :options = "this.tags.map(x=>({value:x.name, text:x.name}))" 
							:selectedOptions="items" @select="onSelect" @searchchange="x=>console.log(x)" />
							<input type="text" id="create_tag" class="form-control" placeholder="Введите новый тег" @keyup.enter="this.CreateTag()">
						</div>
						<button type="submit" class="btn btn-success">Сохранить</button>
					</form>
				</div>
			</div>
		</div>
	</article>
</template>

<style scoped>
.imgHeader{
	background-repeat: no-repeat;
	background-size: cover;
	background-position: top;
	background-image: var(--image-url);
	height: 500px;
	width: 100%;
}
</style>
