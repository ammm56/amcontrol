namespace AM.Model.Common
{
    /// <summary>
    /// 数据库统一错误码
    /// </summary>
    public enum DbErrorCode
    {
        Success = 0,

        Unknown = -3000,
        InvalidArgument = -3001,
        QueryFailed = -3002,
        SaveFailed = -3003,
        DeleteFailed = -3004,
        NotFound = -3005
    }
}