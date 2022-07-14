import LoginButtons from './LoginButtons';
import Container from '../bootstrap/components/Container';
import { LoadWrapper } from '../store/GlobalData';

const App = () => {

  return <Container size={'xl'} className={"flex-center"}>
    <LoadWrapper>
      <LoginButtons />
    </LoadWrapper>
  </Container>
}

export default App;