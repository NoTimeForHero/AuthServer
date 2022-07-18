import { FC, Fragment } from 'preact/compat';
import { Settings } from '../../types';
import { Companies } from '../../companies';
import ProviderButton from '../../components/ProviderButton';
import { Link } from 'preact-router/match';

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

export default ContinueBlock;