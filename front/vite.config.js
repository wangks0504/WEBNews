import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src')
    }
  },
  server: {
    port: 5175, // 前端端口（固定）
    proxy: {
      // 代理后端请求（替换为你的后端端口，比如55118）
      '/api': {
        target: 'http://localhost:5000', 
        changeOrigin: true, // 跨域关键
        rewrite: (path) => path.replace(/^\/api/, '/api')
      }
    }
  }
})

