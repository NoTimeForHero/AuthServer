import Card from '../../components/bootstrap/Card';
import ProviderButton from '../../components/ProviderButton';
import { Companies } from '../../companies';
import { FC, Fragment } from 'preact/compat';
import Header from '../../components/Header';
import { useAtom } from 'jotai';
import { authorizeAtom, loadingAtom, settingsAtom, useStatusBar } from '../../api/store';
import StatusBar from '../StatusBar';
import ContinueBlock from './ContinueBlock';
import { useMount } from '../../utils';
import { doLogin, fetchAuthorize } from '../../api';

interface LoginButtonsProps {
  path?: string // Router
  matches?: Record<string,string>
}

const Body = () => {
  const [settings] = useAtom(settingsAtom);
  const [loading,setLoading] = useAtom(loadingAtom);
  if (!settings || loading) return <Fragment />

  const { providers = [] } = settings;
  const onLogin = (name: string) => () => {
    doLogin(name);
    setLoading(true);
  }

  return <>
    <ContinueBlock {...{settings}} />

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

    <Header auth={authorize} />

    <hr/>

    <StatusBar />
    <Body />

  </Card>
}

export default LoginButtons;