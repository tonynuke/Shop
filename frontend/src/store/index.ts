import { createStore, createLogger, Store } from 'vuex'
import createPersistedState from "vuex-persistedstate";
import basket from '@/modules/basket/store'
import user from '@/modules/identity/store'
import { State as BasketState } from '@/modules/basket/store/state';
import { State as UserState } from '@/modules/identity/store/state';

const isDebug: boolean = process.env.NODE_ENV !== 'production';

export type RootState = {
  basket: BasketState;
  user: UserState,
};

const vuexPlugins: any = [
  createPersistedState()
]

export const store: Store<RootState> = createStore<RootState>({
  modules: {
    basket,
    user,
  },
  strict: isDebug,
  plugins: isDebug ? [createLogger(), ...vuexPlugins] : vuexPlugins
});

export function useStore(): Store<RootState> {
  return store;
}
