import { shallowMount, mount } from '@vue/test-utils'
import Basket from '@/modules/basket/components/Basket.vue'
import { RootState, useStore } from "@/store";
import { Store } from 'vuex';
import { BasketDto, BasketItemDto } from '@/modules/basket/client/client';
import { ActionTypes } from '@/modules/basket/store/action-types';
import client from "@/modules/basket/client/index";

jest.mock('@/modules/basket/client/index');
const mockedClient = client as jest.Mocked<typeof client>;

describe('Basket', () => {
  let store: Store<RootState> = useStore();

  it('empty basket', async () => {
    // await store.dispatch(ActionTypes.GET_BASKET);
    const wrapper = mount(Basket);
    expect(wrapper.html()).toContain('Basket is empty!');
  });

  it('basket with item', async () => {
    const backetItemDto = new BasketItemDto({ id: "id", price: 300, quantity: 1 });
    const basketDto = new BasketDto({ id: "id", price: 300, items: [backetItemDto] });
    mockedClient.getOrCreateBasket.mockResolvedValue(basketDto);
    await store.dispatch(ActionTypes.GET_BASKET);

    const wrapper = mount(Basket, {
      global: { plugins: [store] }
    });
    expect(wrapper.html()).toContain('Total price 300');
  });
});
