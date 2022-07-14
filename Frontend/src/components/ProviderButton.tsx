import { FC } from 'preact/compat';
import { Company } from '../companies';
import x from './ProviderButton.module.css';
import { User } from '../types';

interface ButtonProps {
  user?: User,
  company: Company,
  onClick?: (ev: JSX.TargetedMouseEvent<HTMLDivElement>) => void,
}

const ProviderButton : FC<ButtonProps> = (props) => {
  const { company, user } = props;
  const [icon,title] = company;
  const onClick = props.onClick ?? (() => {});
  return <div onClick={onClick}
    className={`btn btn-outline-primary btn-sm ${x.button} w-100`}>
    <img src={icon} className={x.icon} alt="" />
    <div className={x.title}>
      {user ? <>
        <span className="name">{user.displayName} </span>
        <span>({user.id})</span>
      </> : <>
        <span>Войти через </span>
        <span className="name">{title}</span>
      </>}
    </div>
  </div>
}

export default ProviderButton;