<script setup lang="ts">
import { useUserStore } from '@/stores/UserStore'

const userStore = useUserStore()

const logout = async () => {
  userStore.logout()
}
</script>

<template>
  <UPopover v-if="userStore.authenticated">
    <UUser
      :name="userStore.user?.name"
      :description="userStore.user?.email"
      :avatar="{ icon: 'i-uil-user' }"
      class="cursor-pointer"
    />

    <template #content>
      <div class="p-2">
        <UButton @click="logout()" color="neutral" variant="outline" icon="i-uil-sign-out-alt">
          Logout
        </UButton>
      </div>
    </template>
  </UPopover>
</template>
