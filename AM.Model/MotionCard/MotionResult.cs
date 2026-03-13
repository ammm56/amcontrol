namespace AM.Model.MotionCard
{
    /// <summary>
    /// 统一执行结果模型
    /// </summary>
    public sealed class MotionResult
    {
        public short Code { get; }
        public string Message { get; }
        public bool IsSuccess { get { return Code == (short)MotionErrorCode.Success; } }

        public MotionResult(short code, string message)
        {
            Code = code;
            Message = message ?? string.Empty;
        }

        public static MotionResult Ok(string message = "OK")
        {
            return new MotionResult((short)MotionErrorCode.Success, message);
        }

        public static MotionResult Fail(MotionErrorCode code, string message)
        {
            return new MotionResult((short)code, message);
        }

        public static MotionResult Fail(short code, string message)
        {
            return new MotionResult(code, message);
        }
    }
}