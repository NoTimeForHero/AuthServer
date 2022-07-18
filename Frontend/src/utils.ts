import { useEffect } from 'preact/compat';

export const wait = (timeout: number) : Promise<void> =>
  new Promise((resolve) => setTimeout(resolve, timeout));

export const useMount = (callback: () => void|Promise<void>) => {
  useEffect(() => {
    // noinspection JSIgnoredPromiseFromCall
    callback();
  }, []);
}