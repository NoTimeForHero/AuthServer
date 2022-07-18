import LoginButtons from './LoginButtons';
import Container from '../bootstrap/components/Container';
import { Route, Router } from 'preact-router';
import Information from './Information';
import Card from '../bootstrap/components/Card';
import { useAtom } from 'jotai';
import { settingsAtom, useStatusBar } from '../api/store';
import { useEffect } from 'preact/compat';
import { fetchSettings } from '../api';
import StatusBar from './StatusBar';


const App = () => {
  const [settings,setSettings] = useAtom(settingsAtom);
  const { call } = useStatusBar();
  useEffect(() => { call(fetchSettings).then(setSettings) }, []);

  if (!settings) return (<Container size={'xl'} className={"flex-center"}>
    <StatusBar />
  </Container>);

  return <Container size={'xl'} className={"flex-center"}>
    <Router>
        <LoginButtons path="/authorize" />
        <Information path="/info" />
      {/* TODO: Нормальная страница по умолчанию */}
        <Route path="/" component={() => <Card>
          <StatusBar />
          <h1 className="p-4">Under Construction!</h1>
        </Card>} />
    </Router>
  </Container>
}

export default App;