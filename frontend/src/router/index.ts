import { createRouter, createWebHashHistory } from 'vue-router'
import { routes as basket } from '@/modules/basket/routes'
import { routes as chat } from '@/modules/chat/routes'
import { routes as catalog } from '@/modules/catalog/routes'

const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    ...basket,
    ...chat,
    ...catalog
  ]
})

export default router
