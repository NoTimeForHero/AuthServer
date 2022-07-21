import { Claim, Settings } from '../types';
import { buildURL, wait } from '../utils';


const get = async <T,>(url: string, params?: Record<string,string|undefined>) : Promise<T> => {
  const finalUrl = buildURL(url, params);
  const data = await fetch(finalUrl);
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

export const fetchAuthorize = (app: string, redirect?: string) =>
  get<Authorize>(`/api/authorize`, {app, redirect});

export const fetchAccess = (app: string, redirect?: string) =>
  get<{redirect: string, token: string}>(`/api/access`, {app, redirect});

export const doLogin = (provider: string) => {
  const redirect = encodeURIComponent(document.location.toString());
  document.location = `/api/login?provider=${provider}&redirect=${redirect}`;
}