import { useAtom } from 'jotai';
import { errorAtom, loadingAtom } from '../api/store';
import { Fragment } from 'preact/compat';
import Alert from '../components/bootstrap/Alert';

const StatusBar = () => {
  const [loading] = useAtom(loadingAtom);
  const [error] = useAtom(errorAtom);
  return <Fragment>
    {error && <Alert theme='danger'>
        <strong>Ошибка: </strong>
      {error?.message ?? error?.toString()}
    </Alert>}
    {loading && <Alert>Загрузка данных...</Alert>}
  </Fragment>
}

export default StatusBar;
