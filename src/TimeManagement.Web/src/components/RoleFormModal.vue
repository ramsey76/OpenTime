<script setup lang="ts">
import { ref, watch, useTemplateRef } from 'vue'
import type { RoleDto } from '../api/types'

const props = defineProps<{
  open: boolean
  role?: RoleDto | null
}>()

const emit = defineEmits<{
  submit: [data: { name: string; description: string }]
  cancel: []
}>()

const dialogRef = useTemplateRef('dialog')
const name = ref('')
const description = ref('')
const nameError = ref('')

watch(() => props.open, (isOpen) => {
  if (isOpen) {
    name.value = props.role?.name ?? ''
    description.value = props.role?.description ?? ''
    nameError.value = ''
    dialogRef.value?.showModal()
  } else {
    dialogRef.value?.close()
  }
})

function validate(): boolean {
  nameError.value = ''
  if (!name.value.trim()) {
    nameError.value = 'Name is required'
    return false
  }
  if (name.value.length > 50) {
    nameError.value = 'Name must be 50 characters or fewer'
    return false
  }
  return true
}

function handleSubmit() {
  if (!validate()) return
  emit('submit', { name: name.value.trim(), description: description.value.trim() })
}
</script>

<template>
  <dialog ref="dialog" class="modal" @close="emit('cancel')">
    <div class="modal-box">
      <h3 class="text-lg font-bold">
        {{ role ? 'Edit Role' : 'Add Role' }}
      </h3>
      <form @submit.prevent="handleSubmit" class="mt-4 space-y-4">
        <div class="form-control w-full">
          <label class="label"><span class="label-text">Name *</span></label>
          <input
            v-model="name"
            type="text"
            maxlength="50"
            class="input input-bordered w-full"
            :class="{ 'input-error': nameError }"
          />
          <label v-if="nameError" class="label">
            <span class="label-text-alt text-error">{{ nameError }}</span>
          </label>
        </div>
        <div class="form-control w-full">
          <label class="label"><span class="label-text">Description</span></label>
          <textarea
            v-model="description"
            rows="3"
            class="textarea textarea-bordered w-full"
          />
        </div>
        <div class="modal-action">
          <button type="button" class="btn btn-ghost" @click="emit('cancel')">Cancel</button>
          <button type="submit" class="btn btn-primary">
            {{ role ? 'Save' : 'Create' }}
          </button>
        </div>
      </form>
    </div>
    <form method="dialog" class="modal-backdrop">
      <button>close</button>
    </form>
  </dialog>
</template>
