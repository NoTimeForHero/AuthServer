import { format, SimpleProps, Theme } from '../types';
import { FC } from 'preact/compat';

interface Props extends SimpleProps {
  theme?: Theme
}

const Alert : FC<Props> = (props) => {
  const theme = format(props.theme, 'alert', 'alert-info');
  return <div className={`alert ${theme} ${props.className??''}`} style={props.style}>
    {props.children}
  </div>
}

export default Alert;