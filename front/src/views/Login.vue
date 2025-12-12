<template>
  <div class="login-box">
    <h2>用户登录</h2>
    <form @submit.prevent="handleLogin">
      <div class="form-item">
        <label>用户名：</label>
        <input 
          type="text" 
          v-model="form.userName" 
          placeholder="请输入用户名" 
          required
        >
      </div>
      <div class="form-item">
        <label>密码：</label>
        <input 
          type="password" 
          v-model="form.password" 
          placeholder="请输入密码" 
          required
        >
      </div>
      <button type="submit" class="submit-btn">登录</button>
    </form>
    <p class="switch-link" @click="$router.push('/register')">
      没有账号？立即注册
    </p>
    <!-- 修复：通过变量判断Token是否存在，而非直接用localStorage -->
    <button class="logout-btn" @click="handleLogout" v-if="hasToken">
      退出登录
    </button>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { loginApi } from '@/api/auth'

const router = useRouter()
const form = ref({
  userName: '',
  password: ''
})

// 修复：用computed变量封装localStorage读取逻辑
const hasToken = computed(() => {
  return localStorage.getItem('token') !== null
})

// 登录逻辑
const handleLogin = async () => {
  try {
    const res = await loginApi(form.value)
    const { token, userId, userName } = res.data.data
    
    localStorage.setItem('token', token)
    localStorage.setItem('userId', userId)
    localStorage.setItem('userName', userName)

    alert('登录成功！')
    router.push('/home')
  } catch (err) {
    const errMsg = err.response?.data?.message || '用户名/密码错误'
    alert(errMsg)
  }
}

// 退出登录
const handleLogout = () => {
  localStorage.removeItem('token')
  localStorage.removeItem('userId')
  localStorage.removeItem('userName')
  alert('已退出登录')
  router.push('/login')
}
</script>

<style scoped>
.login-box {
  width: 400px;
  margin: 100px auto;
  padding: 30px;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 0 10px rgba(0,0,0,0.1);
}
h2 {
  text-align: center;
  margin-bottom: 20px;
  color: #333;
}
.form-item {
  margin-bottom: 15px;
}
label {
  display: block;
  margin-bottom: 5px;
  font-size: 14px;
  color: #666;
}
input {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}
.submit-btn {
  width: 100%;
  padding: 12px;
  background: #42b983;
  color: #fff;
  border: none;
  border-radius: 4px;
  font-size: 16px;
  cursor: pointer;
  margin-top: 10px;
}
.submit-btn:hover {
  background: #359e6d;
}
.switch-link {
  text-align: center;
  margin-top: 15px;
  font-size: 14px;
  color: #42b983;
  cursor: pointer;
}
.switch-link:hover {
  text-decoration: underline;
}
.logout-btn {
  width: 100%;
  padding: 10px;
  background: #ff4d4f;
  color: #fff;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  margin-top: 10px;
}
.logout-btn:hover {
  background: #ff7875;
}
</style>