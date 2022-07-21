import { useEffect } from 'preact/compat';

export const wait = (timeout: number) : Promise<void> =>
  new Promise((resolve) => setTimeout(resolve, timeout));

export const useMount = (callback: () => void|Promise<void>) => {
  useEffect(() => {
    // noinspection JSIgnoredPromiseFromCall
    callback();
  }, []);
}

export const notEmpty = <T>(pair: [any,T|undefined]): pair is [any,T] => pair[1] !== undefined;

export const buildURL = (url: string, params?: Record<string,string|undefined>) : string => {
  if (!params) return url;
  const validParams = Object.entries(params).filter(notEmpty);
  const query = new URLSearchParams(validParams).toString();
  return `${url}?${query}`;
}