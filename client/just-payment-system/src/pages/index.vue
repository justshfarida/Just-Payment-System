<!-- pages/index.vue -->
<template>
  <div class="min-h-screen bg-gray-950 text-white">
    <!-- Navbar -->
    <header class="border-b border-gray-800">
      <UContainer class="flex items-center justify-between py-4">
        <div class="flex items-center gap-2">
          <div class="size-3 rounded-full bg-primary"></div>
          <span class="text-lg font-bold">MockPay</span>
        </div>

        <div class="flex items-center gap-3">
          <UButton color="neutral" variant="ghost" label="Documentation" />

          <UButton color="primary" label="Get Started" />
        </div>
      </UContainer>
    </header>

    <!-- Hero -->
    <section class="relative overflow-hidden">
      <div
        class="absolute inset-0 bg-gradient-to-br from-primary/20 via-transparent to-cyan-500/10"
      />

      <UContainer class="relative py-28">
        <div class="mx-auto max-w-4xl text-center">
          <UBadge
            label="Developer Friendly Payment Simulator"
            variant="soft"
            color="primary"
            class="mb-6"
          />

          <h1 class="text-5xl font-extrabold tracking-tight md:text-7xl">
            Build & Test
            <span class="text-primary">Payment Flows</span>
            Without Real Banks
          </h1>

          <p class="mx-auto mt-6 max-w-2xl text-lg text-gray-400">
            MockPay helps developers simulate transactions, callbacks, refunds, failures, and
            payment processing flows for testing and development.
          </p>

          <div class="mt-10 flex flex-col items-center justify-center gap-4 sm:flex-row">
            <UButton size="xl" color="primary" icon="i-lucide-play" label="Start Testing" />

            <UButton
              size="xl"
              color="neutral"
              variant="outline"
              icon="i-lucide-book-open"
              label="API Docs"
            />
          </div>

          <!-- Fake Payment Card -->
        </div>
      </UContainer>
    </section>

    <!-- Features -->
    <section class="border-t border-gray-800 bg-black/20 py-24">
      <UContainer>
        <div class="text-center">
          <h2 class="text-4xl font-bold">Everything You Need For Payment Testing</h2>

          <p class="mt-4 text-gray-400">
            Designed for backend engineers, frontend developers, and QA teams.
          </p>
        </div>

        <div class="mt-16 grid gap-6 md:grid-cols-3">
          <UCard
            v-for="feature in features"
            :key="feature.title"
            class="border border-white/10 bg-white/5"
          >
            <template #header>
              <div class="flex size-12 items-center justify-center rounded-xl bg-primary/10">
                <UIcon :name="feature.icon" class="text-2xl text-primary" />
              </div>
            </template>

            <h3 class="text-xl font-semibold">
              {{ feature.title }}
            </h3>

            <p class="mt-3 text-gray-400">
              {{ feature.description }}
            </p>
          </UCard>
        </div>
      </UContainer>
    </section>

    <!-- CTA -->
    <section class="py-24">
      <UContainer>
        <div class="rounded-3xl border border-primary/20 bg-primary/10 px-8 py-16 text-center">
          <h2 class="text-4xl font-bold">Start Building Today</h2>

          <p class="mx-auto mt-4 max-w-2xl text-gray-300">
            Integrate your payment flows in minutes with a modern mock payment gateway.
          </p>

          <div class="mt-8 flex justify-center gap-4">
            <UButton size="xl" color="primary" label="Create Sandbox" />

            <UButton size="xl" color="neutral" variant="outline" label="Read Docs" />
          </div>
        </div>
      </UContainer>
    </section>

    <!-- Footer -->
    <footer class="border-t border-gray-800 py-8">
      <UContainer
        class="flex flex-col items-center justify-between gap-4 text-sm text-gray-500 md:flex-row"
      >
        <p>© 2026 MockPay. All rights reserved.</p>

        <div class="flex items-center gap-4">
          <a href="#">Documentation</a>
          <a href="#">API</a>
          <a href="#">GitHub</a>
        </div>
      </UContainer>
    </footer>
  </div>
</template>

<script setup lang="ts">
const features = [
  {
    title: 'Mock Transactions',
    description: 'Simulate successful, failed, and pending payments instantly.',
    icon: 'i-lucide-credit-card',
  },
  {
    title: 'Webhook Testing',
    description: 'Send fake callbacks and test your backend integrations safely.',
    icon: 'i-lucide-webhook',
  },
  {
    title: 'Developer First',
    description: 'Simple APIs, clean documentation, and fast sandbox setup.',
    icon: 'i-lucide-code-2',
  },
]
</script>
