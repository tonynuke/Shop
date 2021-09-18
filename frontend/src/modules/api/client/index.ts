import { apiGateway } from "@/modules/common/connection-strings";
import { Client } from "./client";
import instance from "@/modules/common/client"

const client = new Client(apiGateway, instance);
export default client;
