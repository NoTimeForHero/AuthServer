import { FC } from 'preact/compat';
import { Size, StyleType } from '../types';


interface ContainerProps {
  style?: StyleType,
  className?: string,
  size?: Size
}

const Container : FC<ContainerProps> = (props) => {
  const size = props.size ? `container-${props.size}` : 'container';
  return <div className={`${size} ${props.className??''}`} style={props.style}>{props.children}</div>
}

export default Container;