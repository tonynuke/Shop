import { UserModel } from "../client/client";
import { MutationTypes } from "./mutation-types";
import { anonymousUser, State } from "./state";

export default {
    [MutationTypes.LOG_IN_USER](state: State, model: UserModel) {
        state.id = model.id;
        state.name = model.name;
        state.token = model.token;
    },
    [MutationTypes.LOG_OUT_USER](state: State) {
        state.id = anonymousUser.id;
        state.name = anonymousUser.name;
        state.token = anonymousUser.token;
    },
}