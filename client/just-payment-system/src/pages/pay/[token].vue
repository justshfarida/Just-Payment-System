<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const token = route.path
</script>
<template>
  {{ token }}
</template>
