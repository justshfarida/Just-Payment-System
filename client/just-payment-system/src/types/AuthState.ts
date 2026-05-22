import type { UserInfo } from './User'

export interface AuthState {
  initialized: boolean
  authenticated: boolean
  user: UserInfo | null
}
