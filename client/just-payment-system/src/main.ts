import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import '../assets/styles/index.css'
import ui from '@nuxt/ui/vue-plugin'
import { createPinia } from 'pinia'

const pinia = createPinia()
const app = createApp(App)

app.use(pinia)
app.use(router)
app.use(ui)
app.mount('#app')
