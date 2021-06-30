import { RouteRecordRaw } from 'vue-router'
import CatalogItemsGrid from './components/CatalogItemsGrid.vue'
import CatalogItem from './components/CatalogItemsGrid.vue'
import CatalogItemsTable from './components/CatalogItemsTable.vue'

export const routes: Array<RouteRecordRaw> = [
    {
        path: '/catalog',
        name: 'CatalogItemsGrid',
        component: CatalogItemsGrid
    },
    {
        path: '/catalog:id',
        name: 'CatalogItem',
        component: CatalogItem
    },
    {
        path: '/admin/catalog',
        name: 'CatalogItemsTable',
        component: CatalogItemsTable
    }
]