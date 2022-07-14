import { FC } from 'preact/compat';
import { Theme, StyleType } from '../types';

interface CardProps {
  className?: string,
  header?: JSX.Element|string
  theme?: Theme
  style?: StyleType
}

const Card : FC<CardProps> = (props) => {
  const { header } = props;
  const theme = props.theme ? `bg-${props.theme}` : '';
  return <div className={`card ${theme} ${props.className??''}`} style={props.style ?? ''}>
    {header && <div className="card-header">{header}</div>}
    <div className="card-body">
      {props.children}
    </div>
  </div>
}

export default Card;