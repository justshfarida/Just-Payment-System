<script setup lang="ts">
import { ref } from 'vue'

interface Credentials {
  merchantId: string
  privateKey: string
}

// Mock initial data fetch
const credentials = ref<Credentials>({
  merchantId: 'mid_2026_prod_9942a',
  privateKey: 'sk_live_51N...j8x9', // Existing truncated key
})

const isCopiedId = ref(false)
const isCopiedKey = ref(false)
const showPrivateKey = ref(false)

// 1. Copy utility functions
const copyToClipboard = async (text: string, type: 'id' | 'key') => {
  await navigator.clipboard.writeText(text)
  if (type === 'id') {
    isCopiedId.value = true
    setTimeout(() => (isCopiedId.value = false), 2000)
  } else {
    isCopiedKey.value = true
    setTimeout(() => (isCopiedKey.value = false), 2000)
  }
}

// 2. Client-side cryptographically secure random key generator
const generateNewKey = () => {
  const array = new Uint8Array(32) // 256 bits of entropy
  window.crypto.getRandomValues(array)

  // Convert byte arrays to hex string format
  const hexKey = Array.from(array)
    .map((b) => b.toString(16).padStart(2, '0'))
    .join('')

  credentials.value.privateKey = `sk_live_${hexKey}`
  showPrivateKey.value = true // Force visibility so the user can save it
}
</script>

<template>
  <div class="max-w-3xl mx-auto py-10 px-4 sm:px-6">
    <!-- Header Section -->
    <div class="mb-8 border-b border-neutral-200 dark:border-neutral-800 pb-5">
      <h1 class="text-2xl font-semibold text-neutral-900 dark:text-neutral-100">API Credentials</h1>
      <p class="mt-1 text-sm text-neutral-500 dark:text-neutral-400">
        Use these credentials to authenticate your server-side requests. Keep the private key
        secure.
      </p>
    </div>

    <div class="space-y-6">
      <!-- Merchant ID Input Group -->
      <UFormField
        label="Merchant ID"
        description="Your unique merchant identifier used to route API traffic."
      >
        <div class="flex w-full gap-2 mt-1">
          <UInput
            :model-value="credentials.merchantId"
            disabled
            class="w-full font-mono bg-neutral-50 dark:bg-neutral-900"
          />
          <UButton
            :icon="isCopiedId ? 'i-heroicons-check' : 'i-heroicons-clipboard'"
            :color="isCopiedId ? 'success' : 'neutral'"
            variant="outline"
            @click="copyToClipboard(credentials.merchantId, 'id')"
          >
            {{ isCopiedId ? 'Copied' : 'Copy' }}
          </UButton>
        </div>
      </UFormField>

      <!-- Private Key Input Group -->
      <UFormField
        label="Private Key"
        description="Secret key used to sign authorization payloads. Never share this key."
      >
        <div class="flex w-full gap-2 mt-1">
          <UInput
            :type="showPrivateKey ? 'text' : 'password'"
            :model-value="credentials.privateKey"
            disabled
            class="w-full font-mono bg-neutral-50 dark:bg-neutral-900"
          />

          <UButton
            :icon="showPrivateKey ? 'i-heroicons-eye-slash' : 'i-heroicons-eye'"
            color="neutral"
            variant="outline"
            @click="showPrivateKey = !showPrivateKey"
          />

          <UButton
            :icon="isCopiedKey ? 'i-heroicons-check' : 'i-heroicons-clipboard'"
            :color="isCopiedKey ? 'success' : 'neutral'"
            variant="outline"
            @click="copyToClipboard(credentials.privateKey, 'key')"
          >
            {{ isCopiedKey ? 'Copied' : 'Copy' }}
          </UButton>
        </div>
      </UFormField>

      <!-- Danger Zone / Key Generation Section -->
      <div
        class="mt-10 border border-error-200 dark:border-error-900/50 bg-error-50/50 dark:bg-error-900/10 rounded-lg p-5"
      >
        <h3 class="text-sm font-semibold text-error-900 dark:text-error-400">
          Regenerate Secret Credentials
        </h3>
        <p class="text-xs text-neutral-600 dark:text-neutral-400 mt-1">
          Generating a new private key will immediately invalidate your active server workflows
          using the current key. This action cannot be undone.
        </p>
        <div class="mt-4">
          <UButton
            color="error"
            variant="solid"
            icon="i-heroicons-arrow-path"
            @click="generateNewKey"
          >
            Generate New Private Key
          </UButton>
        </div>
      </div>
    </div>
  </div>
</template>
