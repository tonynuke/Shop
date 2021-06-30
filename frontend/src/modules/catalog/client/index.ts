import { catalog } from "@/modules/common/connection-strings";
import { Client } from "./client";

const client = new Client(catalog);
export default client;
