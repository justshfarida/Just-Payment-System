<script setup lang="ts">
import { useUserStore } from '@/stores/UserStore'

const userStore = useUserStore()

const login = async () => {
  await userStore.login()
  window.location.reload()
}
</script>
<template>
  <slot></slot>
</template>
