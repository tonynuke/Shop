<template>
  <div>
    <Table
      :rows="rows"
      :columns="columns"
      @editItem="handler"
      @deleteItem="handler"
      @addItem="addItemHandler"
    >
      <template v-slot:cell="{ type, value }">
        <component :is="type" v-bind="value" />
      </template>
    </Table>
    <create-catalog-item-modal v-if="isCreation" />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import Table from "@/modules/common/components/table/Table.vue";
import TextCell from "@/modules/common/components/table/TextCell.vue";
import CreateCatalogItemModal from "@/modules/catalog/components/CreateCatalogItemModal.vue";

export default defineComponent({
  name: "CatalogItemsTable",
  components: {
    Table,
    TextCell,
    CreateCatalogItemModal,
  },
  setup() {
    const isCreation = ref(false);

    return {
      isCreation,
    };
  },
  methods: {
    handler: function (row: any) {
      console.log(row.name);
    },
    addItemHandler: function () {
      console.log("hello");
    },
  },
  data() {
    return {
      columns: [
        { name: "brand", title: "Brand", type: "TextCell" },
        { name: "name", title: "Name", type: "TextCell" },
        { name: "price", title: "Price", type: "TextCell" },
      ],
      rows: [
        {
          brand: { text: "Adidas" },
          name: { text: "TShirt" },
          price: { text: "100" },
        },
      ],
    };
  },
});
</script>
