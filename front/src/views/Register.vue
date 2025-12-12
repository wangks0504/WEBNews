<template>
  <div class="register-box">
    <h2>用户注册</h2>
    <form @submit.prevent="handleRegister">
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
        <label>真实姓名：</label>
        <input 
          type="text" 
          v-model="form.realName" 
          placeholder="请输入真实姓名" 
          required
        >
      </div>
      <div class="form-item">
        <label>邮箱：</label>
        <input 
          type="email" 
          v-model="form.email" 
          placeholder="请输入邮箱" 
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
      <button type="submit" class="submit-btn">注册</button>
    </form>
    <p class="switch-link" @click="$router.push('/login')">
      已有账号？立即登录
    </p>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { registerApi } from '@/api/auth'

// 表单数据（完全匹配后端RegisterDto）
const form = ref({
  userName: '',   // 对应后端UserName
  realName: '',   // 对应后端RealName
  email: '',      // 对应后端Email
  password: ''    // 对应后端Password
})

// 注册逻辑
const handleRegister = async () => {
  try {
    const res = await registerApi(form.value)
    // 后端返回注册成功
    alert(res.data.message || '注册成功！')
    // 跳登录页
    window.location.href = '/#/login'
  } catch (err) {
    // 捕获后端错误（409用户名已存在/400参数错误）
    const errMsg = err.response?.data?.message || '注册失败，请重试'
    alert(errMsg)
  }
}
</script>

<style scoped>
.register-box {
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
</style>
