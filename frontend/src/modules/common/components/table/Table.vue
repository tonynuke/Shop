<template>
  <table class="table table-hover table-striped">
    <thead>
      <th v-for="col in columns" :key="col.name">{{ col.title }}</th>
      <th>Operations</th>
    </thead>
    <tbody>
      <tr v-for="(row, index) in rows" :key="index">
        <td v-for="col in columns" :key="col.name">
          <slot name="cell" :type="col.type" :value="row[col.name]" />
        </td>
        <td>
          <div class="btn-group">
            <button
              type="button"
              class="btn btn-outline-primary"
              @click="editItem(row)"
            >
              Edit
            </button>
            <button
              type="button"
              class="btn btn-outline-danger"
              @click="deleteItem(row)"
            >
              Delete
            </button>
          </div>
        </td>
      </tr>
      <tr>
        <button type="button" class="btn btn-outline-success" @click="addItem">
          Add
        </button>
      </tr>
    </tbody>
  </table>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import IColumn from "@/modules/common/components/table/column";

export default defineComponent({
  name: "Table",
  props: {
    columns: Array as PropType<Array<IColumn>>,
    rows: Array,
    isEditable: { type: Boolean, default: true },
  },
  methods: {
    editItem(row: any) {
      this.$emit("editItem", row);
    },
    deleteItem(row: any) {
      this.$emit("deleteItem", row);
    },
    addItem() {
      this.$emit("addItem");
    },
  },
});
</script>
