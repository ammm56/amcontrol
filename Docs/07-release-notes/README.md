# 07-release-notes · 版本发布记录

本目录用于维护 `ammm56/amcontrol` 的**版本发布记录**与**持续开发进展**。两者都与“时间维度”相关，但用途不同：

1. 正式版本发布记录：面向已发布版本；
2. 持续开发进展：面向当前活跃分支的阶段状态、已完成项和下一步优先级。

---

## 文件类型

| 类型 | 文件命名 | 用途 |
|------|----------|------|
| 正式发布记录 | `release-v<主>.<次>.<补丁>.md` | 记录已发布版本的新增、变更、修复和升级说明 |
| 持续开发进展 | `*-development-progress.md` | 记录当前分支的实现进度、剩余工作和当前优先级 |

当前目录中的持续开发进展文档：

| 文件 | 说明 |
|------|------|
| [winf-development-progress.md](winf-development-progress.md) | AMControlWinF 当前活跃分支的持续开发进展，不等同正式 release note |

---

## 命名规范

版本记录文件命名格式：

```
release-v<主版本>.<次版本>.<补丁>.md
```

示例：

```
release-v1.0.0.md
release-v1.1.0.md
```

持续开发进展文档示例：

```
winf-development-progress.md
```

---

## 版本记录模板

每个版本文件应包含以下内容：

```markdown
# v<版本号> 发布记录

**发布日期**：YYYY-MM-DD  
**发布类型**：正式版 / 预发布 / 热修复

## 新增功能

## 功能变更

## 缺陷修复

## 破坏性变更

## 升级说明
```

---

## 相关文档

- [WinForms 开发进展](winf-development-progress.md)
- [运维与部署](../05-operations/README.md)
- [文档命名规范](../00-governance/document-naming-conventions.md)
- [文档维护规则](../00-governance/document-maintenance-rules.md)
