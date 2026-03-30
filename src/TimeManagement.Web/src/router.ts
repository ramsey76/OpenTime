import { createRouter, createWebHistory } from 'vue-router'
import RolesPage from './pages/RolesPage.vue'
import DepartmentsPage from './pages/DepartmentsPage.vue'

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/roles' },
    { path: '/roles', component: RolesPage },
    { path: '/departments', component: DepartmentsPage },
  ],
})
