import { apiFetch } from './client'
import type { DepartmentDto, CreateDepartmentRequest, UpdateDepartmentRequest } from './types'

export function getDepartments(): Promise<DepartmentDto[]> {
  return apiFetch<DepartmentDto[]>('/departments')
}

export function getDepartment(id: string): Promise<DepartmentDto> {
  return apiFetch<DepartmentDto>(`/departments/${id}`)
}

export function createDepartment(request: CreateDepartmentRequest): Promise<DepartmentDto> {
  return apiFetch<DepartmentDto>('/departments', {
    method: 'POST',
    body: JSON.stringify(request),
  })
}

export function updateDepartment(id: string, request: UpdateDepartmentRequest): Promise<DepartmentDto> {
  return apiFetch<DepartmentDto>(`/departments/${id}`, {
    method: 'PUT',
    body: JSON.stringify(request),
  })
}

export function deleteDepartment(id: string): Promise<void> {
  return apiFetch<void>(`/departments/${id}`, {
    method: 'DELETE',
  })
}
