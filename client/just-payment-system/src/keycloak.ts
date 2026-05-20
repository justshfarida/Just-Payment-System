import Keycloak from 'keycloak-js'

export const keycloak = new Keycloak({
  url: 'http://localhost:8080',
  realm: 'JustPay',
  clientId: 'just-pay-vue',
})
