import { useContext } from 'preact/compat';
import { globalSettingsCtx } from '../store/GlobalData';

const Header = () => {
  const settings = useContext(globalSettingsCtx);
  const { brand = {} } = settings ?? {};

  if (brand.logotype) {
    const size = (brand.size) ? {width: brand.size.x, height: brand.size.y} : {};
    console.warn(size, brand);
    return <img src={brand.logotype} style={size} alt={brand.name ?? ""} />;
  }

  return <h5>
    {brand.name ?? "Вход через социальные сети"}
  </h5>;
}

export default Header;