import { FC, Fragment } from 'preact/compat';
import { Settings } from '../../types';
import { Companies } from '../../companies';
import ProviderButton from '../../components/ProviderButton';
import { Link } from 'preact-router/match';
import { authorizeAtom, useStatusBar } from '../../api/store';
import { useAtom } from 'jotai';
import { fetchAccess } from '../../api';

const ContinueBlock : FC<{settings?: Settings}> = (props) => {
  const { user } = props?.settings ?? {};
  const { call } = useStatusBar();
  const [auth] = useAtom(authorizeAtom);
  if (!user) return <Fragment />
  const company = Companies[user.provider];
  const onContinue = async() => {
    if (!auth) return;
    const res = await call(fetchAccess, auth.application.id, auth.redirect);
    if (res.redirect) document.location = res.redirect;
  }
  return <>
    <div className="text-center fw-bold mb-2">Продолжить как:</div>
    <div className="d-flex m-2">
      <ProviderButton company={company} user={user} onClick={onContinue} />
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