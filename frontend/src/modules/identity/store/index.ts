import actions from "./actions";
import mutations from "./mutations";
import { anonymous, State, state } from "./state";

export default {
    //namespaced: true,
    state : state,
    getters: {
        isAnonymous: (state: State) => state?.id == anonymous,
    },
    actions: actions,
    mutations: mutations,
}