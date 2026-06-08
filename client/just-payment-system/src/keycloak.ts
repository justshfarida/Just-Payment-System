import Keycloak from 'keycloak-js'

export const keycloak = new Keycloak({
  url: 'http://localhost:5055/auth',
  realm: 'JustPay',
  clientId: 'just-pay-vue',
})
