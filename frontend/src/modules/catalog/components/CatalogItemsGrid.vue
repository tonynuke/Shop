<template>
  <div v-if="!isLoaded" class="spinner-border m-5" role="status">
    <span class="visually-hidden">Loading...</span>
  </div>

  <div v-else class="container">
    <div
      class="row row-cols-1 row-cols-md-3"
      v-for="row in catalogTable"
      v-bind:key="row"
    >
      <div class="col" v-for="item of row" v-bind:key="item.id">
        <catalog-item :item="item" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { ItemsQueryDto, PageDto, ItemDto} from "../client/client";
import CatalogItem from "./CatalogItem.vue";
import client from "@/modules/catalog/client/index";
import _ from "lodash";

export default defineComponent({
  name: "CatalogItemsList",
  components: {
    CatalogItem,
  },
  data() {
    return {
      items: [] as Array<ItemDto>,
      isLoaded: false,
    };
  },
  methods: {
    async loadCatalogPage() {
      const pageModel = new PageDto({ skip: 0, limit: 10 });
      const searchModel = new ItemsQueryDto({ query: "", page: pageModel });
      client.searchItems(searchModel);
      try {
        const catlogItems = await client.searchItems(searchModel);
        this.items = catlogItems;
      } catch (exception) {
        console.log(exception);
      } finally {
        this.isLoaded = true;
      }
    },
  },
  computed: {
    catalogTable(): Array<Array<ItemDto>> {
      const chunkSize = 3;
      return _.chunk(this.items, chunkSize);
    }
  },
  async created() {
    await this.loadCatalogPage();
  },
});
</script>
