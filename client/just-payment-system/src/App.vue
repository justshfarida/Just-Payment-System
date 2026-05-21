<script setup lang="ts">
import { onMounted } from 'vue'
import { keycloak } from './keycloak'
import Admin from './layout/default.vue'
onMounted(async () => {
  await keycloak.init({
    onLoad: 'login-required',
  })
})
</script>

<template>
  <Admin>
    <RouterView></RouterView>
  </Admin>
</template>
