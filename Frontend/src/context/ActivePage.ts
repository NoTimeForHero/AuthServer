import { createContext, StateUpdater } from 'preact/compat';

type ContextType = StateUpdater<any>;
const defaultValue = () => {};
export const activePageCtx = createContext<ContextType>(defaultValue);