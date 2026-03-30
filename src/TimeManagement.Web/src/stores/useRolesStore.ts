import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { RoleDto, CreateRoleRequest, UpdateRoleRequest } from '../api/types'
import * as rolesApi from '../api/roles'

export const useRolesStore = defineStore('roles', () => {
  const roles = ref<RoleDto[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchRoles() {
    loading.value = true
    error.value = null
    try {
      roles.value = await rolesApi.getRoles()
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to load roles'
    } finally {
      loading.value = false
    }
  }

  async function createRole(request: CreateRoleRequest) {
    await rolesApi.createRole(request)
    await fetchRoles()
  }

  async function updateRole(id: string, request: UpdateRoleRequest) {
    await rolesApi.updateRole(id, request)
    await fetchRoles()
  }

  async function deleteRole(id: string) {
    await rolesApi.deleteRole(id)
    await fetchRoles()
  }

  return { roles, loading, error, fetchRoles, createRole, updateRole, deleteRole }
})
