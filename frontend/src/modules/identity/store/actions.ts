import { Commit } from "vuex";
import { ActionTypes } from "./action-types";
import { MutationTypes } from "./mutation-types";
import client from "@/modules/identity/client/index";
import { SignInModel, SignUpModel } from "../client/client";

export default {
    async [ActionTypes.SIGN_UP](
        { commit }: { commit: Commit }, model: SignUpModel) {
        const result = await client.signUp(model);
        commit(MutationTypes.LOG_IN_USER, result);
    },
    async [ActionTypes.LOG_IN](
        { commit }: { commit: Commit }, model: SignInModel) {
        const result = await client.logIn(model);
        commit(MutationTypes.LOG_IN_USER, result);
    },
    async [ActionTypes.LOG_OUT](
        { commit }: { commit: Commit }) {
        await client.logOut();
        commit(MutationTypes.LOG_OUT_USER);
    },
}