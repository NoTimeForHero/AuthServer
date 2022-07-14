
export interface ExtendedWindow extends Window {
  settings?: Settings
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
  username: string,
  combined: string
}