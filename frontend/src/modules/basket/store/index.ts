import { State, state } from './state'
import actions from './actions'
import mutations from './mutations';

export default {
  //namespaced: true,
  state,
  mutations: mutations,
  getters: {
    isBasketEmpty: (state: State) => state.items?.length == 0,
  },
  actions: actions
};
