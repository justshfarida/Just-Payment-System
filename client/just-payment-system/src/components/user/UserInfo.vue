<script setup lang="ts">
import { keycloak } from '@/keycloak'
import { onMounted } from 'vue'
if (keycloak.authenticated) {
  console.log('User ID (Subject):', keycloak.subject)
  console.log('Preferred Username:', keycloak.idTokenParsed?.preferred_username)
  console.log('Full Token Data:', keycloak.tokenParsed)
}
</script>

<template></template>
