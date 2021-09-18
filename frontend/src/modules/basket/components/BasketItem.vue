<template>
  <div class="card">
    <div class="card-body">
      <div class="row">
        <div class="col-sm-6">
          <h5 class="card-title">{{ item.name }}</h5>
          <p class="card-text">{{ item.price }}</p>
        </div>
        <div class="col-sm-6 text-right">
          <p class="card-text">{{ item.quantity }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import { IUserBasketItemDto } from "@/modules/api/client/client";

export default defineComponent({
  name: "BasketItem",
  props: {
    item: {
      type: Object as PropType<IUserBasketItemDto>,
      required: true,
    },
  },
});
</script>