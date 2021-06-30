import { identity } from "@/modules/common/connection-strings";
import { IdentityClient } from "./client";

const client = new IdentityClient(identity);
export default client;
