<script setup lang="ts">
import { onMounted } from 'vue'
import { keycloak } from './keycloak'

onMounted(async () => {
  await keycloak.init({
    onLoad: 'login-required',
  })
})
</script>

<template>
  <RouterView></RouterView>
</template>
