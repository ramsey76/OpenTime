<script setup lang="ts">
import { watch, useTemplateRef } from 'vue'

const props = defineProps<{
  open: boolean
  title: string
  message: string
}>()

const emit = defineEmits<{
  confirm: []
  cancel: []
}>()

const dialogRef = useTemplateRef('dialog')

watch(() => props.open, (isOpen) => {
  if (isOpen) {
    dialogRef.value?.showModal()
  } else {
    dialogRef.value?.close()
  }
})
</script>

<template>
  <dialog ref="dialog" class="modal" @close="emit('cancel')">
    <div class="modal-box">
      <h3 class="text-lg font-bold">{{ title }}</h3>
      <p class="py-4">{{ message }}</p>
      <div class="modal-action">
        <button class="btn btn-ghost" @click="emit('cancel')">Cancel</button>
        <button class="btn btn-error" @click="emit('confirm')">Delete</button>
      </div>
    </div>
    <form method="dialog" class="modal-backdrop">
      <button>close</button>
    </form>
  </dialog>
</template>
