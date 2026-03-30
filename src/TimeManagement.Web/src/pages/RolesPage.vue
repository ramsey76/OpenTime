<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRolesStore } from '../stores/useRolesStore'
import { ApiError } from '../api/client'
import type { RoleDto } from '../api/types'
import RoleFormModal from '../components/RoleFormModal.vue'
import ConfirmDialog from '../components/ConfirmDialog.vue'
import ToastNotification from '../components/ToastNotification.vue'

const store = useRolesStore()

const showFormModal = ref(false)
const editingRole = ref<RoleDto | null>(null)
const showDeleteConfirm = ref(false)
const deletingRole = ref<RoleDto | null>(null)
const toastMessage = ref<string | null>(null)
const toastType = ref<'error' | 'success'>('error')

onMounted(() => {
  store.fetchRoles()
})

function openCreateModal() {
  editingRole.value = null
  showFormModal.value = true
}

function openEditModal(role: RoleDto) {
  editingRole.value = role
  showFormModal.value = true
}

function openDeleteConfirm(role: RoleDto) {
  deletingRole.value = role
  showDeleteConfirm.value = true
}

async function handleFormSubmit(data: { name: string; description: string }) {
  try {
    if (editingRole.value) {
      await store.updateRole(editingRole.value.id, data)
    } else {
      await store.createRole(data)
    }
    showFormModal.value = false
  } catch (e) {
    if (e instanceof ApiError && e.status === 409) {
      toastType.value = 'error'
      toastMessage.value = 'A role with this name already exists.'
    } else {
      toastType.value = 'error'
      toastMessage.value = e instanceof Error ? e.message : 'An error occurred.'
    }
  }
}

async function handleDeleteConfirm() {
  if (!deletingRole.value) return
  try {
    await store.deleteRole(deletingRole.value.id)
    showDeleteConfirm.value = false
  } catch (e) {
    showDeleteConfirm.value = false
    if (e instanceof ApiError && e.status === 409) {
      toastType.value = 'error'
      toastMessage.value = 'This role is assigned to employees and cannot be deleted.'
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
      <h1 class="text-2xl font-bold">Roles</h1>
      <button class="btn btn-primary btn-sm" @click="openCreateModal">
        + Add Role
      </button>
    </div>

    <div v-if="store.loading" class="flex justify-center p-8">
      <span class="loading loading-spinner loading-md"></span>
    </div>
    <div v-else-if="store.error" class="alert alert-error">{{ store.error }}</div>
    <div v-else-if="store.roles.length === 0" class="text-base-content/50">
      No roles have been created yet.
    </div>
    <div v-else class="overflow-x-auto">
      <table class="table table-zebra">
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th class="text-right">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="role in store.roles" :key="role.id">
            <td class="font-medium">{{ role.name }}</td>
            <td>{{ role.description || '—' }}</td>
            <td class="text-right space-x-2">
              <button class="btn btn-info btn-link btn-sm" @click="openEditModal(role)">Edit</button>
              <button class="btn btn-error btn-link btn-sm" @click="openDeleteConfirm(role)">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <RoleFormModal
      :open="showFormModal"
      :role="editingRole"
      @submit="handleFormSubmit"
      @cancel="showFormModal = false"
    />

    <ConfirmDialog
      :open="showDeleteConfirm"
      title="Delete Role"
      :message="`Are you sure you want to delete the role '${deletingRole?.name}'?`"
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
