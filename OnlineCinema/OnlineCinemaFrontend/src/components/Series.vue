<script>
	import $ from "jquery"; 
	import APIHelper from "../mixins/APIHelper.js";
	import {MultiSelect} from "../../node_modules/vue-search-select/dist/VueSearchSelect.js";
    
	export default{
		mixins:[APIHelper],
		components:{MultiSelect},
		data(){
			return{
				url:"/series",
				myObject: {},
				tagsURL:"/tags",
				backIurl:"",
				smalIurl:"",
				tags:[],
				items:[]
			}
		},
		computed:{
			cssVars(){
				return{
					'--image-url' : 'url("'+`${this._urlFilePrefix}/${this.backIurl}`+'")',
				}
			}
		},
		methods:{
			async LoadData(){
				var myId = this.$route.params.id;
				this.myObject = await this.apiRequestJson("GET", `${this.url}/${myId}`)
				
				this.tags = await this.apiRequestJson("GET",this.tagsURL)
				this.items = this.myObject.tags
				if(this.items!=null){
					this.items = this.items.filter(x=>x!="").map(x=>({value:x, text:x}))
				}
				else{
					this.items = []
				}

				this.backIurl = await this.apiRequestJson("GET",`/image/background/${this.myObject.name}`)
				this.smalIurl = await this.apiRequestJson("GET",`/image/object/${this.myObject.name}`)
			},
			changeStatus(index){
				const line = $(`#line_${index}`)
				line.hasClass("d-none")? line.removeClass("d-none") : line.addClass("d-none")
			},
			async downloadSeason(item, index){

				$(`#loadPlace_${item.id}`).html('<img src="http://192.168.1.120:5173/src/assets/load.gif" height="40" width="40">')
				const dPath = await this.apiRequestJson("GET",`/seasons/${item.id}/download`)
				if(dPath!=null && dPath != "")
				{
					const dUrl = `${this._urlFilePrefix}/${dPath}`
					console.log(dUrl)

					const a = document.createElement('a');
					a.style.display = 'none';
					a.href = dUrl;
					a.setAttribute("download","custom_name.csv")
					//a.download = (item.name+".zip");
					document.body.appendChild(a);
					a.click();
					console.log(a)
				}
				$(`#loadPlace_${item.id}`).html("")

				

				// const urlDownload = `${this._urlPrefix}/seasons/${sid}/download`

				// 

				// console.log(urlDownload)

				// const responce = await fetch(urlDownload)

				// if(responce.status === 200){
				// 	const blob = await responce.blob()
				// 	const urlFile = window.URL.createObjectURL(blob);

				// 	const a = document.createElement('a');
				// 	a.style.display = 'none';
				// 	a.href = urlFile;
				// 	a.download = sname;
				// 	document.body.appendChild(a);
				// 	a.click();
				// 	window.URL.revokeObjectURL(urlFile);
				// 	alert('Начало загрузки. ')
				// }
				// else{
				// 	alert('Ошибка загрузки. Попробуйте ещё раз')
				// }
				// $(`#loadPlace_${sid}`).html("")
			},
			onSelect (items, lastSelectItem) {
				this.items = items
				this.lastSelectItem = lastSelectItem
			},
			async UpdateSeries(){
				
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
	<article class="d-flex flex-column justify-content-center align-items-center" :style="cssVars">
		<div class="imgHeader">
		</div>
		<h1 class="text-center"> {{ this.myObject.name }}</h1>
		<div class="d-flex flex-column flex-md-row justify-content-center align-items-start mx-3 mw-md-90"><!--Контейнер для страницы-->
				<div class="d-flex flex-column me-2 my-2 w-md-30 p-0 w-100">
					<img class="panel w-100 m-0 p-0" :src="`${this._urlFilePrefix}/${this.smalIurl}`" ><!--Большая картинка справа-->
				</div>
				<div class="d-flex flex-column mw-md-50 my-2">
					<div class="d-flex flex-column panel m-0" ><!--Информационная панель(раньше не было)-->
						<h4 class="text-center">Информация</h4>
						<div class="d-flex justify-content-between m-0"> <!--Контейнер вида |текст.......текст|-->
							<p class="m-0 fw-bold">Тип:</p>
							<p class="m-0 fs-5">Сериал</p>
						</div>
						<div v-if="this.myObject.mySeasons!=null" class="d-flex justify-content-between m-0"> <!--Контейнер вида |текст.......текст|-->
							<p class="m-0 fw-bold">Cезонов:</p>
							<p class="m-0 fs-5">{{ this.myObject.mySeasons.length }}</p>
						</div>
						<div v-if="this.myObject.mySeasons!=null" class="d-flex justify-content-between m-0"> <!--Контейнер вида |текст.......текст|-->
							<p class="m-0 fw-bold">Эпизодов:</p>
							<p class="m-0 fs-5">{{ this.myObject.totalEpisodes}}</p>
						</div>
						<div class="d-flex flex-column justify-content-between m-0"> <!--Контейнер вида |текст.......текст|-->
							<p class="m-0 fw-bold">Теги:</p>
							<div class="d-flex flex-wrap">
								<a class="m-1 fs-5 text-success" :href="`/serieses?tags=${item}&tags=`" v-for="item in this.myObject.tags">{{item }}</a>
							</div>

						</div>
						<div class="d-flex flex-column justify-content-between m-0"> <!--Контейнер вида |текст.......текст|-->
							<p class="m-0 fw-bold">Описание:</p>
							<p class="text-wrap m-0 fs-5">{{ this.myObject.description != null ? this.myObject.description : 
							"На данный момент описания ещё нет. Но оно скоро будет" }}</p>
						</div>
					</div>
					<div v-if="this.isRole('ADMIN')" class="d-flex flex-column panel" >
						<h4 class="text-center">Редактирование</h4>
						<form >
							<div class="mb-3">
							  <label class="form-label">id</label>
							  <input type="text" class="form-control" readonly :value="this.myObject.id">
							</div>
							<div class="mb-3">
								<label class="form-label">Название</label>
								<input type="text" class="form-control" id="form_name" :value="this.myObject.name">
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
							<button type="button" @click="this.UpdateSeries()" class="btn btn-success">Сохранить</button>
						</form>
					</div>
				</div>
		</div>
		<div class="d-flex flex-column mx-2 align-items-center w-100 mt-3">
			<div class="table-responsive panel w-md-60 m-0 p-0">
				<table class="table table-striped table-success align-middle m-0">
					<thead class="table-light">
						<tr>
							<th></th>
							<th>Название</th>
							<th>Скачать</th>
						</tr>
					</thead>
					<tbody class="table-group-divider" v-for="(item, index) in this.myObject.mySeasons"><!-- Вот этот вот тег со всем содержимым надо повторять -->
						<tr class="table-success" >
							<td >
								<a type="button" @click="changeStatus(index)">></a>
							</td>
							<td >{{item.name}}</td>
							<td class="d-flex">
								<a class="btn btn-success" @click="downloadSeason(item,index)">Скачать сезон</a>
								<div :id="`loadPlace_${item.id}`"></div>
							</td>
						</tr>
						<tr>
							<td colspan="3">
								<table class="d-none table table-striped table-hover table-borderless table-success align-middle" :id="	`line_${index}`">
									<tbody>
										<tr class="table-success" v-for="episode in item.episodes">
											<td scope="row"></td>
											<td class="text-wrap">{{episode.item2.replaceAll("_", " ").replaceAll(".", " ")}}</td>
											<td>
												<div class="d-flex flex-column flex-md-row">
													<a class="btn btn-success m-2" 
													:href="`${this._urlFilePrefix}/${episode.item3}`" role="button">
													Скачать
													</a>
													<a class="btn btn-primary m-2" :href="`/player/${episode.item1}`" role="button">
														Посмотреть 
													</a>
												</div>
												
											</td>
										</tr>
									</tbody>
								</table>
							</td>
						</tr>
					</tbody>
				</table>
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
	height: 400px;
	width: 100%;
}
</style>
