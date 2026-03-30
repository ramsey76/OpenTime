import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { DepartmentDto, CreateDepartmentRequest, UpdateDepartmentRequest } from '../api/types'
import * as departmentsApi from '../api/departments'

export interface DepartmentTreeNode extends DepartmentDto {
  children: DepartmentTreeNode[]
  depth: number
}

export const useDepartmentsStore = defineStore('departments', () => {
  const departments = ref<DepartmentDto[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  const hierarchicalDepartments = computed<DepartmentTreeNode[]>(() => {
    const map = new Map<string, DepartmentTreeNode>()
    const roots: DepartmentTreeNode[] = []

    for (const dept of departments.value) {
      map.set(dept.id, { ...dept, children: [], depth: 0 })
    }

    for (const node of map.values()) {
      if (node.parentDepartmentId && map.has(node.parentDepartmentId)) {
        const parent = map.get(node.parentDepartmentId)!
        node.depth = parent.depth + 1
        parent.children.push(node)
      } else {
        roots.push(node)
      }
    }

    function flatten(nodes: DepartmentTreeNode[]): DepartmentTreeNode[] {
      const result: DepartmentTreeNode[] = []
      for (const node of nodes) {
        result.push(node)
        result.push(...flatten(node.children))
      }
      return result
    }

    return flatten(roots)
  })

  async function fetchDepartments() {
    loading.value = true
    error.value = null
    try {
      departments.value = await departmentsApi.getDepartments()
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to load departments'
    } finally {
      loading.value = false
    }
  }

  async function createDepartment(request: CreateDepartmentRequest) {
    await departmentsApi.createDepartment(request)
    await fetchDepartments()
  }

  async function updateDepartment(id: string, request: UpdateDepartmentRequest) {
    await departmentsApi.updateDepartment(id, request)
    await fetchDepartments()
  }

  async function deleteDepartment(id: string) {
    await departmentsApi.deleteDepartment(id)
    await fetchDepartments()
  }

  return {
    departments,
    loading,
    error,
    hierarchicalDepartments,
    fetchDepartments,
    createDepartment,
    updateDepartment,
    deleteDepartment,
  }
})
