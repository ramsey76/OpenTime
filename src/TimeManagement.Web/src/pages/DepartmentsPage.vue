<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useDepartmentsStore } from '../stores/useDepartmentsStore'
import { ApiError } from '../api/client'
import type { DepartmentDto } from '../api/types'
import DepartmentFormModal from '../components/DepartmentFormModal.vue'
import ConfirmDialog from '../components/ConfirmDialog.vue'
import ToastNotification from '../components/ToastNotification.vue'

const store = useDepartmentsStore()

const showFormModal = ref(false)
const editingDepartment = ref<DepartmentDto | null>(null)
const showDeleteConfirm = ref(false)
const deletingDepartment = ref<DepartmentDto | null>(null)
const toastMessage = ref<string | null>(null)
const toastType = ref<'error' | 'success'>('error')

onMounted(() => {
  store.fetchDepartments()
})

function openCreateModal() {
  editingDepartment.value = null
  showFormModal.value = true
}

function openEditModal(department: DepartmentDto) {
  editingDepartment.value = department
  showFormModal.value = true
}

function openDeleteConfirm(department: DepartmentDto) {
  deletingDepartment.value = department
  showDeleteConfirm.value = true
}

function getParentName(parentId: string | null): string {
  if (!parentId) return '—'
  const parent = store.departments.find(d => d.id === parentId)
  return parent?.name ?? '—'
}

async function handleFormSubmit(data: { name: string; code: string; parentDepartmentId: string | null }) {
  try {
    if (editingDepartment.value) {
      await store.updateDepartment(editingDepartment.value.id, data)
    } else {
      await store.createDepartment(data)
    }
    showFormModal.value = false
  } catch (e) {
    if (e instanceof ApiError && e.status === 409) {
      toastType.value = 'error'
      toastMessage.value = 'A department with this code already exists.'
    } else if (e instanceof ApiError && e.status === 422) {
      toastType.value = 'error'
      toastMessage.value = 'Invalid parent department selection. This would create a circular reference.'
    } else {
      toastType.value = 'error'
      toastMessage.value = e instanceof Error ? e.message : 'An error occurred.'
    }
  }
}

async function handleDeleteConfirm() {
  if (!deletingDepartment.value) return
  try {
    await store.deleteDepartment(deletingDepartment.value.id)
    showDeleteConfirm.value = false
  } catch (e) {
    showDeleteConfirm.value = false
    if (e instanceof ApiError && e.status === 409) {
      toastType.value = 'error'
      toastMessage.value = 'This department cannot be deleted because it has assigned employees or subdepartments.'
    } else {
      toastType.value = 'error'
      toastMessage.value = e instanceof Error ? e.message : 'An error occurred.'
    }
  }
}
</script>

<template>
  <div>
    <div class="flex items-center justify-between mb-6">
      <h1 class="text-2xl font-bold">Departments</h1>
      <button class="btn btn-primary btn-sm" @click="openCreateModal">
        + Add Department
      </button>
    </div>

    <div v-if="store.loading" class="flex justify-center p-8">
      <span class="loading loading-spinner loading-md"></span>
    </div>
    <div v-else-if="store.error" class="alert alert-error">{{ store.error }}</div>
    <div v-else-if="store.departments.length === 0" class="text-base-content/50">
      No departments have been created yet.
    </div>
    <div v-else class="overflow-x-auto">
      <table class="table table-zebra">
        <thead>
          <tr>
            <th>Name</th>
            <th>Code</th>
            <th>Parent</th>
            <th class="text-right">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="dept in store.hierarchicalDepartments" :key="dept.id">
            <td class="font-medium">
              <span :style="{ paddingLeft: `${dept.depth * 1.5}rem` }">
                <span v-if="dept.depth > 0" class="opacity-40 mr-1">&boxur;</span>
                {{ dept.name }}
              </span>
            </td>
            <td>{{ dept.code }}</td>
            <td>{{ getParentName(dept.parentDepartmentId) }}</td>
            <td class="text-right space-x-2">
              <button class="btn btn-info btn-link btn-sm" @click="openEditModal(dept)">Edit</button>
              <button class="btn btn-error btn-link btn-sm" @click="openDeleteConfirm(dept)">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <DepartmentFormModal
      :open="showFormModal"
      :department="editingDepartment"
      :all-departments="store.departments"
      @submit="handleFormSubmit"
      @cancel="showFormModal = false"
    />

    <ConfirmDialog
      :open="showDeleteConfirm"
      title="Delete Department"
      :message="`Are you sure you want to delete the department '${deletingDepartment?.name}'?`"
      @confirm="handleDeleteConfirm"
      @cancel="showDeleteConfirm = false"
    />

    <ToastNotification
      :message="toastMessage"
      :type="toastType"
      @dismiss="toastMessage = null"
    />
  </div>
</template>
