<template>
  <empty-basket v-if="isBasketEmpty" />
  <div v-else>
    <div>Total price {{ basketPrice }}</div>
    <basket-item v-for="item in basketItems" v-bind:key="item.id" :item="item" />
  </div>
</template>

<script lang="ts">
import { defineComponent, computed } from "vue";
import { useStore } from "@/store";
import BasketItem from "@/modules/basket/components/BasketItem.vue";
import EmptyBasket from "@/modules/basket/components/EmptyBasket.vue";
import {
  UpdateBasketDto,
} from "@/modules/api/client/client";
import { ActionTypes } from "@/modules/basket/store/action-types";

export default defineComponent({
  name: "Basket",
  components: {
    BasketItem,
    EmptyBasket,
  },
  setup() {
    const store = useStore();

    const basketItems = computed(() => store.state.basket.items);
    const basketPrice = computed(() => store.state.basket.price);
    const isBasketEmpty = computed(() => store.getters.isBasketEmpty);

    const updateBasket = async () => {
      const model = new UpdateBasketDto();
      await store.dispatch(ActionTypes.UPDATE_BASKET, model);
    };

    return {
      basketItems,
      basketPrice,
      isBasketEmpty,
      updateBasket,
    };
  },
});
</script>