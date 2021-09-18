import { IUserBasketDto } from '@/modules/api/client/client'

export interface State extends IUserBasketDto {
}

export const state: State = {
    id: undefined,
    items: [],
    price: undefined,
};
