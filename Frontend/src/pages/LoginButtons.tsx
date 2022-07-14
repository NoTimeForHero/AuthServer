import Card from '../bootstrap/components/Card';
import ProviderButton from '../components/ProviderButton';
import { Companies } from '../companies';
import { FC, Fragment, useContext } from 'preact/compat';
import { globalSettingsCtx } from './GlobalData';
import Header from '../components/Header';
import { Settings } from '../types';

const ContinueBlock : FC<{settings?: Settings}> = (props) => {
  const { user } = props?.settings ?? {};
  if (!user) return <Fragment />
  const company = Companies[user.provider];
  return <>
    <div className="text-center fw-bold mb-2">Продолжить как:</div>
    <ProviderButton company={company} user={user.username} />
    <hr />
  </>
}

const LoginButtons = () => {
  const settings = useContext(globalSettingsCtx);
  const { providers = [] } = settings ?? {};

  const onLogin = (name: string) => () => {
    document.location = `https://localhost:3002/api/login?provider=${name}`;
  }

  return <Card className="login-block">

    <div className="d-flex justify-content-center">
      <Header />
    </div>

    <hr/>

    <ContinueBlock settings={settings} />

    {providers.map((name) =>
      <ProviderButton
        onClick={onLogin(name)}
        company={Companies[name]}
        key={name} />
    )}

  </Card>
}

export default LoginButtons;