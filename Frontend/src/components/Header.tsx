import { useAtom } from 'jotai';
import { settingsAtom } from '../api/store';
import { FC, Fragment } from 'preact/compat';
import { Authorize } from '../api';
import { Brand } from '../types';

interface Props {
  auth?: Authorize
}

const Image : FC<{brand: Brand}> = ({brand}) => {
  if (!brand.logotype) return <Fragment />;
  const size = (brand.size) ? {width: brand.size.x, height: brand.size.y} : {};
  return <img src={brand.logotype} style={size} alt={brand.name ?? ""} />;
}

const AppBlock : FC<{auth?: Authorize}> = ({auth}) => {
  if (!auth) return <Fragment />;
  const app = auth.application;
  return <h4 className="text-center mt-3">
    <span>Доступ к </span>
    "<a href={app.baseURL}>{app.title}</a>"
  </h4>
}

const Header : FC<Props> = (props) => {
  const [settings] = useAtom(settingsAtom);
  const { brand = {} } = settings ?? {};

  return <div>
    {brand.logotype
      ? <Image brand={brand} />
      : <h3>{brand.name ?? "OAuth2 Server"}</h3>}
    <AppBlock auth={props.auth} />
  </div>;
}

export default Header;