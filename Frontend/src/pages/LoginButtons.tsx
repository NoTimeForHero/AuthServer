import Card from '../bootstrap/components/Card';
import ProviderButton from '../components/ProviderButton';
import { Companies } from '../companies';
import { FC, Fragment, useContext } from 'preact/compat';
import { globalSettingsCtx } from '../context/GlobalData';
import Header from '../components/Header';
import { Settings } from '../types';
import { activePageCtx } from '../context/ActivePage';
import Information from './Information';

const ContinueBlock : FC<{settings?: Settings}> = (props) => {
  const setActivePage = useContext(activePageCtx);
  const { user } = props?.settings ?? {};
  if (!user) return <Fragment />
  const company = Companies[user.provider];
  const onInfo = () => setActivePage(<Information />);
  return <>
    <div className="text-center fw-bold mb-2">Продолжить как:</div>
    <div className="d-flex m-2">
      <ProviderButton company={company} user={user} />
      <div className="btn btn-sm btn-outline-info d-flex"
           onClick={onInfo}
           title="Информация о пользователе">
        <img src="/images/question-mark-svgrepo-com.svg" alt="?" width={32} />
      </div>
    </div>
    <hr />
  </>
}

const LoginButtons = () => {
  const settings = useContext(globalSettingsCtx);
  const { providers = [] } = settings ?? {};

  const onLogin = (name: string) => () =>
    document.location = `https://localhost:3002/api/login?provider=${name}`;

  return <Card className="login-block">

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