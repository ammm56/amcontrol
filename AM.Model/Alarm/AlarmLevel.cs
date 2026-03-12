using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Alarm
{

    /// <summary>
    /// 报警等级
    /// | 等级       | 示例      |
    /// | -------- | ------- |
    /// | Info     | 参数加载完成  |
    /// | Warning  | 相机帧率下降  |
    /// | Alarm    | 轴跟随误差过大 |
    /// | Critical | 急停触发    |
    /// </summary>
    public enum AlarmLevel
    {
        Info,       //提示
        Warning,    //警告
        Alarm,      //报警
        Critical    //严重报警（设备停机）
    }
}
