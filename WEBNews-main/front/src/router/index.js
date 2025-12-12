import { createRouter, createWebHistory } from 'vue-router'
import Layout from '@/views/Layout.vue'
import Login from '@/views/Login.vue'
import Register from '@/views/Register.vue'
import NewsList from '@/views/NewsList.vue'
import NewsDetail from '@/views/NewsDetail.vue'
import CreateNews from '@/views/CreateNews.vue'

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: Login,
    meta: { title: '登录' }
  },
  {
    path: '/register',
    name: 'Register',
    component: Register,
    meta: { title: '注册' }
  },
  {
    path: '/',
    component: Layout,
    redirect: '/news',
    children: [
      {
        path: 'news',
        name: 'NewsList',
        component: NewsList,
        meta: { title: '新闻列表', requireAuth: true }
      },
      {
        path: 'news/:id',
        name: 'NewsDetail',
        component: NewsDetail,
        meta: { title: '新闻详情', requireAuth: true }
      },
      {
        path: 'create',
        name: 'CreateNews',
        component: CreateNews,
        meta: { title: '发布新闻', requireAuth: true }
      }
    ]
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// 路由守卫
router.beforeEach((to, from, next) => {
  // 设置页面标题
  document.title = to.meta.title ? `${to.meta.title} - WEB新闻系统` : 'WEB新闻系统'
  
  // 判断是否需要登录
  if (to.meta.requireAuth) {
    const token = localStorage.getItem('token')
    if (!token) {
      next('/login')
    } else {
      next()
    }
  } else {
    next()
  }
})

export default router

