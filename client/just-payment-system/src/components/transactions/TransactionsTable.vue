<script setup lang="ts">
import type { Transaction } from '@/types/Transaction'
import type { TableColumn } from '@nuxt/ui'
import { ref } from 'vue'
const transactions = ref<Transaction[]>([
  {
    id: 'tx_84729',
    date: '2026-05-21 12:34',
    status: 'completed',
    amount: 124.5,
    card_mask: '•••• 4242',
    currency: 'USD',
    payload: { gateway: 'stripe', ip_address: '192.168.1.1', device: 'iOS' },
  },
  {
    id: 'tx_19472',
    date: '2026-05-20 09:15',
    status: 'pending',
    amount: 15.0,
    card_mask: '•••• 5555',
    currency: 'EUR',
    payload: { gateway: 'paypal', pre_auth: true },
  },
  {
    id: 'tx_03821',
    date: '2026-05-19 18:40',
    status: 'failed',
    amount: 2500.0,
    card_mask: '•••• 9102',
    currency: 'USD',
    payload: { error_code: 'insufficient_funds', attempts: 2 },
  },
])

const columns: TableColumn<Transaction>[] = [
  {
    accessorKey: 'id',
    header: '#',
    cell: ({ row }) => `#${row.getValue('id')}`,
  },
  {
    accessorKey: 'date',
    header: 'Date',
    cell: ({ row }) => {
      return new Date(row.getValue('date')).toLocaleString('en-US', {
        day: 'numeric',
        month: 'short',
        hour: '2-digit',
        minute: '2-digit',
        hour12: false,
      })
    },
  },
  {
    accessorKey: 'status',
    header: 'Status',
    cell: ({ row }) => {
      const color =
        {
          paid: 'success' as const,
          failed: 'error' as const,
          refunded: 'neutral' as const,
        }[row.getValue('status') as string] || 'neutral'

      return row.getValue('status')
    },
  },
  {
    accessorKey: 'card_mask',
    header: 'Card',
    cell: ({ row }) => row.getValue('card_mask'),
  },
  {
    accessorKey: 'currency',
    header: 'Currency',
    meta: {
      class: {
        th: 'text-right',
        td: 'text-right font-medium',
      },
    },
    cell: ({ row }) => {
      return row.getValue('currency')
    },
  },
  {
    accessorKey: 'amount',
    header: 'Amount',
    meta: {
      class: {
        th: 'text-right',
        td: 'text-right font-medium',
      },
    },
    cell: ({ row }) => {
      return row.getValue('amount')
    },
  },
  {
    accessorKey: 'payload',
    header: 'Payload',
    cell: ({ row }) => {
      const payload = row.getValue('payload')

      return JSON.stringify(payload)
    },
  },
]

// Status badge color mapping helper
const getStatusColor = (status: Transaction['status']) => {
  switch (status) {
    case 'completed':
      return 'success'
    case 'pending':
      return 'warning'
    case 'failed':
      return 'error'
    default:
      return 'neutral'
  }
}
</script>

<template>
  <div class="w-full border border-neutral-200 dark:border-neutral-800 rounded-lg overflow-hidden">
    <UTable :columns="columns" :data="transactions">
      <template #id-data="{ row }">
        <span class="font-mono font-medium text-neutral-900 dark:text-neutral-100">
          {{ row.original.id }}
        </span>
      </template>

      <template #status-data="{ row }">
        <UBadge :color="getStatusColor(row.original.status)" variant="soft" class="capitalize">
          {{ row.original.status }}
        </UBadge>
      </template>

      <template #card_mask-data="{ row }">
        <div class="flex items-center gap-2 text-neutral-600 dark:text-neutral-400">
          <UIcon name="i-heroicons-credit-card" class="w-4 h-4" />
          <span class="font-mono text-sm">{{ row.original.card_mask }}</span>
        </div>
      </template>

      <template #amount-data="{ row }">
        <div class="text-right font-semibold">
          {{
            new Intl.NumberFormat('en-US', {
              style: 'currency',
              currency: row.original.currency,
            }).format(row.original.amount)
          }}
        </div>
      </template>

      <template #expand="{ row }">
        <div
          class="p-4 bg-neutral-50 dark:bg-neutral-900/50 border-t border-neutral-100 dark:border-neutral-800"
        >
          <div class="mb-2 text-xs font-semibold text-neutral-500 tracking-wider uppercase">
            Raw Payload Data
          </div>
          <pre
            class="p-3 text-xs font-mono rounded bg-neutral-100 dark:bg-neutral-900 overflow-x-auto text-neutral-800 dark:text-neutral-200 border border-neutral-200 dark:border-neutral-800"
            >{{ JSON.stringify(row.original.payload, null, 2) }}</pre
          >
        </div>
      </template>
    </UTable>
  </div>
</template>
