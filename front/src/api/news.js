import request from '@/utils/request'

/**
 * 获取新闻列表
 */
export function getNewsList(params) {
  return request({
    url: '/news',
    method: 'get',
    params
  })
}

/**
 * 获取新闻详情
 */
export function getNewsDetail(id) {
  return request({
    url: `/news/${id}`,
    method: 'get'
  })
}

/**
 * 发布新闻
 */
export function createNews(data) {
  return request({
    url: '/news',
    method: 'post',
    data
  })
}

/**
 * 上传附件
 */
export function uploadAttachment(newsId, file) {
  const formData = new FormData()
  formData.append('file', file)
  
  return request({
    url: `/news/${newsId}/attachments`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  })
}

/**
 * 下载附件
 */
export function downloadAttachment(attachmentId) {
  return request({
    url: `/attachments/${attachmentId}/download`,
    method: 'get',
    responseType: 'blob'
  })
}

/**
 * 指定接收人
 */
export function setReceivers(newsId, userIds) {
  return request({
    url: `/news/${newsId}/receivers`,
    method: 'post',
    data: { userIds }
  })
}

/**
 * 获取用户列表
 */
export function getUserList() {
  return request({
    url: '/users',
    method: 'get'
  })
}

