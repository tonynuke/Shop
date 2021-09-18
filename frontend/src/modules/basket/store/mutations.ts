import { IUserBasketDto } from "@/modules/api/client/client";
import { MutationTypes } from "./mutation-types";
import { State } from "./state";

export default {
  [MutationTypes.SET_BASKET](state: State, basket: IUserBasketDto) {
    state.id = basket.id;
    state.items = basket.items;
    state.price = basket.price;
  },
}