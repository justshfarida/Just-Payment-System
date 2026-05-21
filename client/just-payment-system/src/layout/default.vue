<script setup lang="ts">
import { ref } from 'vue'
import { useRoute } from 'vue-router'
import type { NavigationMenuItem } from '@nuxt/ui'
import { keycloak } from '@/keycloak'

const toast = useToast()
const route = useRoute()

const open = ref(false)

const links = [
  [
    {
      label: 'Home',
      icon: 'i-lucide-house',
      to: '/merchant',
      onSelect: () => {
        open.value = false
      },
    },
    {
      label: 'Transactions',
      icon: 'i-uil-transaction',
      to: '/merchant/transactions',
      onSelect: () => {
        open.value = false
      },
    },
    {
      label: 'My credentials',
      icon: 'i-lucide-key-round',
      to: '/merchant/credentials',
      onSelect: () => {
        open.value = false
      },
    },
    {
      label: 'Webhook',
      icon: 'i-ic-baseline-webhook',
      to: '/merchant/webhook',
      onSelect: () => {
        open.value = false
      },
    },
  ],
] satisfies NavigationMenuItem[][]

const logout = () => {
  keycloak.logout()
}
</script>
<template>
  <UDashboardGroup>
    <UDashboardSidebar
      id="default"
      v-model:open="open"
      collapsible
      resizable
      class="bg-elevated/25"
      :ui="{ footer: 'lg:border-t lg:border-default' }"
    >
      <template #default="{ collapsed }">
        <UNavigationMenu
          :collapsed="collapsed"
          :items="links[0]"
          orientation="vertical"
          tooltip
          popover
        />

        <UNavigationMenu
          :collapsed="collapsed"
          :items="links[1]"
          orientation="vertical"
          tooltip
          class="mt-auto"
        />
      </template>

      <template #footer="{ collapsed }">
        <UserMenu :collapsed="collapsed" />
      </template>
    </UDashboardSidebar>

    <slot></slot>
    <UDashboardNavbar>
      <template #right>
        <UButton color="neutral" @click="logout">Logout</UButton>
      </template>
    </UDashboardNavbar>
  </UDashboardGroup>
</template>
