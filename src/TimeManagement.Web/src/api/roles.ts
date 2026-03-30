import { apiFetch } from './client'
import type { RoleDto, CreateRoleRequest, UpdateRoleRequest } from './types'

export function getRoles(): Promise<RoleDto[]> {
  return apiFetch<RoleDto[]>('/roles')
}

export function getRole(id: string): Promise<RoleDto> {
  return apiFetch<RoleDto>(`/roles/${id}`)
}

export function createRole(request: CreateRoleRequest): Promise<RoleDto> {
  return apiFetch<RoleDto>('/roles', {
    method: 'POST',
    body: JSON.stringify(request),
  })
}

export function updateRole(id: string, request: UpdateRoleRequest): Promise<RoleDto> {
  return apiFetch<RoleDto>(`/roles/${id}`, {
    method: 'PUT',
    body: JSON.stringify(request),
  })
}

export function deleteRole(id: string): Promise<void> {
  return apiFetch<void>(`/roles/${id}`, {
    method: 'DELETE',
  })
}
