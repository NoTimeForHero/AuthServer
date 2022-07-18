import Card from '../bootstrap/components/Card';
import ProviderButton from '../components/ProviderButton';
import { Companies } from '../companies';
import { FC, Fragment } from 'preact/compat';
import Header from '../components/Header';
import { Settings } from '../types';
import { Link } from 'preact-router/match';
import { useAtom } from 'jotai';
import { settingsAtom } from '../api/store';
import StatusBar from './StatusBar';

const ContinueBlock : FC<{settings?: Settings}> = (props) => {
  const { user } = props?.settings ?? {};
  if (!user) return <Fragment />
  const company = Companies[user.provider];
  return <>
    <div className="text-center fw-bold mb-2">Продолжить как:</div>
    <div className="d-flex m-2">
      <ProviderButton company={company} user={user} />
      <Link className="btn btn-sm btn-outline-info d-flex"
           href="/info"
           title="Информация о пользователе">
        <img src="/images/question-mark-svgrepo-com.svg" alt="?" width={32} />
      </Link>
    </div>
    <hr />
  </>
}

interface LoginButtonsProps {
  path?: string // Router
  matches?: Record<string,string>
}

const LoginButtons : FC<LoginButtonsProps> = () => {
  const [settings] = useAtom(settingsAtom);
  const { providers = [] } = settings ?? {};

  const onLogin = (name: string) => () =>
    document.location = `https://localhost:3002/api/login?provider=${name}`;

  return <Card className="login-block">

    <StatusBar />

    <div className="d-flex justify-content-center">
      <Header />
    </div>

    <hr/>

    <ContinueBlock settings={settings} />

    <div className="social-buttons-group">
      {providers.map((name) =>
        <ProviderButton
          onClick={onLogin(name)}
          company={Companies[name]}
          key={name} />
      )}
    </div>

  </Card>
}

export default LoginButtons;