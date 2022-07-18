import { FC } from 'preact/compat';
import { SimpleProps, format, Size } from './types';


interface ContainerProps extends SimpleProps {
  size?: Size
}

const Container : FC<ContainerProps> = (props) => {
  const size = format(props.size, 'container', 'container');
  return <div className={`${size} ${props.className??''}`} style={props.style}>{props.children}</div>
}

export default Container;