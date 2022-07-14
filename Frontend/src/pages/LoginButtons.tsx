import Card from '../bootstrap/components/Card';
import ProviderButton from '../components/ProviderButton';
import { Companies } from '../companies';

const LoginButtons = () => {
  return <Card className="login-block">

    <div className="d-flex justify-content-center">

      <h5>Logotype or brand name</h5>
    </div>

    <hr/>
    <div className="text-center fw-bold mb-2">Продолжить как:</div>
    <ProviderButton company={Companies.MailRu} user="Example User" />
    <hr />

    <ProviderButton company={Companies.GitHub} />
    <ProviderButton company={Companies.Yandex} />

  </Card>
}

export default LoginButtons;