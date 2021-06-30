import { State } from "@/modules/basket/store/state";
import { Commit } from "vuex";
import { AddOrUpdateBasketItemDto } from "../client/client";
import { ActionTypes } from "./action-types";
import { MutationTypes } from "./mutation-types";
import client from "./../client/index";

export default {
    async [ActionTypes.GET_BASKET](
        { commit }: { commit: Commit }) {
        const basket = await client.getOrCreateBasket();
        commit(MutationTypes.SET_BASKET, basket);
    },
    async [ActionTypes.UPDATE_BASKET_ITEM](
        { state, commit }: { state: State, commit: Commit }, model: AddOrUpdateBasketItemDto) {
        const basket = await client.addOrUpdateBasketItem(model);
        commit(MutationTypes.SET_BASKET, basket);
    },
    async [ActionTypes.REMOVE_BASKET_ITEM](
        { state, commit }: { state: State, commit: Commit }, itemId: string) {
        const basket = await client.removeItemFromBasket(itemId);
        commit(MutationTypes.SET_BASKET, basket);
    }
}
