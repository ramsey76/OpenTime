const BASE_URL = '/api'

export class ApiError extends Error {
  constructor(
    public status: number,
    public statusText: string,
    public body: string,
  ) {
    super(`${status} ${statusText}: ${body}`)
  }
}

export async function apiFetch<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${BASE_URL}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...options?.headers,
    },
    ...options,
  })

  if (!response.ok) {
    const body = await response.text()
    throw new ApiError(response.status, response.statusText, body)
  }

  if (response.status === 204) {
    return undefined as T
  }

  return response.json()
}
