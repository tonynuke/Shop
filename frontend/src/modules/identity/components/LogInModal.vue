<template>
  <modal ref="modal">
    <template v-slot:header>Login</template>
    <template v-slot:body>
      <div class="input-group mb-3">
        <span class="input-group-text" id="basic-addon1">Email</span>
        <input
          type="text"
          class="form-control"
          placeholder="Username"
          aria-label="Username"
          v-model="loginModel.email"
          aria-describedby="basic-addon1"
        />
      </div>
      <div class="input-group mb-3">
        <span class="input-group-text" id="basic-addon1">Password</span>
        <input
          type="password"
          class="form-control"
          placeholder="Password"
          aria-label="Password"
          v-model="loginModel.password"
          aria-describedby="basic-addon1"
        />
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn-warning" @click="login">Login</button>
    </template>
  </modal>
</template>

<script lang="ts">
import { defineComponent, Ref, ref } from "vue";
import Modal from "@/modules/common/components/Modal.vue";
import { useStore } from "@/store";
import { ActionTypes } from "../store/action-types";
import { ISignInModel } from "../client/client";

export default defineComponent({
  name: "LogInModal",
  components: { Modal },
  setup() {
    const loginModel: Ref<ISignInModel> = ref({
      email: "",
      password: "",
    });

    const modal = ref<InstanceType<typeof Modal>>();
    const store = useStore();
    const login = async () => {
      try {
        await store.dispatch(ActionTypes.LOG_IN, loginModel.value);
        modal.value?.close();
      } catch (exception) {
        console.log(exception);
      }
    };

    return {
      modal,
      loginModel,
      login,
    };
  },
});
</script>
