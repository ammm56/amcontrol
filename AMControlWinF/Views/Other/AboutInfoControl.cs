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
            var appVersionText = AM.Tools.Tools.GetAppVersionText();
            string language = AM.Tools.Tools.GetCurrentLanguage();
            labelProgramName.Text = AM.Tools.Tools.IsEnglishLanguage(language) ? "AM Motion Control System" : "AM运动控制系统";

            labelAuthorValue.Text = AM.Tools.Tools.IsEnglishLanguage(language) ? "Developers: amm" : "开发者：amm";

            labelemail.Text = AM.Tools.Tools.IsEnglishLanguage(language) ? "Email: tang.am@foxmail.com" : "邮箱：tang.am@foxmail.com";

            labelVersionValue.Text = AM.Tools.Tools.IsEnglishLanguage(language)
                ? "Version: " + appVersionText
                : "版本：" + appVersionText;

            labelLibrariesTitle.Text = AM.Tools.Tools.IsEnglishLanguage(language) ? "Third-party libraries and protocols" : "第三方库与协议";

            tableLibraries.Columns = new ColumnCollection
            {
                new Column("Name", AM.Tools.Tools.IsEnglishLanguage(language) ? "Name" : "名称"),
                new Column("License", AM.Tools.Tools.IsEnglishLanguage(language) ? "License" : "协议名称"),
                new Column("Description", AM.Tools.Tools.IsEnglishLanguage(language) ? "Description" : "描述")
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