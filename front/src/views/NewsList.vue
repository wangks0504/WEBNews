<template>
  <!-- 心怡负责：F1 新闻列表页面 -->
  <div class="news-list">
    <el-card>
      <template #header>
        <div class="card-header">
          <h3>新闻列表</h3>
          <el-button type="primary" @click="goToCreate">发布新闻</el-button>
        </div>
      </template>
      
      <!-- 新闻列表 -->
      <el-table
        v-loading="loading"
        :data="newsList"
        style="width: 100%"
        @row-click="handleRowClick">
        <el-table-column prop="newsId" label="ID" width="80" />
        <el-table-column prop="title" label="标题" min-width="200" />
        <el-table-column prop="summary" label="摘要" min-width="300" show-overflow-tooltip />
        <el-table-column prop="author" label="作者" width="120" />
        <el-table-column prop="publishTime" label="发布时间" width="180">
          <template #default="{ row }">
            {{ formatDate(row.publishTime) }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="120">
          <template #default="{ row }">
            <el-button type="text" @click.stop="viewDetail(row.newsId)">查看详情</el-button>
          </template>
        </el-table-column>
      </el-table>
      
      <!-- 分页 -->
      <div class="pagination">
        <el-pagination
          v-model:current-page="currentPage"
          v-model:page-size="pageSize"
          :total="total"
          :page-sizes="[10, 20, 50]"
          layout="total, sizes, prev, pager, next, jumper"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange" />
      </div>
    </el-card>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { getNewsList } from '@/api/news'

const router = useRouter()
const loading = ref(false)
const newsList = ref([])
const currentPage = ref(1)
const pageSize = ref(10)
const total = ref(0)

// 获取新闻列表
const fetchNewsList = async () => {
  loading.value = true
  try {
    const res = await getNewsList({
      page: currentPage.value,
      pageSize: pageSize.value
    })
    newsList.value = res.list || []
    total.value = res.total || 0
  } catch (error) {
    console.error('获取新闻列表失败：', error)
  } finally {
    loading.value = false
  }
}

// 格式化日期
const formatDate = (dateString) => {
  const date = new Date(dateString)
  return date.toLocaleString('zh-CN')
}

// 查看详情
const viewDetail = (newsId) => {
  router.push(`/news/${newsId}`)
}

// 行点击事件
const handleRowClick = (row) => {
  viewDetail(row.newsId)
}

// 去发布新闻
const goToCreate = () => {
  router.push('/create')
}

// 分页事件
const handleSizeChange = () => {
  fetchNewsList()
}

const handleCurrentChange = () => {
  fetchNewsList()
}

onMounted(() => {
  fetchNewsList()
})
</script>

<style scoped>
.news-list {
  padding: 20px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.card-header h3 {
  margin: 0;
}

.pagination {
  margin-top: 20px;
  display: flex;
  justify-content: flex-end;
}

:deep(.el-table__row) {
  cursor: pointer;
}

:deep(.el-table__row:hover) {
  background-color: #f5f7fa;
}
</style>

