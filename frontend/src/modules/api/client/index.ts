import { identity } from "@/modules/common/connection-strings";
import { Client } from "./client";

const client = new Client(identity);
export default client;
