<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>
<script>
	import APIHelper from "./mixins/APIHelper.js";
  import $ from "jquery"; 
  import {MultiSelect} from "../node_modules/vue-search-select/dist/VueSearchSelect.js";
  
  export default{
    mixins:[APIHelper],
    components:{MultiSelect},
    data(){
      return{
        tagsURL:"/tags",
        tags:[],
        items:[]
      }
    },
    methods:{
      async LoadData(){
        this.tags = await this.apiRequestJson("GET",this.tagsURL)
        this.tags = this.tags.filter(x=>this.isRole('ADMIN') || x.name != 'private')
        this.items = this.$route.query.tags
        if(this.items!=null && this.items!=""){
          this.items = this.items.filter(x=>x!="").map(x=>({value:x, text:x}))
        }
        else{
          this.items = []
        }
      },
      onSelect (items, lastSelectItem) {
        this.items = items
        this.lastSelectItem = lastSelectItem

        var innerHtml = ""
        items.forEach(element => {
          innerHtml += `<option selected value='${element.value}'></option>\n`
        });
        $("#tags").html(innerHtml)
      }
    },
    async mounted(){
      await this.LoadData()
    }
  }
</script>

<template>
  <header>
    <div class="d-flex justify-content-start">
        <nav class="navbar navbar-expand-sm navbar-light bg-light container m-2">
          <a class="navbar-brand" href="#">Онлайн кинотеатр</a>

          <button class="navbar-toggler d-lg-none" type="button" data-bs-toggle="collapse"
           data-bs-target="#collapsibleNavId" aria-controls="collapsibleNavId" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
          </button>

          <div class="collapse navbar-collapse w-100" id="collapsibleNavId">
            <ul class="navbar-nav mt-2 mt-lg-0 w-100">
              <li class="nav-item">
                <a class="nav-link active" href="/films">Фильмы</a>
              </li>
              <li class="nav-item">
                <a class="nav-link active" href="/serieses">Сериалы</a>
              </li>
              <li class="nav-item w-100">
                <form class="d-flex flex-column flex-md-row my-2 my-lg-0">
                  <input class="form-control me-sm-2" name="search" type="text" :value="this.$route.query.search" >
                  <select id="tags" class="d-none" name="tags" multiple></select>
                  <MultiSelect :options = "this.tags.map(x=>({value:x.name, text:x.name}))" 
                  :selectedOptions="items"
                  name="tags"
                  @select="onSelect" />
                  <input class="d-none" type="number" name="page" :value="this.$route.query.page ? this.$route.query.page : 0">
                  <button class="btn btn-outline-success ms-1 my-2 my-sm-0"  type="submit">Поиск</button>
                </form>
              </li>
              <li class="nav-item">
                <a class="nav-link active " href="/about">Информация</a>
              </li>
              <div v-if="this.isAuthtorised()" class="d-flex flex-column flex-md-row">
                <li class="nav-item">
                  <a class="nav-link active" @click="this.logout()">Выйти</a>
                </li>
                <li v-if="this.isRole('ADMIN')" class="nav-item">
                  <a class="nav-link active" href="/register">Создать</a>
                </li>
                <li v-if="this.isRole('ADMIN')" class="nav-item">
                  <a class="nav-link active" href="/controls">Админская</a>
                </li>
              </div>
              <div v-else>
                <li  class="nav-item">
                  <a class="nav-link active" href="/login">Войти</a>
                </li>
              </div>
            </ul>
          </div>
        </nav>
    </div>
  </header>
  <RouterView />
  <footer class="d-flex flex-column mt-3">
    <p class="text-center mt-auto">Онлайн кинотеатр 2023</p>
  </footer>
</template>

<style scoped>
</style>
