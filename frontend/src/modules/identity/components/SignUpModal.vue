<template>
  <modal ref="modal">
    <template v-slot:header>Sign-up</template>
    <template v-slot:body>
      <div class="input-group mb-3">
        <span class="input-group-text" id="basic-addon1">Email</span>
        <input
          type="text"
          class="form-control"
          placeholder="Username"
          aria-label="Username"
          v-model="signUpModel.email"
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
          v-model="signUpModel.password"
          aria-describedby="basic-addon1"
        />
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn-warning" @click="signUp">Sign-up</button>
    </template>
  </modal>
</template>

<script lang="ts">
import { defineComponent, ref, Ref } from "vue";
import { ISignUpModel } from "../client/client";
import Modal from "@/modules/common/components/Modal.vue";
import { ActionTypes } from "../store/action-types";
import { useStore } from "@/store";

export default defineComponent({
  name: "SignUpModal",
  components: { Modal },
  setup() {
    const signUpModel: Ref<ISignUpModel> = ref({
      email: "",
      password: "",
    });
    const modal = ref<InstanceType<typeof Modal>>();
    const store = useStore();
    const signUp = async () => {
      try {
        await store.dispatch(ActionTypes.SIGN_UP, signUpModel.value);
        modal.value?.close();
      } catch (exception) {
        console.log(exception);
      }
    };

    return {
      modal,
      signUpModel,
      signUp,
    };
  },
});
</script>
