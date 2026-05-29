import { createRouter, createWebHistory } from 'vue-router'
import { routes, handleHotUpdate } from 'vue-router/auto-routes'
import { setUpAuthGuard } from './roleGuard'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

setUpAuthGuard(router)
if (import.meta.hot) {
  handleHotUpdate(router)
}

export default router
