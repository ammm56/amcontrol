using AM.Model.Common;

namespace AM.Model.MotionCard
{
    /// <summary>
    /// 运动控制统一结果
    /// </summary>
    public class MotionResult : Result
    {
        public static MotionResult Ok(string message = "OK")
        {
            return new MotionResult
            {
                Success = true,
                Code = (short)MotionErrorCode.Success,
                Message = message,
                Source = ResultSource.Motion,
                Time = System.DateTime.Now
            };
        }

        public static MotionResult Fail(MotionErrorCode code, string message)
        {
            return new MotionResult
            {
                Success = false,
                Code = (short)code,
                Message = message,
                Source = ResultSource.Motion,
                Time = System.DateTime.Now
            };
        }

        public static MotionResult Fail(short code, string message)
        {
            return new MotionResult
            {
                Success = false,
                Code = code,
                Message = message,
                Source = ResultSource.Motion,
                Time = System.DateTime.Now
            };
        }
    }
}