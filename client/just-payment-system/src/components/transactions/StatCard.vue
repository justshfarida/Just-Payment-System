<script setup lang="ts">
defineProps<{
  title: string
  isUp: boolean
  count: number
  trendCount: string | null
}>()
</script>
<template>
  <UCard>
    <div class="flex items-center justify-between">
      <span class="text-sm font-medium text-neutral-500 dark:text-neutral-400">{{ title }}</span>
      <div class="p-2 bg-success-50 dark:bg-success-950/40 rounded-lg">
        <slot name="icon"></slot>
      </div>
    </div>
    <div class="mt-4">
      <h3 class="text-2xl font-semibold tracking-tight text-neutral-900 dark:text-neutral-100">
        {{ count }}
      </h3>
      <p
        :class="[
          'mt-1 text-xs flex items-center gap-1 font-medium',
          isUp ? 'text-success-600 dark:text-success-400' : 'text-error-600 dark:text-error-400',
        ]"
      >
        <!-- Passing the explicit entire string ensures Tailwind compiles both variants properly -->
        <UIcon
          :name="isUp ? 'i-heroicons-arrow-trending-up' : 'i-heroicons-arrow-trending-down'"
          class="w-3.5 h-3.5"
        />

        {{ trendCount }}
        <span class="text-neutral-400 font-normal">vs last week</span>
      </p>
    </div>
  </UCard>
</template>
