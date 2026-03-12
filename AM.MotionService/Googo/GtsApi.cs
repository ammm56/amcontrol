using System;
using GTN;

namespace AM.MotionCard.Googo
{
    // 对 GTN.gts.cs 中原始 P/Invoke "mc" 静态类的简单封装
    // 目的：为 GoogoMotionCard 提供一个更窄、可测试且可注入的抽象层。
    public interface IGtsApi
    {
        short Open(short cardId, short param);
        short Close();
        short Reset(short core);
        short ExtModuleInit(short core, short method);
        short ClrSts(short core, short axis, short count);
        short ZeroPos(short core, short axis, short count);
        short AxisOff(short core, short axis);
        short AxisOn(short axis);
        short AlarmOn(short core, short axis);
        short AlarmOff(short core, short axis);
        short SetSense(short core, short type, short index, short value);
        short StepDir(short core, short step);
        short StepPulse(short core, short step);
        short EncOn(short core, short encoder);
        short EncOff(short core, short encoder);
        short LmtsOn(short core, short axis, short limitType);
        short LmtsOff(short core, short axis, short limitType);
        short Stop(short core, Int32 mask, Int32 option);

        short PrfTrap(short core, short profile);
        short GetTrapPrm(short core, short profile, out mc.TTrapPrm prm);
        short SetTrapPrm(short core, short profile, ref mc.TTrapPrm prm);
        short SetVel(short core, short profile, double vel);
        short GetPrfPos(short core, short profile, out double pValue, short count, out UInt32 pClock);
        short SetPos(short core, short profile, Int32 pos);
        short Update(short core, Int32 mask);

        short PrfJog(short core, short profile);
        short GetJogPrm(short core, short profile, out mc.TJogPrm prm);
        short SetJogPrm(short core, short profile, ref mc.TJogPrm prm);
    }

    public class GtsApi : IGtsApi
    {
        public short Open(short cardId, short param) => mc.GTN_Open(cardId, param);
        public short Close() => mc.GTN_Close();
        public short Reset(short core) => mc.GTN_Reset(core);
        public short ExtModuleInit(short core, short method) => mc.GTN_ExtModuleInit(core, method);
        public short ClrSts(short core, short axis, short count) => mc.GTN_ClrSts(core, axis, count);
        public short ZeroPos(short core, short axis, short count) => mc.GTN_ZeroPos(core, axis, count);
        public short AxisOff(short core, short axis) => mc.GTN_AxisOff(core, axis);
        public short AxisOn(short axis) => mc.GT_AxisOn(axis);
        public short AlarmOn(short core, short axis) => mc.GTN_AlarmOn(core, axis);
        public short AlarmOff(short core, short axis) => mc.GTN_AlarmOff(core, axis);
        public short SetSense(short core, short type, short index, short value) => mc.GTN_SetSense(core, type, index, value);
        public short StepDir(short core, short step) => mc.GTN_StepDir(core, step);
        public short StepPulse(short core, short step) => mc.GTN_StepPulse(core, step);
        public short EncOn(short core, short encoder) => mc.GTN_EncOn(core, encoder);
        public short EncOff(short core, short encoder) => mc.GTN_EncOff(core, encoder);
        public short LmtsOn(short core, short axis, short limitType) => mc.GTN_LmtsOn(core, axis, limitType);
        public short LmtsOff(short core, short axis, short limitType) => mc.GTN_LmtsOff(core, axis, limitType);
        public short Stop(short core, Int32 mask, Int32 option) => mc.GTN_Stop(core, mask, option);

        public short PrfTrap(short core, short profile) => mc.GTN_PrfTrap(core, profile);
        public short GetTrapPrm(short core, short profile, out mc.TTrapPrm prm) => mc.GTN_GetTrapPrm(core, profile, out prm);
        public short SetTrapPrm(short core, short profile, ref mc.TTrapPrm prm) => mc.GTN_SetTrapPrm(core, profile, ref prm);
        public short SetVel(short core, short profile, double vel) => mc.GTN_SetVel(core, profile, vel);
        public short GetPrfPos(short core, short profile, out double pValue, short count, out UInt32 pClock) => mc.GTN_GetPrfPos(core, profile, out pValue, count, out pClock);
        public short SetPos(short core, short profile, Int32 pos) => mc.GTN_SetPos(core, profile, pos);
        public short Update(short core, Int32 mask) => mc.GTN_Update(core, mask);

        public short PrfJog(short core, short profile) => mc.GTN_PrfJog(core, profile);
        public short GetJogPrm(short core, short profile, out mc.TJogPrm prm) => mc.GTN_GetJogPrm(core, profile, out prm);
        public short SetJogPrm(short core, short profile, ref mc.TJogPrm prm) => mc.GTN_SetJogPrm(core, profile, ref prm);
    }
}
