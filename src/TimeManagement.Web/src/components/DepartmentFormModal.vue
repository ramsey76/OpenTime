<script setup lang="ts">
import { ref, watch, computed, useTemplateRef } from 'vue'
import type { DepartmentDto } from '../api/types'

const props = defineProps<{
  open: boolean
  department?: DepartmentDto | null
  allDepartments: DepartmentDto[]
}>()

const emit = defineEmits<{
  submit: [data: { name: string; code: string; parentDepartmentId: string | null }]
  cancel: []
}>()

const dialogRef = useTemplateRef('dialog')
const name = ref('')
const code = ref('')
const parentDepartmentId = ref<string | null>(null)
const nameError = ref('')
const codeError = ref('')

watch(() => props.open, (isOpen) => {
  if (isOpen) {
    name.value = props.department?.name ?? ''
    code.value = props.department?.code ?? ''
    parentDepartmentId.value = props.department?.parentDepartmentId ?? null
    nameError.value = ''
    codeError.value = ''
    dialogRef.value?.showModal()
  } else {
    dialogRef.value?.close()
  }
})

const availableParents = computed(() => {
  if (!props.department) {
    return props.allDepartments
  }

  const excludeIds = new Set<string>()
  excludeIds.add(props.department.id)

  function addDescendants(parentId: string) {
    for (const dept of props.allDepartments) {
      if (dept.parentDepartmentId === parentId && !excludeIds.has(dept.id)) {
        excludeIds.add(dept.id)
        addDescendants(dept.id)
      }
    }
  }
  addDescendants(props.department.id)

  return props.allDepartments.filter(d => !excludeIds.has(d.id))
})

function validate(): boolean {
  nameError.value = ''
  codeError.value = ''
  let valid = true

  if (!name.value.trim()) {
    nameError.value = 'Name is required'
    valid = false
  } else if (name.value.length > 100) {
    nameError.value = 'Name must be 100 characters or fewer'
    valid = false
  }

  if (!code.value.trim()) {
    codeError.value = 'Code is required'
    valid = false
  } else if (code.value.length > 10) {
    codeError.value = 'Code must be 10 characters or fewer'
    valid = false
  }

  return valid
}

function handleSubmit() {
  if (!validate()) return
  emit('submit', {
    name: name.value.trim(),
    code: code.value.trim(),
    parentDepartmentId: parentDepartmentId.value || null,
  })
}
</script>

<template>
  <dialog ref="dialog" class="modal" @close="emit('cancel')">
    <div class="modal-box">
      <h3 class="text-lg font-bold">
        {{ department ? 'Edit Department' : 'Add Department' }}
      </h3>
      <form @submit.prevent="handleSubmit" class="mt-4 space-y-4">
        <div class="form-control w-full">
          <label class="label"><span class="label-text">Name *</span></label>
          <input
            v-model="name"
            type="text"
            maxlength="100"
            class="input input-bordered w-full"
            :class="{ 'input-error': nameError }"
          />
          <label v-if="nameError" class="label">
            <span class="label-text-alt text-error">{{ nameError }}</span>
          </label>
        </div>
        <div class="form-control w-full">
          <label class="label"><span class="label-text">Code *</span></label>
          <input
            v-model="code"
            type="text"
            maxlength="10"
            class="input input-bordered w-full"
            :class="{ 'input-error': codeError }"
          />
          <label v-if="codeError" class="label">
            <span class="label-text-alt text-error">{{ codeError }}</span>
          </label>
        </div>
        <div class="form-control w-full">
          <label class="label"><span class="label-text">Parent Department</span></label>
          <select
            v-model="parentDepartmentId"
            class="select select-bordered w-full"
          >
            <option :value="null">— None (root department)</option>
            <option
              v-for="dept in availableParents"
              :key="dept.id"
              :value="dept.id"
            >
              {{ dept.name }} ({{ dept.code }})
            </option>
          </select>
        </div>
        <div class="modal-action">
          <button type="button" class="btn btn-ghost" @click="emit('cancel')">Cancel</button>
          <button type="submit" class="btn btn-primary">
            {{ department ? 'Save' : 'Create' }}
          </button>
        </div>
      </form>
    </div>
    <form method="dialog" class="modal-backdrop">
      <button>close</button>
    </form>
  </dialog>
</template>
