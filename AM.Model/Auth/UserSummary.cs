using System;

namespace AM.Model.Auth
{
    /// <summary>
    /// 用户管理页展示模型。
    /// </summary>
    public class UserSummary
    {
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string UserName { get; set; }

        public string RoleCode { get; set; }

        public string RoleName { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public string Remark { get; set; }

        public string AvatarId
        {
            get { return string.IsNullOrWhiteSpace(LoginName) ? "guest" : LoginName; }
        }

        public string DisplayName
        {
            get { return string.IsNullOrWhiteSpace(UserName) ? LoginName : UserName; }
        }

        public string RoleDisplayName
        {
            get { return string.IsNullOrWhiteSpace(RoleName) ? "未分配" : RoleName; }
        }

        public string StateText
        {
            get { return IsEnabled ? "已启用" : "已禁用"; }
        }

        public string LastLoginTimeText
        {
            get { return LastLoginTime.HasValue ? LastLoginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-"; }
        }
    }
}