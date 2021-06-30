import { IBasketDto } from "../client/client";
import { MutationTypes } from "./mutation-types";
import { State } from "./state";

export default {
  [MutationTypes.SET_BASKET](state: State, basket: IBasketDto) {
    state.id = basket.id;
    state.items = basket.items;
    state.price = basket.price;
  },
}