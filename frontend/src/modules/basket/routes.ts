import { RouteRecordRaw } from 'vue-router'
import Basket from '@/modules/basket/components/Basket.vue'

export const routes: Array<RouteRecordRaw> = [
    {
        path: '/basket',
        name: 'Basket',
        component: Basket
    }
]