<script setup lang="ts">
import { useUserStore } from '@/stores/UserStore'

const userStore = useUserStore()

const logout = async () => {
  userStore.logout()
}
</script>

<template>
  <UUser
    v-if="userStore.authenticated"
    :name="userStore.user?.name"
    :description="userStore.user?.email"
    :avatar="{
      icon: 'i-uil-user',
    }"
  >
    <template #avatar>
      <UPopover>
        <UIcon name="i-uil-user" class="size-5"></UIcon>
        <template #content>
          <div class="p-1">
            <UButton @click="logout()" color="neutral" variant="outline">Logout</UButton>
          </div>
        </template>
      </UPopover>
    </template>
  </UUser>
</template>
