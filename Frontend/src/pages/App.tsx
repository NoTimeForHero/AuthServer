import LoginButtons from './LoginButtons';
import Container from '../components/bootstrap/Container';
import { Route, Router } from 'preact-router';
import Information from './Information';
import Card from '../components/bootstrap/Card';
import { useAtom } from 'jotai';
import { settingsAtom, useStatusBar } from '../api/store';
import { fetchSettings } from '../api';
import StatusBar from './StatusBar';
import { useMount } from '../utils';
import { usePreloader } from '../api/preloader';


const App = () => {
  const {hasSettings} = usePreloader();
  const [settings,setSettings] = useAtom(settingsAtom);
  const { call } = useStatusBar();
  useMount(async () => {
    if (hasSettings || settings) return;
    setSettings(await call(fetchSettings));
  });

  if (!settings) return (<Container size={'xl'} className={"flex-center"}>
    <StatusBar />
  </Container>);

  return <Container size={'xl'} className={"flex-center"}>
    <Router>
        <LoginButtons path="/authorize" />
        <LoginButtons path="/complete" />
        <Information path="/info" />
        <Route default component={() => <Card>
          <h1 className="p-4">Ошибка 404:</h1>
          <h2>Страница не найдена</h2>
        </Card>} />
    </Router>
  </Container>
}

export default App;