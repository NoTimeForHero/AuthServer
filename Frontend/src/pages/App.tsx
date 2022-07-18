import LoginButtons from './LoginButtons';
import Container from '../bootstrap/components/Container';
import { LoadWrapper } from '../context/GlobalData';
import {Router, Route} from 'preact-router';
import Information from './Information';
import Card from '../bootstrap/components/Card';

const App = () => {
  return <Container size={'xl'} className={"flex-center"}>
    <LoadWrapper>
      <Router>
        <LoginButtons path="/authorize" />
        <Information path="/info" />
        {/* TODO: Нормальная страница по умолчанию */}
        <Route path="/" component={() => <Card>
          <h1 className="p-4">Under Construction!</h1>
        </Card>} />
      </Router>
    </LoadWrapper>
  </Container>
}

export default App;