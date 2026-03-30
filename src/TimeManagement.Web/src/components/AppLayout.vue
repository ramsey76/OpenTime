<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div class="flex h-screen">
    <nav class="w-56 bg-base-300 flex flex-col">
      <div class="p-4 text-lg font-semibold border-b border-base-content/10">
        TimeManagement
      </div>
      <ul class="menu flex-1 p-2">
        <li>
          <RouterLink to="/dashboard" active-class="active">Dashboard</RouterLink>
        </li>
        <li>
          <RouterLink to="/time-entries" active-class="active">Time Entries</RouterLink>
        </li>
        <li>
          <details open>
            <summary>Admin</summary>
            <ul>
              <li>
                <RouterLink to="/roles" active-class="active">Roles</RouterLink>
              </li>
              <li>
                <RouterLink to="/departments" active-class="active">Departments</RouterLink>
              </li>
              <li>
                <RouterLink to="/projects" active-class="active">Projects</RouterLink>
              </li>
              <li>
                <RouterLink to="/employees" active-class="active">Employees</RouterLink>
              </li>
            </ul>
          </details>
        </li>
      </ul>
    </nav>
    <main class="flex-1 overflow-auto bg-base-200 p-6">
      <RouterView />
    </main>
  </div>
</template>
