using System;

namespace AM.Model.Auth
{
    /// <summary>
    /// 登录日志展示模型。
    /// </summary>
    public class LoginLogSummary
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string LoginName { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public DateTime LoginTime { get; set; }

        public string ClientInfo { get; set; }

        public string ResultText
        {
            get { return IsSuccess ? "登录成功" : "登录失败"; }
        }

        public string LoginTimeText
        {
            get { return LoginTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public string ClientInfoText
        {
            get { return string.IsNullOrWhiteSpace(ClientInfo) ? "-" : ClientInfo; }
        }

        public string MessageText
        {
            get { return string.IsNullOrWhiteSpace(Message) ? "-" : Message; }
        }
    }
}