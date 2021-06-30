<template>
  <div>
    <span>{{ text }}</span>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "TextCell",
  props: {
    text: String,
  },
  setup() {},
});
</script>
