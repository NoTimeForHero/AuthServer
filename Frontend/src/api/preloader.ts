import { Settings } from '../types';
import { useAtom } from 'jotai';
import { settingsAtom } from './store';

interface ExtendedWindow extends Window {
  preload?: {
    settings: Settings
  }
}

export const usePreloader = () => {

  const preload = (window as ExtendedWindow).preload;
  console.log('PRELOAD DATA', preload);
  if (!preload) return { hasSettings: false }

  const [,setSettings] = useAtom(settingsAtom);
  setSettings(preload.settings);

  return { hasSettings: true }
}