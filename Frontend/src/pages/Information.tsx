import { FC, useEffect } from 'preact/compat';
import Card from '../components/bootstrap/Card';
import { Claim } from '../types';
import { fetchClaims} from '../api';
import { atom, useAtom } from 'jotai';
import { useStatusBar } from '../api/store';
import StatusBar from './StatusBar';

export const formatValue = (input: string) : JSX.Element|string => {
  const byAnchor = input.split("#");
  if (byAnchor.length > 1) return <a href={input}>{byAnchor[1]}</a>;
  if (input.startsWith("http://schemas.xmlsoap.org/")) {
    const parts = input.split("/");
    return <a href={input}>{parts[parts.length-1]}</a>
  }
  return input;
}

export const ShowClaim = ({claims} : {claims: Claim[]}) => {
  return <table class="table table-borderless my-table">
    <tbody>
      <tr>
        <th>Тип:</th>
        <th>Поле:</th>
        <th>Значение:</th>
      </tr>
      {claims.map((claim, id) => (
        <tr key={id}>
          <td>{formatValue(claim.valueType)}</td>
          <td>{formatValue(claim.type)}</td>
          <td>{formatValue(claim.value)}</td>
        </tr>
      ))}
    </tbody>
  </table>
}

interface InformationProps {
  path?: string // Router
}

const claimsAtom = atom<Claim[]|undefined>(undefined);

const Information : FC<InformationProps> = () => {
  const onBack = () => history.go(-1);
  const [claims, setClaims] = useAtom(claimsAtom);
  const { call } = useStatusBar();

  const onLoad = async() => {
    if (claims) return;
    const entries = await call(fetchClaims);
    // TODO: Показывать остальные вкладки?
    const newClaims = Object.values(entries)[0];
    setClaims(newClaims);
  };
  useEffect(() => { onLoad() }, []);

  return <Card header="Информация о пользователе">
    <StatusBar />
    {claims && <ShowClaim claims={claims} />}

    <div className="d-flex justify-content-center">
      <div className="btn btn-primary px-4 py-2" onClick={onBack}>Назад?</div>
    </div>
  </Card>
}

export default Information;