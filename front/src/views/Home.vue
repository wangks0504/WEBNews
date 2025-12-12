<template>
  <div class="home-box">
    <div class="header">
      <h1>WEBNews 首页</h1>
      <div class="user-info">
        <span>欢迎：{{ userName }}</span>
        <button @click="handleLogout">退出登录</button>
      </div>
    </div>
    <div class="content">
      <p>Token已自动携带，其他功能接口可正常调用</p>
      <p>当前Token：{{ token }}</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const userName = ref('')
const token = ref('')

// 页面加载时读取localStorage里的用户信息
onMounted(() => {
  userName.value = localStorage.getItem('userName') || '未登录'
  token.value = localStorage.getItem('token') || '无'
  
  // 如果没有Token，自动跳登录页
  if (!localStorage.getItem('token')) {
    router.push('/login')
  }
})

// 退出登录：清除Token
const handleLogout = () => {
  localStorage.removeItem('token')
  localStorage.removeItem('userId')
  localStorage.removeItem('userName')
  alert('已退出登录')
  router.push('/login')
}
</script>

<style scoped>
.home-box {
  width: 800px;
  margin: 50px auto;
  padding: 30px;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 0 10px rgba(0,0,0,0.1);
}
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #eee;
  padding-bottom: 20px;
  margin-bottom: 20px;
}
.user-info {
  display: flex;
  align-items: center;
  gap: 15px;
}
.user-info button {
  padding: 6px 12px;
  background: #ff4d4f;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
.user-info button:hover {
  background: #ff7875;
}
.content {
  line-height: 2;
  color: #666;
}
</style>