# WEB新闻发布系统

基于 ASP.NET Core Web API + SQL Server 开发的新闻发布系统

## 👥 团队成员

- **王世强** - 组长（项目架构 + 协调）
- **赵畅** - 技术支持（JWT配置 + 技术指导）
- **欧阳** - 开发（用户登录模块 F6）
- **心怡** - 开发（新闻模块 F1/F2/F3）
- **黄奕** - 开发（附件模块 F4/F5/F7 + 测试）

## 🚀 快速开始

### 1. 环境要求

- .NET 6/7 SDK
- SQL Server 2019+
- Visual Studio 2022 或 VS Code

### 2. 克隆项目

```bash
git clone [项目地址]
cd WEBNews
```

### 3. 配置数据库

修改 `WEBNews.API/appsettings.json` 中的数据库连接字符串：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WEBNewsDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 4. 创建数据库

```bash
cd WEBNews.API
dotnet ef database update
```

### 5. 运行项目

```bash
dotnet run
```

访问 Swagger 文档：`https://localhost:5001/swagger`

## 📁 项目结构

```
WEBNews/
├── WEBNews.API/           # Web API 项目（控制器、中间件）
├── WEBNews.Models/        # 数据模型（实体类、DTO）
├── WEBNews.Data/          # 数据访问层（EF Core、DbContext）
├── WEBNews.Services/      # 业务逻辑层（Services）
└── docs/                  # 项目文档
    ├── 任务分配.md        # 团队任务分配
    ├── API接口规范.md     # API接口文档
    └── 开发流程图.md      # 开发流程
```

## 📋 功能模块

| 模块 | 功能 | 负责人 |
|------|------|--------|
| F1 | 查看新闻列表 | 心怡 |
| F2 | 发布新闻 | 心怡 |
| F3 | 查看新闻详情 | 心怡 |
| F4 | 添加附件 | 黄奕 |
| F5 | 下载附件 | 黄奕 |
| F6 | 用户注册登录 | 欧阳 |
| F7 | 指定接收人 | 黄奕 |

## 🔧 技术栈

- **后端框架**: ASP.NET Core Web API 6.0+
- **数据库**: SQL Server 2019
- **ORM**: Entity Framework Core
- **认证**: JWT Bearer Token
- **API文档**: Swagger/OpenAPI

## 📖 文档

- [任务分配文档](./docs/任务分配.md)
- [API接口规范](./docs/API接口规范.md)
- [开发流程图](./docs/开发流程图.md)

## 🌿 Git 分支管理

- `main` - 主分支（王世强管理）
- `dev-auth` - 用户模块（欧阳）
- `dev-news` - 新闻模块（心怡）
- `dev-attachment` - 附件模块（黄奕）

## ⚠️ 注意事项

1. 每完成一个功能立即提交代码
2. 遇到问题先问AI，再问赵畅
3. 使用Swagger测试每个接口
4. 保持代码注释清晰

## 📞 联系方式

- 技术问题：@赵畅
- 协调问题：@王世强
- 项目群：[微信群]

---

**开发时间**: 2025年12月  
**课程**: .NET平台开发技术  
**项目类型**: 课程设计

