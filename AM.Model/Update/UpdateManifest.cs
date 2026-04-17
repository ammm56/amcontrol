using System;

namespace AM.Model.Update
{
    /// <summary>
    /// 更新清单
    /// 由客户端从更新服务读取，用于判断是否存在新版本以及如何下载安装包。
    /// </summary>
    public class UpdateManifest
    {
        /// <summary>
        /// 应用编码。
        /// 例如：AMControlWinF。
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// 发布通道。
        /// 例如：stable / beta。
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 最新版本号。
        /// </summary>
        public string LatestVersion { get; set; }

        /// <summary>
        /// 最低支持版本号。
        /// 低于该版本时可由客户端判定为必须升级。
        /// </summary>
        public string MinSupportedVersion { get; set; }

        /// <summary>
        /// 是否强制更新。
        /// </summary>
        public bool ForceUpdate { get; set; }

        /// <summary>
        /// 更新包下载地址。
        /// </summary>
        public string PackageUrl { get; set; }

        /// <summary>
        /// 更新包 SHA256 摘要。
        /// </summary>
        public string PackageSha256 { get; set; }

        /// <summary>
        /// 更新包大小，单位字节。
        /// </summary>
        public long PackageSize { get; set; }

        /// <summary>
        /// 发布时间。
        /// </summary>
        public DateTime ReleaseTime { get; set; }

        /// <summary>
        /// 版本说明。
        /// </summary>
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// 包类型。
        /// 当前首版建议固定为 full。
        /// </summary>
        public string PackageType { get; set; }

        /// <summary>
        /// Manifest 或更新包签名。
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 升级协议版本。
        /// 用于客户端与服务端清单结构兼容控制。
        /// </summary>
        public int UpgradeScriptVersion { get; set; }
    }
}