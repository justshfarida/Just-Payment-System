<script setup lang="ts">
import { onBeforeMount, ref } from 'vue'
import Default from './layout/default.vue'
import isnotmerchant from './layout/isnotmerchant.vue'
import { useUserStore } from './stores/UserStore'
import { VueSpinner } from 'vue3-spinners'

const userStore = useUserStore()
const isLoading = ref(false)
onBeforeMount(async () => {
  isLoading.value = true
  try {
    await userStore.init()
    console.log(userStore.user)
  } finally {
    isLoading.value = false
  }
})
</script>

<template>
  <div v-if="!isLoading && userStore.authenticated">
    <Default v-if="userStore.inRole('merchant')">
      <RouterView></RouterView>
    </Default>
    <isnotmerchant v-else></isnotmerchant>
  </div>
  <div v-else-if="isLoading" class="h-screen w-screen flex justify-center align-middle">
    <VueSpinner color="green" size="40"></VueSpinner>
  </div>
</template>
