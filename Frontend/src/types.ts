
export interface Claim {
  type: string,
  value: string,
  valueType: string,
  subject: string,
}

export interface Settings {
  brand?: Brand,
  user?: User,
  providers: string[]
}

export interface Brand {
  name?: string,
  logotype?: string,
  size?: {x: number, y: number},
}

export interface User {
  provider: string,
  id: string,
  displayName: string,
  fullId: string
}