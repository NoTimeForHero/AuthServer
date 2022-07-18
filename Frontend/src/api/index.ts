import { Claim, Settings } from '../types';
import { wait } from '../utils';


const get = async <T,>(url: string) : Promise<T> => {
  const data = await fetch(url);
  // await wait(2400);
  if (!data.ok) throw await data.text();
  return data.json();
}

export const fetchSettings = () => get<Settings>('/api/settings');
export const fetchClaims = () : Promise<Record<string,Claim[]>> => get<any>('/api/info').then(x => x?.claims);

export interface Authorize {
  application: {
    id: string,
    title: string
    baseURL: string
  },
  redirect: string
}

export const fetchAuthorize = (app: string, redirect?: string) => {
  const params = new URLSearchParams();
  params.set('app', app);
  if (redirect) params.set('redirect', redirect);
  return get<Authorize>(`/api/authorize?${params}`);
}

export const doLogin = (provider: string) => {
  const redirect = encodeURIComponent(document.location.toString());
  document.location = `/api/login?provider=${provider}&redirect=${redirect}`;
}