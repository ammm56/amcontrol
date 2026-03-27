# assets · 共享资产目录

本目录统一存放 `docs/` 文档中引用的**图片、截图、流程图、导出文件和其他附件**。

---

## 子目录结构

```
assets/
  architecture/    # 系统架构图、模块关系图
  screenshots/     # 界面截图
  diagrams/        # 流程图、时序图、状态机图
  simulation/      # 3D 仿真示意图、场景图
  exports/         # 导出的 PDF、Excel 等附件
```

---

## 使用规范

- 图片文件命名使用全小写英文，单词间用连字符 `-` 分隔，不使用中文文件名。
- 文档中引用本目录资源时使用相对路径，例如：

  ```markdown
  ![系统架构图](../../assets/architecture/system-overview.png)
  ```

- 单个图片文件建议压缩至 **5 MB 以内**，超大文件请使用外部图床链接。

---

## 相关文档

- [文档命名与治理规范（图片管理章节）](../00-governance/document-naming-conventions.md#7-图片与附件管理)
