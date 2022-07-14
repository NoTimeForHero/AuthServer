import LoginButtons from './LoginButtons';
import Container from '../bootstrap/components/Container';
import { LoadWrapper } from '../context/GlobalData';
import { useState } from 'preact/compat';
import { activePageCtx } from '../context/ActivePage';

const App = () => {

  const [page, setActivePage] = useState<JSX.Element>(<LoginButtons />);
  return <Container size={'xl'} className={"flex-center"}>
    <LoadWrapper>
      <activePageCtx.Provider value={setActivePage}>
        {page}
      </activePageCtx.Provider>
    </LoadWrapper>
  </Container>
}

export default App;