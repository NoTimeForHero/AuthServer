import { FC } from 'preact/compat';

interface Props {
  titles: string[],
  active?: number,
  onChange?: (index: number) => void,
}

const Tabs : FC<Props> = (props) => {
  const { titles, active = 0 } = props;
  const onChange = (index: number) => (ev:  JSX.TargetedMouseEvent<HTMLAnchorElement>) => {
    props.onChange?.call(null, index);
    ev.preventDefault();
  }
  return <ul className="nav nav-tabs">
    {titles.map((title, index) => (
      <li className="nav-item" key={index}>
        <a className={`nav-link ${index===active?'active':''}`}
           onClick={onChange(index)}
           href="#">
          {titles[index]}
        </a>
      </li>
    ))}
  </ul>
}

export default Tabs;