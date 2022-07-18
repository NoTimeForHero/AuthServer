import { useAtom } from 'jotai';
import { settingsAtom } from '../api/store';

const Header = () => {
  const [settings] = useAtom(settingsAtom);
  const { brand = {} } = settings ?? {};

  if (brand.logotype) {
    const size = (brand.size) ? {width: brand.size.x, height: brand.size.y} : {};
    return <img src={brand.logotype} style={size} alt={brand.name ?? ""} />;
  }

  return <h5>
    {brand.name ?? "Вход через социальные сети"}
  </h5>;
}

export default Header;