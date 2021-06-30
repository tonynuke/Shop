import { RouteRecordRaw } from 'vue-router'
import TalkJs from '@/modules/chat/components/TalkJs.vue'

export const routes: Array<RouteRecordRaw> = [
    {
        path: '/chat',
        name: 'Chat',
        component: TalkJs
    }
]
