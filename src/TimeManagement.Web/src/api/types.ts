export interface RoleDto {
  id: string
  name: string
  description: string | null
}

export interface CreateRoleRequest {
  name: string
  description?: string
}

export interface UpdateRoleRequest {
  name: string
  description?: string
}

export interface DepartmentDto {
  id: string
  name: string
  code: string
  parentDepartmentId: string | null
}

export interface CreateDepartmentRequest {
  name: string
  code: string
  parentDepartmentId?: string | null
}

export interface UpdateDepartmentRequest {
  name: string
  code: string
  parentDepartmentId?: string | null
}
