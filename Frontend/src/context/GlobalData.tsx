import { createContext, FC, useEffect, useState } from 'preact/compat';
import { Settings } from '../types';
import Container from '../bootstrap/components/Container';
import Alert from '../bootstrap/components/Alert';

export const globalSettingsCtx = createContext<Settings|undefined>(undefined);

export const LoadWrapper : FC = (props) => {

  const [settings, setSettings] = useState<Settings>();

  const onMount = async() => setSettings(await fetch('/api/settings').then(x => x.json()));
  useEffect(() => { onMount() }, []);

  if (!settings) return <Container size={'sm'} className="pt-3">
    <Alert>Загрузка данных...</Alert>
  </Container>;

  return <globalSettingsCtx.Provider value={settings}>{props.children}</globalSettingsCtx.Provider>;
}