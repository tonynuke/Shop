import { IBasketDto } from '@/modules/basket/client/client'

export interface State extends IBasketDto {
}

export const state: State = {
    id: undefined,
    items: [],
    price: undefined,
};
