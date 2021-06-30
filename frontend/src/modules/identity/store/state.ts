import { IUserModel, UserModel } from "../client/client";

export interface State extends IUserModel {
}

export const anonymous = "anonymous";

export const anonymousUser = new UserModel({
    id: anonymous,
    name: anonymous,
    token: undefined,
});

export const state: State = anonymousUser;
