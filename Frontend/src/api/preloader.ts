import { Settings } from '../types';
import { useAtom, WritableAtom } from 'jotai';
import { authorizeAtom, errorAtom, settingsAtom } from './store';
import { Authorize } from './index';

interface ExtendedWindow extends Window {
  preload?: {
    settings: Settings
    error?: any,
    auth?: Authorize
  }
}

const TryUse = <T,>(atom: WritableAtom<T, T>, value:T|undefined) => {
  if (value === undefined) return;
  useAtom(atom)[1](value);
}

export const usePreloader = () => {

  const preload = (window as ExtendedWindow).preload;
  console.log('PRELOAD DATA', preload);
  if (!preload) return { hasSettings: false }

  useAtom(settingsAtom)[1](preload.settings);
  TryUse(errorAtom, preload.error);
  TryUse(authorizeAtom, preload.auth);

  return { hasSettings: true }
}