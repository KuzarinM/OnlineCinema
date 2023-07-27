<script>
	import $ from "jquery"; 
	import APIHelper from "../mixins/APIHelper.js";
    
	export default{
		mixins:[APIHelper],
		data(){
			return{
				url:"/users",
				users:[],
				_changingLine:Object
			}
		},
		methods:{
			async LoadData(){
				this.users = await this.apiRequestJson("GET",this.url)
			},
			async AddNewLine(){
				var object = {
					login : $(`#login_field`).val(),
					password : $(`#password_field`).val(),
					role : $(`#role_field`).val()
				}
				await this.apiRequest("POST",this.url,object)
				$("#modalClose").click()
				await this.LoadData()
			},
			OpenChangeWindow(object){
				this._changingLine = object
				$("#openChangeModal").click()
			},
			async ChangeLine(){
				const index = this.users.indexOf(this._changingLine)
				if(index>-1){
					this._changingLine={
						id: this._changingLine.id,
						login : $(`#login_field_change`).val(),
						password : $(`#password_field_change`).val(),
						role : $(`#role_field_change`).val(),
					}
					await this.apiRequest("PUT",this.url,this._changingLine)
					$("#modalChangeClose").click()
					await this.LoadData()
				}
			},
			async DeleteLine(){
				const index = this.users.indexOf(this._changingLine)
				if(index>-1 && confirm("Вы действительно хотите удалить данную запись?")){
					this.users.splice(index,1)
					await this.apiRequest("DELETE",`${this.url}/${this._changingLine.id}`)
					$("#modalChangeClose").click()
					await this.LoadData()
				}
			},
		},
		async mounted(){
			await this.LoadData()
		}
	}
</script>

<template>
	<article>
		<div class="table-responsive mx-2">
        <table class="table table-primary">
            <thead>
                <tr>
                    <th scope="col">Логин</th>
                    <th scope="col">Пароль</th>
                    <th scope="col">Роль</th>
                </tr>
            </thead>
            <tbody>
                <tr class="" v-for="item in this.users">
					<td @click="this.OpenChangeWindow(item)">{{ item.login }}</td>
					<td @click="this.OpenChangeWindow(item)">{{ "*".repeat(item.password.length) }}</td>
					<td @click="this.OpenChangeWindow(item)">{{ Roles[item.role] }}</td>
                </tr>
                <tr>
                    <td class="p-0" :colspan="3">
                        <div class="d-flex justify-content-center">
                            <button type="button" class="btn btn-plus-table" data-bs-toggle="modal"
							 data-bs-target="#creatingModalId">+</button>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
		<button type="button" class="hide" data-bs-toggle="modal" id="openChangeModal" data-bs-target="#changeModalId">+</button>
    	</div>
		<div class="modal fade" id="creatingModalId" tabindex="-1" role="dialog" aria-labelledby="modalTitleId" aria-hidden="true">
			<div class="modal-dialog modal-dialog-scrollable modal-dialog-centered modal-md" role="document">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title" id="modalTitleId">Добавить потльзователя</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<div class="modal-body">
						<form @submit.prevent="AddNewLine">
							<div class="mb-3">
								<label class="form-label">Логин</label>
								<input type="text" class="form-control" id="login_field" required>
							</div>
							<div class="mb-3">
								<label class="form-label">Пароль</label>
								<input type="password" class="form-control" id="password_field" required>
							</div>
							<div class="mb-3">
								<label class="form-label">Роль</label>
								<select class="form-select form-select-lg" id="role_field">
									<option :value="index" v-for="(item, index) in this.Roles">{{ item }}</option>
								</select>
							</div>
							<div class="modal-footer">
								<button type="submit" class="btn btn-success">Сохранить</button>
								<button type="button" class="btn btn-secondary" id="modalClose" data-bs-dismiss="modal">Закрыть</button>
							</div>
						</form>
					</div>
				</div>
			</div>
		</div>
		<div class="modal fade" id="changeModalId" tabindex="-1" role="dialog" aria-labelledby="modalTitleId" aria-hidden="true">
			<div class="modal-dialog modal-dialog-scrollable modal-dialog-centered modal-md" role="document">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title" id="modalTitleId">Изменить потльзователя</h5>
						<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
					</div>
					<div class="modal-body">
						<form @submit.prevent="ChangeLine">
							<div class="mb-3">
								<label class="form-label">Логин</label>
								<input type="text" class="form-control" id="login_field_change" required :value="this._changingLine.login">
							</div>
							<div class="mb-3">
								<label class="form-label">Пароль</label>
								<input type="password" class="form-control" id="password_field_change" required :value="this._changingLine.password">
							</div>
							<div class="mb-3">
								<label class="form-label">Роль</label>
								<select class="form-select form-select-lg" id="role_field_change" :value="this._changingLine.role">
									<option :value="index" v-for="(item, index) in this.Roles">{{ item }}</option>
								</select>
							</div>
							<div class="modal-footer">
								<button type="submit" class="btn btn-success">Сохранить</button>
								<button type="button" class="btn btn-danger" @click="DeleteLine">Удалить</button>
								<button type="button" class="btn btn-secondary" id="modalChangeClose" data-bs-dismiss="modal">Закрыть</button>
							</div>
						</form>
					</div>
				</div>
			</div>
		</div>
	</article>
</template>

<style scoped>
	.btn-plus-table{
		background-color: #cfe2ff;
    	border-color: #cfe2ff;
		width: 100%;
	}
</style>
