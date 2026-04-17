using AM.Core.Base;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.License;
using System;
using System.IO;
using System.Text;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 本地授权文件服务。
    /// 统一管理 license.lic 的路径、读取、写入和备份策略。
    /// </summary>
    public class LicenseFileService : ServiceBase
    {
        /// <summary>
        /// 授权文件备份后缀。
        /// </summary>
        private const string BackupSuffix = ".bak";

        /// <summary>
        /// 授权文件临时写入后缀。
        /// </summary>
        private const string TempSuffix = ".tmp";

        protected override string MessageSourceName
        {
            get { return "LicenseFileService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        public LicenseFileService()
            : base()
        {
        }

        public LicenseFileService(IAppReporter reporter)
            : base(reporter)
        {
        }

        /// <summary>
        /// 获取授权文件完整路径。
        /// </summary>
        public string GetLicenseFilePath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                baseDirectory = Environment.CurrentDirectory;
            }

            return Path.Combine(baseDirectory, LicenseConstants.LicenseFileName);
        }

        /// <summary>
        /// 获取授权备份文件完整路径。
        /// </summary>
        public string GetLicenseBackupFilePath()
        {
            return GetLicenseFilePath() + BackupSuffix;
        }

        /// <summary>
        /// 检查授权文件是否存在。
        /// </summary>
        public bool Exists()
        {
            return File.Exists(GetLicenseFilePath());
        }

        /// <summary>
        /// 读取授权文件内容。
        /// </summary>
        public Result<string> ReadLicenseText()
        {
            try
            {
                string filePath = GetLicenseFilePath();
                if (!File.Exists(filePath))
                {
                    return WarnSilent<string>(-1, "未找到授权文件");
                }

                string content = File.ReadAllText(filePath, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(content))
                {
                    return WarnSilent<string>(-2, "授权文件内容为空");
                }

                return OkSilent(content, "读取授权文件成功");
            }
            catch (Exception ex)
            {
                return Fail<string>(-1, "读取授权文件异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 将授权文本写入本地文件。
        /// 采用临时文件 + 备份替换的方式避免中途覆盖损坏。
        /// </summary>
        public Result WriteLicenseText(string licenseText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(licenseText))
                {
                    return Fail(-1, "授权文本为空，无法写入");
                }

                string filePath = GetLicenseFilePath();
                string tempFilePath = filePath + TempSuffix;
                string backupFilePath = GetLicenseBackupFilePath();
                string directory = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(tempFilePath, licenseText.Trim(), new UTF8Encoding(false));

                if (File.Exists(filePath))
                {
                    if (File.Exists(backupFilePath))
                    {
                        File.Delete(backupFilePath);
                    }

                    File.Replace(tempFilePath, filePath, backupFilePath);
                }
                else
                {
                    File.Move(tempFilePath, filePath);
                }

                return OkLogOnly("写入授权文件成功");
            }
            catch (Exception ex)
            {
                return Fail(-1, "写入授权文件异常", ReportChannels.Log, ex);
            }
        }
    }
}