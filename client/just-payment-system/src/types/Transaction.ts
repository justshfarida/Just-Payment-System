// Define the shape of your transaction data
export interface Transaction {
  id: string
  date: string
  status: 'completed' | 'pending' | 'failed'
  amount: number
  card_mask: string
  currency: string
  payload: Record<string, any>
}
