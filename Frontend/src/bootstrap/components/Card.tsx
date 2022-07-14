import { FC } from 'preact/compat';
import { format, SimpleProps } from '../types';

interface CardProps extends SimpleProps {
  header?: JSX.Element|string
}

const Card : FC<CardProps> = (props) => {
  const { header } = props;
  const theme = format(props.theme, 'bg');
  return <div className={`card ${theme} ${props.className??''}`} style={props.style ?? ''}>
    {header && <div className="card-header">{header}</div>}
    <div className="card-body">
      {props.children}
    </div>
  </div>
}

export default Card;