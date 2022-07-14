import { FC } from 'preact/compat';
import { Company } from '../companies';
import x from './ProviderButton.module.css';

interface ButtonProps {
  user?: string,
  company: Company,
  onClick?: (ev: JSX.TargetedMouseEvent<HTMLDivElement>) => void,
}

const ProviderButton : FC<ButtonProps> = (props) => {
  const { company, user } = props;
  const [icon,title] = company;
  const onClick = props.onClick ?? (() => {});
  return <div onClick={onClick}
    className={`btn btn-outline-primary btn-sm ${x.button}`}>
    <img src={icon} className={x.icon} alt="" />
    <div className={x.title}>
      <span>{user ? '' : 'Войти через'}   </span>
      <span className="name">{user ?? title}</span>
    </div>
  </div>
}

export default ProviderButton;