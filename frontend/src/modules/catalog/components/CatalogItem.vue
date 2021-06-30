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
import { AddOrUpdateBasketItemDto } from "@/modules/basket/client/client";
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
      const model = new AddOrUpdateBasketItemDto({
        catalogItemId: item.id,
        quantity: 1,
      });
      store.dispatch(ActionTypes.UPDATE_BASKET_ITEM, model);
    };

    return {
      addOrUpdateBasketItem,
    };
  },
});
</script>