import Card from '../../components/bootstrap/Card';
import ProviderButton from '../../components/ProviderButton';
import { Companies } from '../../companies';
import { FC, Fragment } from 'preact/compat';
import Header from '../../components/Header';
import { useAtom } from 'jotai';
import { authorizeAtom, settingsAtom, useStatusBar } from '../../api/store';
import StatusBar from '../StatusBar';
import ContinueBlock from './ContinueBlock';
import { useMount } from '../../utils';
import { fetchAuthorize } from '../../api';

interface LoginButtonsProps {
  path?: string // Router
  matches?: Record<string,string>
}

const Body = () => {
  const [settings] = useAtom(settingsAtom);
  const [authorize] = useAtom(authorizeAtom);
  if (!settings || !authorize) return <Fragment />

  const { providers = [] } = settings;
  const onLogin = (name: string) => () =>
    document.location = `https://localhost:3002/api/login?provider=${name}`;

  return <>
    <ContinueBlock settings={settings} />

    <div className="social-buttons-group">
      {providers.map((name) =>
        <ProviderButton
          onClick={onLogin(name)}
          company={Companies[name]}
          key={name} />
      )}
    </div>
  </>

}

const LoginButtons : FC<LoginButtonsProps> = (props) => {
  const [authorize, setAuthorize] = useAtom(authorizeAtom);
  const { call } = useStatusBar();

  const onMount = async() => {
    if (authorize) return;
    const { app, redirect } = props.matches ?? {};
    const auth = await call(fetchAuthorize, app, redirect);
    setAuthorize(auth);
  }
  useMount(onMount);

  return <Card className="login-block">

    <div className="d-flex justify-content-center">
      <Header auth={authorize} />
    </div>

    <hr/>

    <StatusBar />
    <Body />

  </Card>
}

export default LoginButtons;