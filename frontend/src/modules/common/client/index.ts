// https://medium.com/swlh/handling-access-and-refresh-tokens-using-axios-interceptors-3970b601a5da
import axios from "axios";
import { useStore } from "@/store/index";

const instance = axios.create();
instance.interceptors.request.use(
    config => {
        const store = useStore();
        const token = store.state.user.token;
        console.log(token);
        if (token) {
            config.headers['Authorization'] = 'Bearer ' + token;
        }
        return config;
    },
    error => {
        Promise.reject(error)
    });

export default instance;