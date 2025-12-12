import axios from 'axios'

// 创建axios实例
const request = axios.create({
  baseURL: '/api',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json;charset=utf-8'
  }
})

// 请求拦截器：自动添加Token到请求头（关键！）
request.interceptors.request.use(
  (config) => {
    // 从localStorage读取Token
    const token = localStorage.getItem('token')
    if (token) {
      // 后端通用的Token传递格式：Bearer + 空格 + Token
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// 响应拦截器：统一处理后端返回的错误（可选，优化体验）
request.interceptors.response.use(
  (response) => {
    return response
  },
  (error) => {
    // 401表示Token过期/未登录，自动跳登录页
    if (error.response?.status === 401) {
      alert('登录状态已过期，请重新登录')
      localStorage.removeItem('token')
      window.location.href = '/#/login'
    }
    return Promise.reject(error)
  }
)

// 注册接口
export const registerApi = (data) => {
  return request.post('/Auth/register', data)
}

// 登录接口
export const loginApi = (data) => {
  return request.post('/Auth/login', data)
}