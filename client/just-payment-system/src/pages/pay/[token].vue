<script setup lang="ts">
import { ref, reactive } from 'vue'
import { z } from 'zod'

// 1. Define the Zod Validation Schema
const paymentSchema = z.object({
  cardNumber: z.string().regex(/^\d{16}$/, 'Card number must be exactly 16 digits'),
  expiry: z
    .string()
    .regex(/^(0[1-9]|1[0-2])\/?([0-9]{2})$/, 'Expiry must be in MM/YY format')
    .refine((val) => {
      const [month, year] = val.split('/').map(Number)
      if (!month || !year) return false
      const fullYear = 2000 + year
      const expiryDate = new Date(fullYear, month, 0)
      return expiryDate >= new Date()
    }, 'Card has expired'),
  cvv: z.string().regex(/^\d{3,4}$/, 'CVV must be 3 or 4 digits'),
})

// 2. Form Reactive States
const state = reactive({
  cardNumber: '',
  expiry: '',
  cvv: '',
})

// Create a reactive object to hold error messages mapped to keys
const errors = reactive<Record<string, string>>({
  cardNumber: '',
  expiry: '',
  cvv: '',
})

const isSubmitting = ref(false)

// 3. Clear single field error on input
const clearError = (field: keyof typeof state) => {
  errors[field] = ''
}

// 4. Form Submit Handler
async function onSubmit() {
  // Clear previous errors
  Object.keys(errors).forEach((key) => (errors[key] = ''))

  // Validate using Zod
  const result = paymentSchema.safeParse(state)

  if (!result.success) {
    // Map Zod errors to our local state errors
    result.error.issues.forEach((issue) => {
      const path = issue.path[0] as string
      errors[path] = issue.message
    })
    return
  }

  // Validated data safely extracted
  console.log('Validated Payment Data:', result.data)

  isSubmitting.value = true
  try {
    // Simulate API Call to Payment Gateway
    await new Promise((resolve) => setTimeout(resolve, 2000))
    alert('Payment Processed Successfully!')
  } catch (error) {
    console.error('Payment failed:', error)
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <div
    class="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900 p-4 font-sans text-gray-900 dark:text-gray-100"
  >
    <div
      class="w-full max-w-md bg-white dark:bg-gray-800 rounded-xl shadow-xl border border-gray-100 dark:border-gray-700 p-6"
    >
      <div class="mb-6">
        <div class="flex items-center gap-3">
          <UIcon name="i-solar-card-outline" class="size-5"></UIcon>
          <h1 class="text-xl font-bold">Secure Payment</h1>
        </div>
        <p class="text-sm text-gray-500 dark:text-gray-400 mt-1">
          Please enter your payment details below.
        </p>
      </div>

      <form @submit.prevent="onSubmit" class="space-y-4">
        <div class="flex flex-col gap-1.5">
          <label class="text-sm font-medium">Card Number <span class="text-red-500">*</span></label>
          <input
            v-model="state.cardNumber"
            type="text"
            placeholder="1234567812345678"
            maxlength="16"
            autocomplete="cc-number"
            @input="clearError('cardNumber')"
            class="w-full px-3 py-2 border rounded-lg shadow-sm focus:outline-none focus:ring-2 bg-transparent transition"
            :class="
              errors.cardNumber
                ? 'border-red-500 focus:ring-red-200'
                : 'border-gray-300 dark:border-gray-600 focus:ring-indigo-200 focus:border-indigo-500'
            "
          />
          <p v-if="errors.cardNumber" class="text-xs text-red-500 font-medium">
            {{ errors.cardNumber }}
          </p>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div class="flex flex-col gap-1.5">
            <label class="text-sm font-medium"
              >Expires (MM/YY) <span class="text-red-500">*</span></label
            >
            <input
              v-model="state.expiry"
              type="text"
              placeholder="MM/YY"
              maxlength="5"
              autocomplete="cc-exp"
              @input="clearError('expiry')"
              class="w-full px-3 py-2 border rounded-lg shadow-sm focus:outline-none focus:ring-2 bg-transparent transition"
              :class="
                errors.expiry
                  ? 'border-red-500 focus:ring-red-200'
                  : 'border-gray-300 dark:border-gray-600 focus:ring-indigo-200 focus:border-indigo-500'
              "
            />
            <p v-if="errors.expiry" class="text-xs text-red-500 font-medium">{{ errors.expiry }}</p>
          </div>

          <div class="flex flex-col gap-1.5">
            <label class="text-sm font-medium">CVV <span class="text-red-500">*</span></label>
            <input
              v-model="state.cvv"
              type="password"
              placeholder="123"
              maxlength="4"
              autocomplete="cc-csc"
              @input="clearError('cvv')"
              class="w-full px-3 py-2 border rounded-lg shadow-sm focus:outline-none focus:ring-2 bg-transparent transition"
              :class="
                errors.cvv
                  ? 'border-red-500 focus:ring-red-200'
                  : 'border-gray-300 dark:border-gray-600 focus:ring-indigo-200 focus:border-indigo-500'
              "
            />
            <p v-if="errors.cvv" class="text-xs text-red-500 font-medium">{{ errors.cvv }}</p>
          </div>
        </div>

        <div class="pt-4">
          <UButton
            type="submit"
            :disabled="isSubmitting"
            class="w-full flex items-center justify-center text-white font-medium py-2.5 px-4 rounded-lg shadow dynamic-btn transition disabled:opacity-50 disabled:cursor-not-allowed"
          >
            <svg
              v-if="isSubmitting"
              class="animate-spin -ml-1 mr-3 h-5 w-5 text-white"
              fill="none"
              viewBox="0 0 24 24"
            >
              <circle
                class="opacity-25"
                cx="12"
                cy="12"
                r="10"
                stroke="currentColor"
                stroke-width="4"
              ></circle>
              <path
                class="opacity-75"
                fill="currentColor"
                d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
              ></path>
            </svg>
            {{ isSubmitting ? 'Processing...' : 'Pay Now' }}
          </UButton>
        </div>
      </form>
    </div>
  </div>
</template>
