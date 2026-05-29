<script setup lang="ts">
import { onBeforeMount, ref } from 'vue'
import isnotmerchant from './layout/isnotmerchant.vue'
import notauthenticated from './layout/notauthenticated.vue'
import { useUserStore } from './stores/UserStore'
import { VueSpinner } from 'vue3-spinners'

const userStore = useUserStore()
const isLoading = ref(false)
</script>

<template>
  <div v-if="isLoading" class="h-screen w-screen flex justify-center align-middle">
    <VueSpinner color="green" size="40"></VueSpinner>
  </div>
  <div v-else-if="userStore.authenticated">
    <RouterView v-if="userStore.inRole('merchant')"></RouterView>

    <isnotmerchant v-else></isnotmerchant>
  </div>
  <div v-else>
    <notauthenticated>
      <RouterView></RouterView>
    </notauthenticated>
  </div>
</template>
