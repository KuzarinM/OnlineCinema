import { createRouter, createWebHistory } from 'vue-router'
import Catalog from '../components/Catalog.vue'
import Film from '../components/Film.vue'
import Series from '../components/Series.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
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
      }
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

export default router
