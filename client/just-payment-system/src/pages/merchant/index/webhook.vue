<script setup lang="ts">
import { ref } from 'vue'

// 1. Define the interface matching your task schema
interface WebhookFormState {
  callback_url: string
  success_url: string
  error_url: string
}

// Initial form state
const state = ref<WebhookFormState>({
  callback_url: 'https://api.yourdomain.com/v1/webhooks/payments',
  success_url: 'https://yourdomain.com/checkout/success',
  error_url: 'https://yourdomain.com/checkout/failed',
})

const isSaving = ref(false)

// 2. Client-side Form Validation
const validate = (formState: WebhookFormState) => {
  const errors: { path: string; message: string }[] = []

  // Helper logic to verify standard URL schemas
  const isValidUrl = (string: string) => {
    try {
      new URL(string)
      return true
    } catch (_) {
      return false
    }
  }

  // callback_url rule: Must be present AND a valid URL
  if (!formState.callback_url) {
    errors.push({ path: 'callback_url', message: 'Callback URL is required' })
  } else if (!isValidUrl(formState.callback_url)) {
    errors.push({
      path: 'callback_url',
      message: 'Must be a valid absolute URL (e.g., https://...)',
    })
  }

  // Optional fields: only validate structure if text has been inputted
  if (formState.success_url && !isValidUrl(formState.success_url)) {
    errors.push({ path: 'success_url', message: 'Must be a valid absolute URL' })
  }
  if (formState.error_url && !isValidUrl(formState.error_url)) {
    errors.push({ path: 'error_url', message: 'Must be a valid absolute URL' })
  }

  return errors
}

// 3. Form Submit Handler
const onSubmit = async () => {
  isSaving.value = true

  // Simulate API Network Request delay
  await new Promise((resolve) => setTimeout(resolve, 1000))

  isSaving.value = false
  // Pro-tip: Trigger your success notification here via useToast() if installed!
  alert('Webhook URLs updated successfully!')
}
</script>

<template>
  <div class="max-w-3xl mx-auto py-10 px-4 sm:px-6">
    <!-- Top Context Header Section -->
    <div class="mb-8 border-b border-neutral-200 dark:border-neutral-800 pb-5">
      <h1 class="text-2xl font-semibold text-neutral-900 dark:text-neutral-100">
        Webhook Endpoints
      </h1>
      <p class="mt-1 text-sm text-neutral-500 dark:text-neutral-400">
        Configure where our servers should send HTTP POST event payloads when transaction states
        mutate.
      </p>
    </div>

    <!-- Nuxt UI Form Component -->
    <UForm :state="state" :validate="validate" @submit="onSubmit" class="space-y-6">
      <!-- 1. Callback URL (Required) -->
      <UFormField
        name="callback_url"
        label="Callback URL"
        required
        description="The primary destination where raw transaction payloads are delivered in real time."
      >
        <UInput
          v-model="state.callback_url"
          placeholder="https://api.yourdomain.com/webhooks"
          class="w-full mt-1 font-mono text-sm"
        />
      </UFormField>

      <!-- 2. Success Redirect URL (Optional) -->
      <UFormField
        name="success_url"
        label="Success Redirect URL"
        description="Where customers are automatically routed after a completed or validated payment transaction."
      >
        <UInput
          v-model="state.success_url"
          placeholder="https://yourdomain.com/checkout/success"
          class="w-full mt-1 font-mono text-sm"
        />
      </UFormField>

      <!-- 3. Error Redirect URL (Optional) -->
      <UFormField
        name="error_url"
        label="Error Redirect URL"
        description="Where customers are sent if a payment processing attempt fails or is declined."
      >
        <UInput
          v-model="state.error_url"
          placeholder="https://yourdomain.com/checkout/failed"
          class="w-full mt-1 font-mono text-sm"
        />
      </UFormField>

      <!-- Form Submission Zone -->
      <div class="pt-4 border-t border-neutral-200 dark:border-neutral-800 flex justify-end">
        <UButton
          type="submit"
          color="primary"
          variant="solid"
          :loading="isSaving"
          icon="i-heroicons-cloud-arrow-up"
        >
          Save Endpoints
        </UButton>
      </div>
    </UForm>
  </div>
</template>
