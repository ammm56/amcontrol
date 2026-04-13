using AM.Core.Base;
using AM.Core.Context;
using AM.Model.Common;
using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using AM.Model.Structs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AM.MotionService.Hub
{
    /// <summary>
    /// 运动控制统一调度入口。
    /// 负责根据逻辑轴、逻辑 IO 位将请求路由到对应的控制卡服务实例。
    /// </summary>
    public class MotionServiceHub : ServiceBase, IMotionCardService
    {
        protected override string MessageSourceName
        {
            get { return "MotionHub"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        /// <summary>
        /// Hub 不直接保存轴映射，该方法仅用于兼容统一接口。
        /// </summary>
        public void LoadAxisConfig(List<AxisConfig> configs)
        {
        }

        private IEnumerable<IMotionCardService> GetAllCardServices()
        {
            return MachineContext.Instance.MotionCards.Values;
        }

        private Result<IMotionCardService> ResolveFromMap(
            IDictionary<short, IMotionCardService> map,
            short key,
            MotionErrorCode errorCode,
            string successMessage,
            string failMessage)
        {
            IMotionCardService service;
            if (map.TryGetValue(key, out service))
            {
                return Result<IMotionCardService>.OkItem(service, successMessage, ResultSource.Motion);
            }

            return Result<IMotionCardService>.Fail((int)errorCode, failMessage, ResultSource.Motion);
        }

        private Result<IMotionCardService> ResolveAxisService(short logicalAxis)
        {
            return ResolveFromMap(
                MachineContext.Instance.AxisMotionCards,
                logicalAxis,
                MotionErrorCode.AxisMapNotFound,
                "逻辑轴所属控制卡解析成功",
                "逻辑轴 " + logicalAxis + " 未找到所属控制卡");
        }

        private Result<IMotionCardService> ResolveDIService(short bit)
        {
            return ResolveFromMap(
                MachineContext.Instance.DICards,
                bit,
                MotionErrorCode.IoMapNotFound,
                "逻辑DI所属控制卡解析成功",
                "逻辑DI位 " + bit + " 未找到所属控制卡");
        }

        private Result<IMotionCardService> ResolveDOService(short bit)
        {
            return ResolveFromMap(
                MachineContext.Instance.DOCards,
                bit,
                MotionErrorCode.IoMapNotFound,
                "逻辑DO所属控制卡解析成功",
                "逻辑DO位 " + bit + " 未找到所属控制卡");
        }

        private Result ExecuteOnAllCards(Func<IMotionCardService, Result> action, string successMessage)
        {
            foreach (var service in GetAllCardServices())
            {
                var result = action(service);
                if (!result.Success)
                    return result;
            }

            return Ok(successMessage);
        }

        private Result ExecuteOnAxis(short logicalAxis, Func<IMotionCardService, Result> action)
        {
            var route = ResolveAxisService(logicalAxis);
            if (!route.Success)
                return Result.Fail(route.Code, route.Message, ResultSource.Motion);

            return action(route.Item);
        }

        private Result<T> QueryOnAxis<T>(short logicalAxis, Func<IMotionCardService, Result<T>> action)
        {
            var route = ResolveAxisService(logicalAxis);
            if (!route.Success)
                return Result<T>.Fail(route.Code, route.Message, ResultSource.Motion);

            return action(route.Item);
        }

        private Result ExecuteOnDO(short bit, Func<IMotionCardService, Result> action)
        {
            var route = ResolveDOService(bit);
            if (!route.Success)
                return Result.Fail(route.Code, route.Message, ResultSource.Motion);

            return action(route.Item);
        }

        private Result<T> QueryOnDI<T>(short bit, Func<IMotionCardService, Result<T>> action)
        {
            var route = ResolveDIService(bit);
            if (!route.Success)
                return Result<T>.Fail(route.Code, route.Message, ResultSource.Motion);

            return action(route.Item);
        }

        private Result<T> QueryOnDO<T>(short bit, Func<IMotionCardService, Result<T>> action)
        {
            var route = ResolveDOService(bit);
            if (!route.Success)
                return Result<T>.Fail(route.Code, route.Message, ResultSource.Motion);

            return action(route.Item);
        }

        public Result Initialize(string configPath)
        {
            return ExecuteOnAllCards(
                service => service.Initialize(configPath),
                "所有运动控制卡初始化成功");
        }

        public Result Connect()
        {
            return ExecuteOnAllCards(
                service => service.Connect(),
                "所有运动控制卡连接成功");
        }

        public Result Disconnect()
        {
            return ExecuteOnAllCards(
                service => service.Disconnect(),
                "所有运动控制卡断开成功");
        }

        public Result<bool> IsConnected()
        {
            var services = new List<IMotionCardService>();

            foreach (var service in GetAllCardServices())
            {
                if (service == null)
                {
                    continue;
                }

                if (!services.Contains(service))
                {
                    services.Add(service);
                }
            }

            if (services.Count <= 0)
            {
                return OkSilent(false, "当前未配置运动控制卡");
            }

            foreach (var service in services)
            {
                var result = service.IsConnected();
                if (result == null)
                {
                    return Result<bool>.Fail((int)MotionErrorCode.CardConnectFailed, "控制卡连接状态查询失败", ResultSource.Motion);
                }

                if (!result.Success)
                {
                    return Result<bool>.Fail(result.Code, result.Message, ResultSource.Motion);
                }

                if (!result.Item)
                {
                    return OkSilent(false, "存在未连接的运动控制卡");
                }
            }

            return OkSilent(true, "所有运动控制卡已连接");
        }

        public Result ClearStatus(short logicalAxis)
        {
            return ExecuteOnAxis(logicalAxis, service => service.ClearStatus(logicalAxis));
        }

        public Result ClearAllAxisStatus()
        {
            return ExecuteOnAllCards(
                service => service.ClearAllAxisStatus(),
                "所有控制卡全轴状态清除成功");
        }

        public Result SetZeroPos(short logicalAxis)
        {
            return ExecuteOnAxis(logicalAxis, service => service.SetZeroPos(logicalAxis));
        }

        public Result SetAllZeroPos()
        {
            return ExecuteOnAllCards(
                service => service.SetAllZeroPos(),
                "所有控制卡全轴位置清零成功");
        }

        public Result ConfigAxisHardware(AxisConfig cfg)
        {
            if (cfg == null)
                return Fail((int)MotionErrorCode.InvalidAxisConfig, "轴配置不能为空");

            return ExecuteOnAxis(cfg.LogicalAxis, service => service.ConfigAxisHardware(cfg));
        }

        public Result Enable(short logicalAxis, bool onOff)
        {
            return ExecuteOnAxis(logicalAxis, service => service.Enable(logicalAxis, onOff));
        }

        public Result Stop(short logicalAxis, bool isEmergency = false)
        {
            return ExecuteOnAxis(logicalAxis, service => service.Stop(logicalAxis, isEmergency));
        }

        public Result StopAll(bool isEmergency = false)
        {
            return ExecuteOnAllCards(
                service => service.StopAll(isEmergency),
                "所有控制卡停止成功");
        }

        public Task<Result> HomeAsync(short logicalAxis)
        {
            var route = ResolveAxisService(logicalAxis);
            if (!route.Success)
                return Task.FromResult(Result.Fail(route.Code, route.Message, ResultSource.Motion));

            return route.Item.HomeAsync(logicalAxis);
        }

        public Result Home(short logicalAxis)
        {
            return ExecuteOnAxis(logicalAxis, service => service.Home(logicalAxis));
        }

        public Result MoveRelativeMm(short logicalAxis, double distanceMm, double velMm)
        {
            return ExecuteOnAxis(logicalAxis, service => service.MoveRelativeMm(logicalAxis, distanceMm, velMm));
        }

        public Result MoveRelative(short logicalAxis, double pulse, double velocity, double acc, double dec)
        {
            return ExecuteOnAxis(logicalAxis, service => service.MoveRelative(logicalAxis, pulse, velocity, acc, dec));
        }

        public Result MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm)
        {
            return ExecuteOnAxis(logicalAxis, service => service.MoveAbsoluteMm(logicalAxis, positionMm, velMm));
        }

        public Result MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec)
        {
            return ExecuteOnAxis(logicalAxis, service => service.MoveAbsolute(logicalAxis, position, velocity, acc, dec));
        }

        public Result JogMove(short logicalAxis, int direction, double velocity)
        {
            return ExecuteOnAxis(logicalAxis, service => service.JogMove(logicalAxis, direction, velocity));
        }

        public Result JogMoveMm(short logicalAxis, bool direction, double velMm)
        {
            return ExecuteOnAxis(logicalAxis, service => service.JogMoveMm(logicalAxis, direction, velMm));
        }

        public Result JogStop(short logicalAxis)
        {
            return ExecuteOnAxis(logicalAxis, service => service.JogStop(logicalAxis));
        }

        public Result SetDO(short bit, bool status)
        {
            return ExecuteOnDO(bit, service => service.SetDO(bit, status));
        }

        public Result<bool> GetDI(short bit)
        {
            return QueryOnDI(bit, service => service.GetDI(bit));
        }

        public Result<bool> GetDO(short bit)
        {
            return QueryOnDO(bit, service => service.GetDO(bit));
        }

        public Result SetVel(short logicalAxis, double vel)
        {
            return ExecuteOnAxis(logicalAxis, service => service.SetVel(logicalAxis, vel));
        }

        public Result SetAcc(short logicalAxis, double acc)
        {
            return ExecuteOnAxis(logicalAxis, service => service.SetAcc(logicalAxis, acc));
        }

        public Result SetDec(short logicalAxis, double dec)
        {
            return ExecuteOnAxis(logicalAxis, service => service.SetDec(logicalAxis, dec));
        }

        public Result<AxisStatus> GetAxisStatus(short logicalAxis)
        {
            return QueryOnAxis(logicalAxis, service => service.GetAxisStatus(logicalAxis));
        }

        public Result<double> GetCommandPosition(short logicalAxis)
        {
            return QueryOnAxis(logicalAxis, service => service.GetCommandPosition(logicalAxis));
        }

        public Result<double> GetEncoderPosition(short logicalAxis)
        {
            return QueryOnAxis(logicalAxis, service => service.GetEncoderPosition(logicalAxis));
        }

        public Result<double> GetCommandPositionMm(short logicalAxis)
        {
            return QueryOnAxis(logicalAxis, service => service.GetCommandPositionMm(logicalAxis));
        }

        public Result<double> GetEncoderPositionMm(short logicalAxis)
        {
            return QueryOnAxis(logicalAxis, service => service.GetEncoderPositionMm(logicalAxis));
        }

        public Result<bool> IsMoving(short logicalAxis)
        {
            return QueryOnAxis(logicalAxis, service => service.IsMoving(logicalAxis));
        }
    }
}