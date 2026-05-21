<script setup lang="ts">
import UserInfo from '@/components/user/UserInfo.vue'
import { keycloak } from '@/keycloak'
import { onMounted } from 'vue'
onMounted(() => {
  keycloak.loadUserInfo()
  console.log(keycloak.userInfo)
})
</script>

<template>
  <UserInfo></UserInfo>
</template>
