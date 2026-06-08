import { useUserStore } from '@/stores/UserStore'
import type { Router } from 'vue-router'

export async function setUpAuthGuard(router: Router) {
  router.beforeEach(async (to) => {
    const auth = useUserStore()

    const requiresAuth = to.meta.requiresAuth
    const allowedRoles = to.meta.roles as string[] | undefined
    if (!auth.initialized) {
      await auth.init()
      console.log(auth.user)
    }
    if (requiresAuth && !auth.authenticated) {
      await auth.login()
      return '/'
    }

    if (allowedRoles?.length) {
      const hasAccess = allowedRoles.some((role) => auth.inRole(role))

      if (!hasAccess) {
        return '/unauthorized'
      }
    }

    return true
  })
}
