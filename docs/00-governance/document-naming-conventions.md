# 文档命名与治理规范

**文档编号**：GOV-001  
**版本**：1.0.0  
**状态**：有效  
**最后更新**：2026-03-27  
**维护人**：项目组

---

## 目录

1. [目录命名规范](#1-目录命名规范)
2. [文件命名规范](#2-文件命名规范)
3. [日期型记录命名规范](#3-日期型记录命名规范)
4. [ADR 命名规范](#4-adr-命名规范)
5. [版本发布记录命名规范](#5-版本发布记录命名规范)
6. [建议的元数据头部](#6-建议的元数据头部)
7. [图片与附件管理](#7-图片与附件管理)

---

## 1. 目录命名规范

### 1.1 顶层分类目录（`docs/` 下一级）

- 使用**两位数字前缀 + 连字符 + 全小写英文短语**，单词间用连字符 `-` 分隔。
- 数字前缀用于固定排列顺序，便于浏览和维护。

```
docs/
  00-governance/
  01-architecture/
  02-development/
  03-features/
  04-testing/
  05-operations/
  06-user-manual/
  07-release-notes/
  08-third-party/
  09-database-config/
  10-simulation/
  assets/
```

### 1.2 子目录

- 子目录使用**全小写英文短语**，单词间用连字符 `-` 分隔，无数字前缀。
- 按功能或主题语义命名，避免使用无意义的 `misc`、`other` 等词。

```
10-simulation/
  backend/
  frontend/
  protocols/
  flow-scripts/
```

### 1.3 禁止事项

- 不使用中文目录名。
- 不使用空格、下划线（`_`）作为目录名单词分隔符（统一用 `-`）。
- 不使用大写字母。

---

## 2. 文件命名规范

### 2.1 一般文档文件

- 使用**全小写英文短语**，单词间用连字符 `-` 分隔。
- 扩展名统一使用 `.md`（Markdown 格式）。
- 每个目录必须有一个 `README.md` 作为该目录的索引文件。

**示例：**

```
net-integration-plan.md
document-naming-conventions.md
axis-config-design.md
release-v1.2.0.md
```

### 2.2 每个目录的索引文件

- 固定命名为 `README.md`（大写，遵循 GitHub/GitLab 默认渲染规则）。
- 该文件作为目录入口，简要说明目录用途，并列出目录下文档的链接索引。

### 2.3 禁止事项

- 不使用中文文件名。
- 不使用空格。
- 不使用大写字母（`README.md` 除外）。

---

## 3. 日期型记录命名规范

适用于会议记录、评审记录、问题排查记录等有明确时间属性的文档。

### 格式

```
YYYY-MM-DD-<简短主题>.md
```

**示例：**

```
2026-03-27-sprint-review.md
2026-01-15-axis-config-review.md
2025-12-08-alarm-db-migration.md
```

### 说明

- 日期部分使用 ISO 8601 格式（`YYYY-MM-DD`），便于按时间排序。
- 主题部分使用全小写英文短语，单词间用连字符分隔。

---

## 4. ADR 命名规范

架构决策记录（Architecture Decision Record，ADR）用于记录重要技术决策的背景、决策内容和影响。

### 格式

```
adr-<四位序号>-<简短主题>.md
```

**示例：**

```
adr-0001-use-websocket-for-simulation-gateway.md
adr-0002-single-result-return-model.md
adr-0003-machinecontext-as-global-motion-entry.md
```

### 建议存放位置

```
docs/01-architecture/adr/
```

### ADR 模板结构

```markdown
# ADR-XXXX：<标题>

**状态**：提议 / 已接受 / 已废弃 / 已取代  
**日期**：YYYY-MM-DD  
**决策者**：<姓名或角色>

## 背景

## 决策

## 影响

## 替代方案
```

---

## 5. 版本发布记录命名规范

适用于 `docs/07-release-notes/` 目录下的版本变更记录。

### 格式

```
release-v<主版本>.<次版本>.<补丁>.md
```

**示例：**

```
release-v1.0.0.md
release-v1.1.0.md
release-v2.0.0.md
```

### 内容建议

每个版本记录应包含：

- 版本号与发布日期
- 新增功能
- 变更说明
- 缺陷修复
- 破坏性变更（如有）
- 升级说明

---

## 6. 建议的元数据头部

对于重要的设计文档、规范文档和 ADR，建议在文件顶部添加以下元数据头部，便于识别文档状态和维护责任人。

### 格式

```markdown
**文档编号**：<分类前缀-序号>  
**版本**：<语义版本号>  
**状态**：草稿 / 评审中 / 有效 / 已废弃  
**最后更新**：YYYY-MM-DD  
**维护人**：<姓名或角色>
```

### 元数据字段说明

| 字段 | 说明 |
|------|------|
| 文档编号 | 由目录前缀和序号组成，如 `GOV-001`、`ARCH-002`、`SIM-003` |
| 版本 | 采用语义版本号，格式为 `主.次.补丁` |
| 状态 | 草稿（尚未审核）、评审中（待确认）、有效（当前适用）、已废弃（不再适用） |
| 最后更新 | ISO 8601 日期格式（`YYYY-MM-DD`） |
| 维护人 | 负责维护该文档内容准确性的人员或角色 |

### 元数据适用场景

- 治理规范文档（`00-governance/`）：**必须** 添加。
- 架构设计文档和 ADR（`01-architecture/`）：**强烈建议** 添加。
- 其他功能或开发文档：**建议** 添加，尤其是有明确责任人的文档。
- `README.md` 索引文件：**不需要** 添加元数据头部。

---

## 7. 图片与附件管理

### 7.1 存放位置

所有文档引用的图片、截图、导出文件和附件，统一存放在 `docs/assets/` 目录下，按子目录分类管理：

```
docs/assets/
  architecture/        # 架构图、系统图
  screenshots/         # 界面截图
  diagrams/            # 流程图、时序图、状态图
  simulation/          # 仿真场景图、3D 示意图
  exports/             # 导出的 PDF、Excel 等附件
```

### 7.2 文件命名

图片和附件文件采用与文档相同的命名规范：全小写英文，单词间用连字符 `-` 分隔。

**示例：**

```
docs/assets/architecture/system-overview.png
docs/assets/simulation/xyz-axis-3d-binding.png
docs/assets/screenshots/motion-monitor-view.png
```

### 7.3 引用方式

在 Markdown 文档中使用相对路径引用 `assets/` 中的资源：

```markdown
![系统架构图](../../assets/architecture/system-overview.png)
```

### 7.4 禁止事项

- 不将图片或附件直接放在文档文件旁边（如 `docs/01-architecture/system.png`），统一集中到 `assets/`。
- 不使用中文文件名。
- 不上传超过 5 MB 的图片（请压缩后上传，或使用外部图床链接）。
