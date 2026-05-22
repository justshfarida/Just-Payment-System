import { useUserStore } from '@/stores/UserStore'
import router from './index'

router.beforeEach(async (to) => {
  const auth = useUserStore()

  // wait auth initialization
  if (!auth.initialized) {
    await auth.init()
  }

  const requiresAuth = to.meta.requiresAuth
  const allowedRoles = to.meta.roles as string[] | undefined

  if (requiresAuth && !auth.authenticated) {
    await auth.login()
    return false
  }

  if (allowedRoles?.length) {
    const hasAccess = allowedRoles.some((role) => auth.inRole(role))

    if (!hasAccess) {
      return '/unauthorized'
    }
  }

  return true
})
