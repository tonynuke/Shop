<template>
  <div class="card h-100">
    <img src="" class="card-img-top" alt="..." />
    <div class="card-body">
      <h5 class="card-title">
        {{ item.name }}
      </h5>
      <div class="row">
        <div class="col">
          {{ item.price }}
        </div>
        <div class="col">
          <button
            class="btn btn-primary"
            v-on:click="addOrUpdateBasketItem(item)"
          >
            Buy
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { UpdateBasketDto, BasketItemDto } from "@/modules/api/client/client";
import { ActionTypes } from "@/modules/basket/store/action-types";
import { defineComponent, PropType } from "vue";
import { ItemDto } from "../client/client";
import { useStore } from "@/store";

export default defineComponent({
  name: "CatalogItem",
  props: {
    item: {
      type: Object as PropType<ItemDto>,
      required: true,
    },
  },
  setup() {
    const store = useStore();

    const addOrUpdateBasketItem = (item: ItemDto) => {
      let dto = new UpdateBasketDto();
      const oldItems = store.state.basket.items!.map(
        (x) => new BasketItemDto({ id: x.id, quantity: x.quantity })
      );
      const itemInBasket = oldItems?.find((x) => x.id == item.id);
      console.log(itemInBasket);
      if (itemInBasket !== null) {
        const newItem = new BasketItemDto({ id: item.id, quantity: 1 });
        dto.items = [...oldItems, newItem];
        store.dispatch(ActionTypes.UPDATE_BASKET, dto);
      }
    };

    return {
      addOrUpdateBasketItem,
    };
  },
});
</script>