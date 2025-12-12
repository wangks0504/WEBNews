<template>
  <!-- 心怡负责：F3 新闻详情页面 -->
  <!-- 黄奕负责：F5 下载附件功能 -->
  <div class="news-detail">
    <el-card v-loading="loading">
      <!-- 返回按钮 -->
      <el-button type="text" @click="goBack" style="margin-bottom: 20px">
        ← 返回列表
      </el-button>
      
      <!-- 新闻内容 -->
      <div v-if="newsDetail.newsId">
        <h2>{{ newsDetail.title }}</h2>
        
        <div class="meta-info">
          <span>作者：{{ newsDetail.author }}</span>
          <span>发布时间：{{ formatDate(newsDetail.publishTime) }}</span>
        </div>
        
        <el-divider />
        
        <div class="content" v-html="newsDetail.content"></div>
        
        <!-- 附件列表 - 黄奕负责 F5 -->
        <div v-if="newsDetail.attachments && newsDetail.attachments.length > 0" class="attachments">
          <el-divider />
          <h4>附件列表</h4>
          <el-table :data="newsDetail.attachments" style="width: 100%">
            <el-table-column prop="fileName" label="文件名" />
            <el-table-column label="文件大小">
              <template #default="{ row }">
                {{ formatFileSize(row.fileSize) }}
              </template>
            </el-table-column>
            <el-table-column prop="uploadTime" label="上传时间">
              <template #default="{ row }">
                {{ formatDate(row.uploadTime) }}
              </template>
            </el-table-column>
            <el-table-column label="操作" width="120">
              <template #default="{ row }">
                <el-button type="primary" size="small" @click="handleDownload(row)">
                  下载
                </el-button>
              </template>
            </el-table-column>
          </el-table>
        </div>
        
        <!-- 接收人列表 - 黄奕负责 F7 -->
        <div v-if="newsDetail.receivers && newsDetail.receivers.length > 0" class="receivers">
          <el-divider />
          <h4>接收人</h4>
          <el-tag
            v-for="receiver in newsDetail.receivers"
            :key="receiver.userId"
            style="margin-right: 10px">
            {{ receiver.userName }}
          </el-tag>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { ElMessage } from 'element-plus'
import { getNewsDetail, downloadAttachment } from '@/api/news'

const router = useRouter()
const route = useRoute()
const loading = ref(false)
const newsDetail = ref({})

// 获取新闻详情
const fetchNewsDetail = async () => {
  loading.value = true
  try {
    const newsId = route.params.id
    const res = await getNewsDetail(newsId)
    newsDetail.value = res
  } catch (error) {
    console.error('获取新闻详情失败：', error)
    ElMessage.error('获取新闻详情失败')
  } finally {
    loading.value = false
  }
}

// 格式化日期
const formatDate = (dateString) => {
  const date = new Date(dateString)
  return date.toLocaleString('zh-CN')
}

// 格式化文件大小
const formatFileSize = (bytes) => {
  if (bytes === 0) return '0 B'
  const k = 1024
  const sizes = ['B', 'KB', 'MB', 'GB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i]
}

// 下载附件 - 黄奕负责实现
const handleDownload = async (attachment) => {
  try {
    const response = await downloadAttachment(attachment.attachmentId)
    
    // 创建下载链接
    const url = window.URL.createObjectURL(new Blob([response.data]))
    const link = document.createElement('a')
    link.href = url
    link.setAttribute('download', attachment.fileName)
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
    
    ElMessage.success('下载成功')
  } catch (error) {
    console.error('下载失败：', error)
    ElMessage.error('下载失败')
  }
}

// 返回列表
const goBack = () => {
  router.push('/news')
}

onMounted(() => {
  fetchNewsDetail()
})
</script>

<style scoped>
.news-detail {
  padding: 20px;
}

h2 {
  margin: 0 0 15px 0;
  color: #303133;
}

.meta-info {
  display: flex;
  gap: 30px;
  color: #909399;
  font-size: 14px;
}

.content {
  margin: 20px 0;
  line-height: 1.8;
  font-size: 16px;
  color: #606266;
  white-space: pre-wrap;
}

.attachments,
.receivers {
  margin-top: 20px;
}

h4 {
  margin: 10px 0;
  color: #606266;
}
</style>

