<template>
  <!-- 心怡负责：F2 发布新闻页面 -->
  <!-- 黄奕负责：F4 上传附件、F7 指定接收人功能 -->
  <div class="create-news">
    <el-card>
      <template #header>
        <h3>发布新闻</h3>
      </template>
      
      <el-form
        ref="newsFormRef"
        :model="newsForm"
        :rules="newsRules"
        label-width="100px">
        <el-form-item label="新闻标题" prop="title">
          <el-input
            v-model="newsForm.title"
            placeholder="请输入新闻标题"
            maxlength="200"
            show-word-limit
            clearable />
        </el-form-item>
        
        <el-form-item label="新闻摘要" prop="summary">
          <el-input
            v-model="newsForm.summary"
            type="textarea"
            :rows="3"
            placeholder="请输入新闻摘要（可选）"
            maxlength="500"
            show-word-limit
            clearable />
        </el-form-item>
        
        <el-form-item label="新闻内容" prop="content">
          <el-input
            v-model="newsForm.content"
            type="textarea"
            :rows="10"
            placeholder="请输入新闻内容"
            clearable />
        </el-form-item>
        
        <!-- 附件上传 - 黄奕负责 F4 -->
        <el-form-item label="上传附件">
          <el-upload
            ref="uploadRef"
            :auto-upload="false"
            :on-change="handleFileChange"
            :file-list="fileList"
            multiple>
            <el-button type="primary">选择文件</el-button>
            <template #tip>
              <div class="el-upload__tip">
                支持格式：pdf, doc, docx, xls, xlsx, jpg, png, zip（最大10MB）
              </div>
            </template>
          </el-upload>
        </el-form-item>
        
        <!-- 指定接收人 - 黄奕负责 F7 -->
        <el-form-item label="指定接收人">
          <el-button type="primary" @click="showReceiverDialog">选择接收人</el-button>
          <div v-if="selectedReceivers.length > 0" style="margin-top: 10px">
            <el-tag
              v-for="user in selectedReceivers"
              :key="user.userId"
              closable
              @close="removeReceiver(user.userId)"
              style="margin-right: 10px">
              {{ user.userName }}
            </el-tag>
          </div>
        </el-form-item>
        
        <el-form-item>
          <el-button type="primary" :loading="loading" @click="handleSubmit">
            发布新闻
          </el-button>
          <el-button @click="handleCancel">取消</el-button>
        </el-form-item>
      </el-form>
    </el-card>
    
    <!-- 选择接收人对话框 - 黄奕负责 F7 -->
    <el-dialog
      v-model="receiverDialogVisible"
      title="选择接收人"
      width="500px">
      <el-transfer
        v-model="selectedUserIds"
        :data="userList"
        :titles="['可选用户', '已选用户']"
        :props="{ key: 'userId', label: 'userName' }" />
      <template #footer>
        <el-button @click="receiverDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="confirmReceivers">确定</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { createNews, uploadAttachment, setReceivers, getUserList } from '@/api/news'

const router = useRouter()
const newsFormRef = ref(null)
const uploadRef = ref(null)
const loading = ref(false)

const newsForm = ref({
  title: '',
  summary: '',
  content: ''
})

const newsRules = {
  title: [
    { required: true, message: '请输入新闻标题', trigger: 'blur' },
    { max: 200, message: '标题最多200个字符', trigger: 'blur' }
  ],
  content: [
    { required: true, message: '请输入新闻内容', trigger: 'blur' }
  ]
}

// 附件相关 - 黄奕负责
const fileList = ref([])

const handleFileChange = (file, files) => {
  fileList.value = files
}

// 接收人相关 - 黄奕负责
const receiverDialogVisible = ref(false)
const userList = ref([])
const selectedUserIds = ref([])
const selectedReceivers = ref([])

// 获取用户列表
const fetchUserList = async () => {
  try {
    const res = await getUserList()
    userList.value = res || []
  } catch (error) {
    console.error('获取用户列表失败：', error)
  }
}

// 显示接收人对话框
const showReceiverDialog = () => {
  receiverDialogVisible.value = true
}

// 确认选择接收人
const confirmReceivers = () => {
  selectedReceivers.value = userList.value.filter(user =>
    selectedUserIds.value.includes(user.userId)
  )
  receiverDialogVisible.value = false
}

// 移除接收人
const removeReceiver = (userId) => {
  selectedReceivers.value = selectedReceivers.value.filter(u => u.userId !== userId)
  selectedUserIds.value = selectedUserIds.value.filter(id => id !== userId)
}

// 提交表单
const handleSubmit = async () => {
  if (!newsFormRef.value) return
  
  await newsFormRef.value.validate(async (valid) => {
    if (valid) {
      loading.value = true
      try {
        // 1. 发布新闻
        const newsRes = await createNews(newsForm.value)
        const newsId = newsRes.newsId
        
        // 2. 上传附件（如果有）- 黄奕负责实现
        if (fileList.value.length > 0) {
          for (const fileItem of fileList.value) {
            await uploadAttachment(newsId, fileItem.raw)
          }
        }
        
        // 3. 设置接收人（如果有）- 黄奕负责实现
        if (selectedUserIds.value.length > 0) {
          await setReceivers(newsId, selectedUserIds.value)
        }
        
        ElMessage.success('新闻发布成功')
        router.push('/news')
      } catch (error) {
        console.error('发布失败：', error)
        ElMessage.error('发布失败，请重试')
      } finally {
        loading.value = false
      }
    }
  })
}

// 取消
const handleCancel = () => {
  router.push('/news')
}

onMounted(() => {
  fetchUserList()
})
</script>

<style scoped>
.create-news {
  padding: 20px;
}

h3 {
  margin: 0;
}

.el-upload__tip {
  color: #909399;
  font-size: 12px;
}
</style>

