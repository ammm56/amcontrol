using AntdUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace AMControlWinF.Views.Other
{
    /// <summary>
    /// 关于信息面板。
    /// 展示程序基本信息与第三方库协议名称。
    /// </summary>
    public partial class AboutInfoControl : UserControl
    {
        public AboutInfoControl()
        {
            InitializeComponent();
            InitializeLibraryTable();
        }

        private void InitializeLibraryTable()
        {
            labelVersionValue.Text = $"版本：{AM.Tools.Tools.GetAppVersionText()}";

            tableLibraries.Columns = new ColumnCollection
            {
                new Column("Name", "名称"),
                new Column("License", "协议名称"),
                new Column("Description", "描述")
            };

            tableLibraries.Binding(new BindingList<ThirdPartyLibraryItem>(
                new List<ThirdPartyLibraryItem>
                {
                    new ThirdPartyLibraryItem
                    {
                        Name = "AntdUI",
                        License = "Apache-2.0",
                        Description = "WinForms UI 组件库"
                    },
                    new ThirdPartyLibraryItem
                    {
                        Name = "Newtonsoft.Json",
                        License = "MIT",
                        Description = "JSON 序列化/反序列化"
                    },
                    new ThirdPartyLibraryItem
                    {
                        Name = "NLog",
                        License = "BSD-3-Clause",
                        Description = "日志记录"
                    },
                    new ThirdPartyLibraryItem
                    {
                        Name = "SqlSugar",
                        License = "Apache-2.0",
                        Description = "数据库 ORM"
                    }
                }));
        }

        private class ThirdPartyLibraryItem
        {
            public string Name { get; set; }

            public string License { get; set; }

            public string Description { get; set; }
        }
    }
}