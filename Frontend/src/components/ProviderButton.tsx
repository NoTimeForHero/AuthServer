import { FC } from 'preact/compat';
import { Company } from '../companies';
import x from './ProviderButton.module.css';

interface ButtonProps {
  user?: string,
  company: Company
}

const ProviderButton : FC<ButtonProps> = (props) => {
  const { company, user } = props;
  return <div className={`btn btn-outline-primary btn-sm ${x.button}`}>
    <img src={company.icon} className={x.icon} />
    <div className={x.title}>
      <span>{user ? '' : 'Войти через'}   </span>
      <span className="name">{user ?? company.title}</span>
    </div>
  </div>
}

export default ProviderButton;