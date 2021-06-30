<template>
  <div class="modal" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <slot name="header">This is the default title!</slot>
          <button
            type="button"
            class="btn-close"
            aria-label="Close"
            data-dismiss="modal"
            @click="close"
          ></button>
        </div>
        <div class="modal-body">
          <slot name="body">This is the default body!</slot>
        </div>
        <div class="modal-footer">
          <slot name="footer">This is the default footer!</slot>
          <button class="btn btn-danger" @click="close">Cancel</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "Modal",
  methods: {
    close() {
      this.$emit("close");
    },
  },
});
</script>

<style scoped>
/* Override default value of 'none' */
.modal {
  display: block;
  color: black;
}
</style>