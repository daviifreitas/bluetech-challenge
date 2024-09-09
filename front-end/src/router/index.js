import AppLayout from '@/layout/AppLayout.vue';
import { createRouter, createWebHistory } from 'vue-router';

const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: '/',
            component: AppLayout,
            children: [
                {
                    path: '/agendamento',
                    name: 'crud',
                    component: () => import('@/views/pages/Schedule.vue')
                },
            ]
        },
    ]
});

export default router;
