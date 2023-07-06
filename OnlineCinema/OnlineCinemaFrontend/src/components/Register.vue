<script>
	import $ from "jquery"; 
	import APIHelper from "../mixins/APIHelper.js";
    
	export default{
		mixins:[APIHelper],
		data(){
			return{
				url:"/users"
			}
		},
		methods:{
			async Registration(){
				const dto = {
					login:$("#login").val(),
					password:$("#password").val()
				}

				const responce = await this.apiRequest("POST",this.url,dto)
				if(responce.status == 202){
					alert(`Пользователь с логином ${dto.login} и паролем ${dto.password}`)
				}
			}
		},
		mounted(){
			//Init component method
		}
	}
</script>

<template>
<article class="d-flex flex-column align-items-center">
        <h1 class="mb-3">{{"Регистрация"}}</h1>
        <form class="d-flex flex-column panel align-items-cente" @submit.prevent="this.Registration()">
            <div class="form-outline mb-3">
                <label class="form-label">Логин</label>
                <input type="text" class="form-control" id="login"  required="true"  placeholder="логин">
            </div>
            <div class="form-outline mb-3">
                <label class="form-label">Пароль</label>
                <input type="password" class="form-control" id="password" required="true"  placeholder="пароль">
            </div>
            <div class="d-flex ">
                <input class="btn btn-success me-2" type="submit" value="Зарегистрироваться">
            </div>
        </form>
    </article>
</template>

<style scoped>

</style>
