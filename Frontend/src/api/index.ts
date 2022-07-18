import { Claim, Settings } from '../types';


const get = <T,>(url: string) : Promise<T> => {
  return fetch(url).then(x => x.json());
}

export const fetchSettings = () => get<Settings>('/api/settings');
export const fetchClaims = () : Promise<Record<string,Claim[]>> => get<any>('/api/info').then(x => x?.claims);
