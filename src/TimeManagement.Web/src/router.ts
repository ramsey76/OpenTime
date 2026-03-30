import { createRouter, createWebHistory } from 'vue-router'
import RolesPage from './pages/RolesPage.vue'
import DepartmentsPage from './pages/DepartmentsPage.vue'
import DashboardPage from './pages/DashboardPage.vue'
import TimeEntriesPage from './pages/TimeEntriesPage.vue'
import ProjectsPage from './pages/ProjectsPage.vue'
import EmployeesPage from './pages/EmployeesPage.vue'

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/dashboard' },
    { path: '/dashboard', component: DashboardPage },
    { path: '/time-entries', component: TimeEntriesPage },
    { path: '/projects', component: ProjectsPage },
    { path: '/employees', component: EmployeesPage },
    { path: '/roles', component: RolesPage },
    { path: '/departments', component: DepartmentsPage },
  ],
})
