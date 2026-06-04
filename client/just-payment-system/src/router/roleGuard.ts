import { useUserStore } from '@/stores/UserStore'
import type { Router } from 'vue-router'

export async function setUpAuthGuard(router: Router) {
  router.beforeEach(async (to) => {
    const auth = useUserStore()

    const requiresAuth = to.meta.requiresAuth
    const allowedRoles = to.meta.roles as string[] | undefined
    if (!auth.initialized) {
      console.log('Initializing auth')
      await auth.init()
    }

    console.log('Authenticated:', auth.authenticated)

    if (requiresAuth && !auth.authenticated) {
      console.log('Calling login()')
      await auth.login()
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
