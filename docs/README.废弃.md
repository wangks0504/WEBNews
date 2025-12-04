# WEB新闻发布系统 - ASP.NET Core Web API 实现流程

## 📋 项目概述

使用 ASP.NET Core Web API + SQL Server 2019 开发新闻发布系统

## 🚀 实现流程

### 1. 创建项目（5分钟）

```bash
# 创建解决方案
dotnet new sln -n WEBNews

# 创建 Web API 项目
dotnet new webapi -n WEBNews.API

# 创建类库项目
dotnet new classlib -n WEBNews.Models
dotnet new classlib -n WEBNews.Data
dotnet new classlib -n WEBNews.Services

# 添加到解决方案
dotnet sln add WEBNews.API
dotnet sln add WEBNews.Models
dotnet sln add WEBNews.Data
dotnet sln add WEBNews.Services
```

### 2. 安装依赖包（5分钟）

```bash
# 在 WEBNews.Data 项目中
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

# 在 WEBNews.API 项目中
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Swashbuckle.AspNetCore
```

### 3. 数据库设计（15分钟）

创建4个主要数据表：
- **Users** - 用户表
- **News** - 新闻表
- **Attachments** - 附件表
- **NewsReceivers** - 新闻接收人表

### 4. 实现功能模块（主要工作）

#### F6: 用户登录（先做）
- 注册接口：`POST /api/auth/register`
- 登录接口：`POST /api/auth/login`
- 返回 JWT Token

#### F2: 发布新闻
- 接口：`POST /api/news`
- 保存标题、内容、作者

#### F1: 查看新闻列表
- 接口：`GET /api/news`
- 返回所有新闻标题列表

#### F3: 查看新闻详情
- 接口：`GET /api/news/{id}`
- 返回新闻完整内容

#### F4: 添加附件
- 接口：`POST /api/news/{id}/attachments`
- 上传文件到服务器

#### F5: 下载附件
- 接口：`GET /api/attachments/{id}/download`
- 返回文件流

#### F7: 指定接收人
- 接口：`POST /api/news/{id}/receivers`
- 关联新闻和用户

### 5. 测试（使用 Swagger）

启动项目后访问：`https://localhost:5001/swagger`

### 6. 撰写报告

包含：需求分析、概要设计、详细设计、系统测试、总结

## 📁 项目结构

```
WEBNews/
├── WEBNews.API/           # Web API（控制器）
├── WEBNews.Services/      # 业务逻辑
├── WEBNews.Data/          # 数据访问（EF Core）
└── WEBNews.Models/        # 数据模型
```

## 🎯 开发顺序

1. 搭建项目 + 数据库
2. F6 用户登录
3. F2 发布新闻
4. F1 查看列表
5. F3 查看详情
6. F4 上传附件
7. F5 下载附件
8. F7 指定接收人

## ⏱️ 预计时间

- 项目搭建：1小时
- 核心功能：8-10小时
- 测试调试：2小时
- 撰写报告：3-4小时

**总计：2-3天完成**

