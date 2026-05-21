<script setup lang="ts">
import { ref, computed } from 'vue'
import StatCard from '../../components/transactions/StatCard.vue'
// Mock database metrics state
const stats = ref({
  totalCount: 1420,
  succeedCount: 1342,
  failedCount: 78,
  revenue: 84250.75,
  currency: 'EUR',
  // Comparison deltas against previous week context
  trends: {
    revenue: {
      isUp: true,
      value: '+9.3',
    },
    total: {
      value: '+8.2%',
      isUp: true,
    },
    succeed: {
      value: '+9.1%',
      isUp: true,
    },
    failed: {
      value: '-3.2%',
      isUp: false,
    },
  },
})

// Calculate operational success rate dynamically
const successRate = computed(() => {
  if (!stats.value.totalCount) return '0%'
  return ((stats.value.succeedCount / stats.value.totalCount) * 100).toFixed(1) + '%'
})

// Format numeric revenue out cleanly
const formattedRevenue = computed(() => {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: stats.value.currency,
  }).format(stats.value.revenue)
})
</script>

<template>
  <div class="max-w-7xl mx-auto py-10 px-4 sm:px-6 lg:px-8">
    <!-- Top Greeting Header section -->
    <div class="mb-8 flex items-center justify-between">
      <div>
        <h1 class="text-2xl font-bold tracking-tight text-neutral-900 dark:text-neutral-100">
          Dashboard Overview
        </h1>
        <p class="text-sm text-neutral-500 dark:text-neutral-400 mt-1">
          Real-time metrics tracking your active commercial payment pipeline.
        </p>
      </div>

      <!-- Operational Performance Badge Indicator -->
      <UBadge
        color="success"
        variant="subtle"
        class="font-medium px-2.5 py-1 text-xs gap-1.5 rounded-full"
      >
        <span class="w-1.5 h-1.5 rounded-full bg-success-500 animate-pulse"></span>
        Health Score: {{ successRate }}
      </UBadge>
    </div>

    <!-- Responsive Grid Display for Metric Cards -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-5">
      <!-- Card 1: Gross Revenue -->
      <StatCard
        title="Gross Revenue"
        :count="stats.revenue"
        :trend-count="stats.trends.revenue.value"
        :is-up="stats.trends.revenue.isUp"
      >
        <template #icon>
          <UIcon
            name="i-heroicons-banknotes"
            class="w-5 h-5 text-primary-500 dark:text-primary-400"
          />
        </template>
      </StatCard>

      <StatCard
        title="Total Transactions"
        :count="stats.totalCount"
        :trend-count="stats.trends.total.value"
        :is-up="stats.trends.total.isUp"
      >
        <template #icon>
          <UIcon
            name="i-heroicons-credit-card"
            class="w-5 h-5 text-neutral-600 dark:text-neutral-400"
          />
        </template>
      </StatCard>

      <StatCard
        title="Succeed Count"
        :count="stats.succeedCount"
        :trend-count="stats.trends.succeed.value"
        :is-up="stats.trends.succeed.isUp"
      >
        <template #icon>
          <UIcon
            name="i-heroicons-check-circle"
            class="w-5 h-5 text-success-500 dark:text-success-400"
          />
        </template>
      </StatCard>

      <StatCard
        title="Failed Count"
        :count="stats.failedCount"
        :trend-count="stats.trends.failed.value"
        :is-up="stats.trends.failed.isUp"
      >
        <template #icon>
          <UIcon
            name="i-heroicons-x-circle"
            class="w-5 h-5 text-error-500 dark:text-error-400" /></template
      ></StatCard>
    </div>
  </div>
</template>
