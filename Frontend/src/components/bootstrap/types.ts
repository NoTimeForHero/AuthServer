export type Theme = 'primary'|'secondary'|'success'|'danger'|'warning'|'info'|'light'|'dark';

export type Size = 'xs'|'sm'|'md'|'lg'|'xl';

export type StyleType = string|Record<string,any>;

export interface SimpleProps {
  className?: string,
  theme?: Theme
  style?: StyleType
}

export const format = <TEnum extends string>(input: TEnum|undefined, format: string, defVal: string = '') : string =>
  (typeof input === 'undefined') ? defVal : `${format}-${input}`;