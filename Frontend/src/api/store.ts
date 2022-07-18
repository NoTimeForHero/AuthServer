import { atom, useAtom } from 'jotai'
import { Settings } from '../types';

export const settingsAtom = atom<Settings|undefined>(undefined);
export const loadingAtom = atom(false);
export const errorAtom = atom<any>(undefined);

export const useStatusBar = () => {
  const [, setLoading] = useAtom(loadingAtom);
  const [, setError] = useAtom(errorAtom);

  const call = async <T extends Array<any>, U>(fn: (...args: T) => Promise<U>, ...args: T) => {
    try {
      setLoading(true);
      setError(undefined);
      // await wait(2000);
      return await fn(...args);
    } catch (ex) {
      setError(ex);
      throw ex;
    } finally {
      setLoading(false);
    }
  }
  return {call}
}