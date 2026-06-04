<template>
  <div class="max-w-4xl mx-auto py-10 px-4 sm:px-6 lg:px-8">
    <header class="mb-10">
      <div class="flex items-center space-x-2 text-sm text-gray-500 dark:text-gray-400 mb-2">
        <span>Documentation</span>
        <span>/</span>
        <span class="text-primary-500 font-medium">Core Flows</span>
      </div>
      <h1 class="text-4xl font-bold text-gray-900 dark:text-white tracking-tight mb-4">
        Payment & Webhook Lifecycle
      </h1>
      <p class="text-lg text-gray-600 dark:text-gray-300">
        An overview of how transactions are initiated, processed, and synchronized across merchant
        services using RabbitMQ and API Gateway.
      </p>
    </header>

    <UDivider class="my-8" />

    <section class="mb-12">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white mb-4">
        Architecture Architecture
      </h2>
      <p class="mb-4 text-gray-600 dark:text-gray-300">
        All client requests enter through the <strong>API Gateway</strong>. Internal services
        communicate asynchronously via <strong>RabbitMQ</strong> events to ensure loose coupling and
        high availability.
      </p>

      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 my-6">
        <UCard class="text-center">
          <template #header><span class="font-bold">1. API Gateway</span></template>
          Routes external client traffic securely to backend microservices.
        </UCard>
        <UCard class="text-center">
          <template #header><span class="font-bold">2. RabbitMQ</span></template>
          Handles event distribution (`transaction-completed`, `callback-failed`).
        </UCard>
        <UCard class="text-center">
          <template #header><span class="font-bold">3. Webhook Service</span></template>
          Manages retries and merchant callbacks asynchronously.
        </UCard>
      </div>
    </section>

    <section class="mb-12">
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white mb-6">
        Step-by-Step Execution Flow
      </h2>

      <div
        class="space-y-8 relative before:absolute before:inset-0 before:left-4 before:w-0.5 before:bg-gray-200 dark:before:bg-gray-800"
      >
        <div class="relative pl-10">
          <div
            class="absolute left-1 top-1 bg-primary-500 rounded-full w-6 h-6 flex items-center justify-center text-white text-xs font-bold"
          >
            1
          </div>
          <h3 class="text-lg font-medium text-gray-900 dark:text-white flex items-center gap-2">
            Transaction Initiation
            <UBadge color="orange" variant="soft">PENDING</UBadge>
          </h3>
          <p class="mt-2 text-gray-600 dark:text-gray-300">
            The client requests a new transaction through the <strong>API Gateway</strong>. The
            Transaction Service creates the record with a
            <code class="text-orange-500">pending</code> status.
          </p>
          <UAlert
            icon="i-heroicons-clock"
            color="amber"
            variant="solid"
            title="Redirect URL Expiration"
            description="The service returns a unique payment redirect URL. This token/URL is strictly valid for only 5 minutes."
            class="mt-3"
          />
        </div>

        <div class="relative pl-10">
          <div
            class="absolute left-1 top-1 bg-primary-500 rounded-full w-6 h-6 flex items-center justify-center text-white text-xs font-bold"
          >
            2
          </div>
          <h3 class="text-lg font-medium text-gray-900 dark:text-white flex items-center gap-2">
            Payment Capture & Event Publishing
            <UBadge color="green" variant="soft">CAPTURED</UBadge>
          </h3>
          <p class="mt-2 text-gray-600 dark:text-gray-300">
            Once the user successfully completes the payment, the transaction state transitions to
            <code class="text-green-500">captured</code>. Immediately, a
            <code class="font-semibold text-primary-500">transaction-completed</code> event is
            published to RabbitMQ.
          </p>
        </div>

        <div class="relative pl-10">
          <div
            class="absolute left-1 top-1 bg-primary-500 rounded-full w-6 h-6 flex items-center justify-center text-white text-xs font-bold"
          >
            3
          </div>
          <h3 class="text-lg font-medium text-gray-900 dark:text-white">Webhook Delivery</h3>
          <p class="mt-2 text-gray-600 dark:text-gray-300">
            The <strong>Webhook Service</strong> consumes the
            <code class="font-semibold">transaction-completed</code> event and executes a POST
            request to the Merchant Service's registered callback URL.
          </p>
        </div>

        <div class="relative pl-10">
          <div
            class="absolute left-1 top-1 bg-primary-500 rounded-full w-6 h-6 flex items-center justify-center text-white text-xs font-bold"
          >
            4
          </div>
          <h3 class="text-lg font-medium text-gray-900 dark:text-white flex items-center gap-2">
            Fallback & Reversal Execution
            <UBadge color="red" variant="soft">REFUNDED</UBadge>
          </h3>
          <p class="mt-2 text-gray-600 dark:text-gray-300">
            If the Merchant callback URL fails to return an HTTP
            <code class="text-green-500">200 OK</code> status:
          </p>
          <ul class="list-disc list-inside mt-2 space-y-1 text-gray-600 dark:text-gray-300 pl-2">
            <li>
              The Webhook Service publishes a
              <code class="font-semibold text-red-500">callback-failed</code> event to RabbitMQ.
            </li>
            <li>
              The Transaction Service consumes this event and automatically updates the state to
              <code class="text-red-500 font-semibold">Refunded</code>.
            </li>
          </ul>
        </div>
      </div>
    </section>

    <UDivider class="my-8" />

    <section>
      <h2 class="text-2xl font-semibold text-gray-900 dark:text-white mb-4">
        State Reference Matrix
      </h2>
      <UTable :rows="stateRows" :columns="stateColumns" />
    </section>
  </div>
</template>

<script setup>
const stateColumns = [
  { key: 'state', label: 'Transaction Status' },
  { key: 'trigger', label: 'Trigger Action' },
  { key: 'next', label: 'Next System Event / State' },
]

const stateRows = [
  {
    state: 'PENDING',
    trigger: 'Client initializes transaction via Gateway',
    next: 'Generates redirect URL (5 min TTL)',
  },
  {
    state: 'CAPTURED',
    trigger: 'Successful external payment confirmation',
    next: 'Publishes "transaction-completed" event',
  },
  {
    state: 'REFUNDED',
    trigger: 'Merchant callback returns non-200 status',
    next: 'Publishes "callback-failed", money reversed',
  },
]
</script>
