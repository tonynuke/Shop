<template>
  <div v-if="isAnonymous">
    <button type="button" class="btn btn-outline-light me-2" @click="showLogIn">
      Login
    </button>
    <button type="button" class="btn btn-warning" @click="showSignUp">
      Sign-up
    </button>

    <transition name="slide-fade">
      <log-in-modal v-if="isLogIn" @close="hideLogIn" />
    </transition>
    <transition name="slide-fade">
      <sign-up-modal v-if="isSignUp" @close="hideSignUp" />
    </transition>
  </div>
  <div v-else class="dropdown text-end">
    <a
      class="nav-link dropdown-toggle"
      href="#"
      id="navbarDropdown"
      role="button"
      data-bs-toggle="dropdown"
      aria-expanded="false"
    >
      User
    </a>
    <ul class="dropdown-menu" aria-labelledby="navbarDropdown">
      <li><a class="dropdown-item" href="#">Action</a></li>
      <li><a class="dropdown-item" href="#">Another action</a></li>
      <li><hr class="dropdown-divider" /></li>
      <li>
        <a class="dropdown-item" href="#" @click="logout">Logout</a>
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { useStore } from "@/store";
import { defineComponent, computed, ref } from "vue";
import LogInModal from "./LogInModal.vue";
import SignUpModal from "./SignUpModal.vue";
import { ActionTypes } from "../store/action-types";

export default defineComponent({
  name: "User",
  components: {
    LogInModal,
    SignUpModal,
  },
  setup() {
    const isLogIn = ref(false);
    const isSignUp = ref(false);

    const store = useStore();
    const user = computed(() => store.state.user.name);
    const isAnonymous = computed(() => store.getters.isAnonymous);
    const logout = async () => {
      await store.dispatch(ActionTypes.LOG_OUT);
    };
    return {
      isLogIn,
      isSignUp,
      user,
      isAnonymous,
      logout,
    };
  },
  methods: {
    showLogIn() {
      this.isLogIn = true;
    },
    hideLogIn() {
      this.isLogIn = false;
    },
    showSignUp() {
      this.isSignUp = true;
    },
    hideSignUp() {
      this.isSignUp = false;
    },
  },
});
</script>

<style scoped>
/* Enter and leave animations can use different */
/* durations and timing functions.              */
.slide-fade-enter-active {
  transition: all 0.2s ease-out;
}

.slide-fade-leave-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}

.slide-fade-enter-from,
.slide-fade-leave-to {
  transform: translateX(20px);
  opacity: 0;
}
</style>