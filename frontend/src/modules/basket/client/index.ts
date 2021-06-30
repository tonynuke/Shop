import { basket } from "@/modules/common/connection-strings";
import { Client } from "@/modules/basket/client/client";
import instance from "@/modules/common/client"

const client = new Client(basket, instance);
export default client;
