<template>
  <modal ref="modal">
    <template v-slot:header>Create catalog item</template>
    <template v-slot:body></template>
    <template v-slot:footer></template>
  </modal>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import Modal from "@/modules/common/components/Modal.vue";
export default defineComponent({
  name: "CreateCatalogItemModal",
  components: { Modal },
});
</script>