import { createRouter, createWebHistory } from 'vue-router'
import Catalog from '../components/Catalog.vue'
import Film from '../components/Film.vue'
import Series from '../components/Series.vue'
import Login from '../components/Login.vue'
import Register from '../components/Register.vue'
import Player from '../components/Player.vue'
import AdminPanel from '../components/AdminPanel.vue'
import Info from '../components/Info.vue'
import UserTable from '../components/UserTable.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path:"/about",
      name:"about",
      component: Info,
    },
    {
      path:"/",
      redirect:"/serieses"
    },
    {
      path:"/users",
      name:"users",
      component: UserTable,
      meta:{
        authorized:true,
        adminOnly:true
      }
    },
    {
      path:"/controls",
      name:"adminPanel",
      component: AdminPanel,
      meta:{
        authorized:true,
        adminOnly:true
      }
    },
    {
      path:"/player/:id",
      name:"videoPlauer",
      component: Player
    },
    {
      path: '/login',
      name: 'login',
      component: Login,
    },
    {
      path: '/register',
      name: 'register',
      component: Register,
      meta:{
        authorized:true,
        adminOnly:true
      }
    },
    {
      path: '/films',
      name: 'catalog',
      component: Catalog,
      props:{
        IsFilm : true
      }
    },
    {
      path: '/serieses',
      name: 'serieses',
      component: Catalog,
      props:{
        IsFilm: false
      },
    },
    {
      path: '/film/:id',
      name: 'film',
      component: Film,
    },
    {
      path: '/series/:id',
      name: 'series',
      component: Series,
    }
  ]
})


router.beforeEach(Check)

function Check(to, from, next)
{
  if(to.meta.authorized && sessionStorage.getItem("token")===null){
      next({
        path: '/films'
      })
  }
  if(to.meta.adminOnly && sessionStorage.getItem("role")!=="ADMIN"){
    next({
      path: from.path
    })
  }
  next()
}


export default router
