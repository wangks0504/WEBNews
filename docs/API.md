# Attachments

## 为新闻添加附件（需登录）

•	POST /api/news/{id}/attachments
•	Headers：Authorization: Bearer {token}
•	Content-Type：multipart/form-data
•	Form 字段：file（文件）
•	返回：200 Ok，data 为提示文本



## 下载新闻附件

•	GET /api/attachments/{id}/download
•	返回：文件流
接收人与用户



# Auth

## 注册用户

•	POST /api/auth/register
•	Body: { "userName": "string", "password": "string", "email": "string", "realName": "string|null" }
•	成功返回：200 Ok，data 为提示文本



## 登录获取 Token

•	POST /api/auth/login
•	Body: { "userName": "string", "password": "string" }
•	成功返回：{ "token": "string", "expireTime": "ISO8601", "userId": number, "userName": "string" }
•	之后在请求头添加：Authorization: Bearer {token}



# News

## 发布新闻（需登录）

•	POST /api/news
•	Headers：Authorization: Bearer {token}
•	Body: { "title": "string", "content": "string", "summary": "string|null" }
•	返回：data: NewsDto



## 查看新闻列表

•	GET /api/news
•	返回：data: NewsListDto[]
•	字段：newsId, title, summary, author, publishTime



## 新闻详情

•	GET /api/news/{id}
•	返回：data: NewsDto
•	字段：newsId, title, content, summary, author, publishTime






# Users

## 获取用户列表（用于选择接收人）

•	GET /api/users
•	返回：data: [{ userId, userName, realName }]





## 指定接收人（需登录）

•	POST /api/news/{id}/receivers
•	Headers：Authorization: Bearer {token}
•	Body：[userId, ...]（int 数组）
•	返回：200 Ok，data 为提示文本



## 查看消息（需登录）

•	GET /api/Users/messages

•	返回类似 查看新闻列表 的格式



