import { State } from "@/modules/basket/store/state";
import { Commit } from "vuex";
import { UpdateBasketDto } from "@/modules/api/client/client";
import { ActionTypes } from "./action-types";
import { MutationTypes } from "./mutation-types";
import client from "@/modules/api/client/index";

export default {
    async [ActionTypes.GET_BASKET](
        { commit }: { commit: Commit }) {
        const basket = await client.getOrCreateBasket();
        commit(MutationTypes.SET_BASKET, basket);
    },
    async [ActionTypes.UPDATE_BASKET](
        { state, commit }: { state: State, commit: Commit }, model: UpdateBasketDto) {
        const basket = await client.updateBasket(model);
        commit(MutationTypes.SET_BASKET, basket);
    },
}
