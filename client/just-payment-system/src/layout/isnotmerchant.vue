<script setup lang="ts">
import { useUserStore } from '@/stores/UserStore'

const userStore = useUserStore()
</script>
<template>
  <div class="flex justify-items-center align-center">
    <UButton @click="userStore.logout()"> Salam </UButton>
  </div>
</template>
