<template>
  <header class="p-3 bg-dark text-white">
    <div class="container">
      <div
        class="
          d-flex
          flex-wrap
          align-items-center
          justify-content-center justify-content-lg-start
        "
      >
        <!-- add icons support -->
        <!-- <a
          href="/"
          class="
            d-flex
            align-items-center
            mb-2 mb-lg-0
            text-white text-decoration-none
          "
        >
          <svg
            class="bi me-2"
            width="40"
            height="32"
            role="img"
            aria-label="Bootstrap"
          >
            <use xlink:href="#bootstrap"></use>
          </svg>
        </a> -->

        <ul
          class="
            nav
            col-12 col-lg-auto
            me-lg-auto
            mb-2
            justify-content-center
            mb-md-0
          "
        >
          <li>
            <router-link class="nav-link px-2 text-white" to="/catalog">
              Catalog
            </router-link>
          </li>
          <li>
            <router-link class="nav-link px-2 text-white" to="/basket">
              Basket
            </router-link>
          </li>
          <!-- <li>
            <router-link class="nav-link px-2 text-white" to="/chat">
              Chat
            </router-link>
          </li> -->
        </ul>

        <searcher class="col-12 col-lg-auto mb-3 mb-lg-0 me-lg-3" />

        <div class="text-end">
          <user />
        </div>
      </div>
    </div>
  </header>
  <body>
    <router-view />
  </body>
  <footer></footer>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import User from "@/modules/identity/components/User.vue";
import Searcher from "@/modules/catalog/components/Searcher.vue";

export default defineComponent({
  name: "App",
  components: { User, Searcher },
  setup() {},
  mounted() {
    this.$router.push({ path: "/catalog" });
  },
});
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}
</style>
