import { keycloak } from '@/keycloak'
import type { UserInfo } from '@/types/User'
import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

export const useAuthStore = defineStore('auth', () => {
  const initialized = ref(false)
  const authenticated = ref(false)
  const user = ref<UserInfo | null>(null)

  const isAdmin = computed(() => {
    return user.value?.roles.includes('admin') ?? false
  })

  async function init() {
    const isAuthenticated = await keycloak.init({
      onLoad: 'check-sso',
      pkceMethod: 'S256',
      checkLoginIframe: false,
    })

    authenticated.value = isAuthenticated

    if (isAuthenticated) {
      setUser()
      startTokenRefresh()
    }

    initialized.value = true
  }

  function setUser() {
    const token = keycloak.tokenParsed as any

    user.value = {
      id: token.sub,
      name: token.name,
      email: token.email,
      username: token.preferred_username,
      roles: token.realm_access?.roles || [],
    }
  }

  async function login() {
    await keycloak.login()
  }

  async function logout() {
    await keycloak.logout({
      redirectUri: window.location.origin,
    })

    authenticated.value = false
    user.value = null
  }

  async function refreshToken() {
    try {
      const refreshed = await keycloak.updateToken(30)

      if (refreshed) {
        setUser()
      }
    } catch {
      await logout()
    }
  }

  function startTokenRefresh() {
    setInterval(async () => {
      if (!authenticated.value) return

      await refreshToken()
    }, 10000)
  }

  return {
    initialized,
    authenticated,
    user,
    isAdmin,

    init,
    login,
    logout,
    refreshToken,
  }
})
