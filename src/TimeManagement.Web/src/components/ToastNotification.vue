<script setup lang="ts">
import { ref, watch } from 'vue'

const props = defineProps<{
  message: string | null
  type?: 'error' | 'success'
}>()

const emit = defineEmits<{
  dismiss: []
}>()

const visible = ref(false)

watch(() => props.message, (msg) => {
  if (msg) {
    visible.value = true
    setTimeout(() => {
      visible.value = false
      emit('dismiss')
    }, 5000)
  }
})

function close() {
  visible.value = false
  emit('dismiss')
}
</script>

<template>
  <div v-if="visible && message" class="toast toast-top toast-end z-50">
    <div
      class="alert"
      :class="type === 'success' ? 'alert-success' : 'alert-error'"
    >
      <span>{{ message }}</span>
      <button class="btn btn-ghost btn-xs" @click="close">&times;</button>
    </div>
  </div>
</template>
