using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.DBService.Services.Runtime
{
    /// <summary>
    /// 统一后台任务宿主管理器。
    /// 负责后台工作单元的注册、启动、停止与状态查询。
    /// 
    /// 最小补充说明：
    /// 1. 增加 `_syncRoot`，保证 `_workers` 的并发读写安全；
    /// 2. 对外接口保持不变，不影响现有调用层；
    /// 3. 仍只管理顶层 worker，不下沉管理 PLC 站 runner / 控制卡 runner。
    /// </summary>
    public class RuntimeTaskManager : ServiceBase, IRuntimeTaskManager
    {
        /// <summary>
        /// 工作单元集合访问同步锁。
        /// </summary>
        private readonly object _syncRoot;

        /// <summary>
        /// 已注册后台工作单元集合。
        /// Key 为工作单元名称。
        /// </summary>
        private readonly IDictionary<string, IRuntimeWorker> _workers;

        protected override string MessageSourceName
        {
            get { return "RuntimeTaskManager"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        public RuntimeTaskManager()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public RuntimeTaskManager(IAppReporter reporter)
            : base(reporter)
        {
            _syncRoot = new object();
            _workers = new Dictionary<string, IRuntimeWorker>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 注册后台工作单元。
        /// </summary>
        public Result Register(IRuntimeWorker worker)
        {
            if (worker == null)
            {
                return Fail(-3001, "后台工作单元不能为空");
            }

            if (string.IsNullOrWhiteSpace(worker.Name))
            {
                return Fail(-3002, "后台工作单元名称不能为空");
            }

            lock (_syncRoot)
            {
                if (_workers.ContainsKey(worker.Name))
                {
                    return Fail(-3003, "后台工作单元名称重复: " + worker.Name);
                }

                _workers[worker.Name] = worker;
            }

            return OkLogOnly("后台工作单元注册成功: " + worker.Name);
        }

        /// <summary>
        /// 注册后台工作单元，并根据 autoStart 决定是否立即启动。
        /// 注册失败 → Fail（上层应中止）。
        /// 启动失败 → 内部 Warn，注册结果仍返回成功（非致命，可后续手动启动）。
        /// </summary>
        public Result Register(IRuntimeWorker worker, bool autoStart)
        {
            var regResult = Register(worker);
            if (!regResult.Success || !autoStart)
            {
                return regResult;
            }

            var startResult = worker.Start();
            if (!startResult.Success)
            {
                _reporter?.Warn(
                    MessageSourceName,
                    string.Format("后台工作单元自动启动失败: {0}，{1}", worker.Name, startResult.Message),
                    startResult.Code);
            }

            return regResult;
        }

        /// <summary>
        /// 查询全部已注册后台工作单元。
        /// 返回的是当前快照副本，避免外部枚举时与内部写操作冲突。
        /// </summary>
        public Result<IRuntimeWorker> QueryAll()
        {
            List<IRuntimeWorker> snapshot;
            lock (_syncRoot)
            {
                snapshot = _workers.Values.ToList();
            }

            return OkList(snapshot, "后台工作单元查询成功");
        }

        /// <summary>
        /// 启动全部后台工作单元。
        /// </summary>
        public Result StartAll()
        {
            List<IRuntimeWorker> snapshot;
            lock (_syncRoot)
            {
                snapshot = _workers.Values.ToList();
            }

            foreach (var worker in snapshot)
            {
                var result = worker.Start();
                if (!result.Success)
                {
                    return Fail(result.Code, "启动后台工作单元失败: " + worker.Name + "，" + result.Message);
                }
            }

            return Ok("后台工作单元全部启动成功");
        }

        /// <summary>
        /// 停止全部后台工作单元。
        /// 按注册顺序逆序停止，便于资源依赖释放。
        /// </summary>
        public async Task<Result> StopAllAsync()
        {
            List<IRuntimeWorker> snapshot;
            lock (_syncRoot)
            {
                snapshot = _workers.Values.Reverse().ToList();
            }

            foreach (var worker in snapshot)
            {
                var result = await worker.StopAsync().ConfigureAwait(false);
                if (!result.Success)
                {
                    return Fail(result.Code, "停止后台工作单元失败: " + worker.Name + "，" + result.Message);
                }
            }

            return Ok("后台工作单元全部停止成功");
        }

        /// <summary>
        /// 按名称启动指定后台工作单元。
        /// </summary>
        public Result Start(string name)
        {
            IRuntimeWorker worker;
            if (!TryGetWorker(name, out worker))
            {
                return Fail(-3004, "未找到后台工作单元: " + name);
            }

            var result = worker.Start();
            if (!result.Success)
            {
                return Fail(result.Code, "启动后台工作单元失败: " + worker.Name + "，" + result.Message);
            }

            return Ok("后台工作单元启动成功: " + worker.Name);
        }

        /// <summary>
        /// 按名称停止指定后台工作单元。
        /// </summary>
        public async Task<Result> StopAsync(string name)
        {
            IRuntimeWorker worker;
            if (!TryGetWorker(name, out worker))
            {
                return Fail(-3004, "未找到后台工作单元: " + name);
            }

            var result = await worker.StopAsync().ConfigureAwait(false);
            if (!result.Success)
            {
                return Fail(result.Code, "停止后台工作单元失败: " + worker.Name + "，" + result.Message);
            }

            return Ok("后台工作单元停止成功: " + worker.Name);
        }

        /// <summary>
        /// 按名称获取后台工作单元。
        /// </summary>
        private bool TryGetWorker(string name, out IRuntimeWorker worker)
        {
            worker = null;

            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            lock (_syncRoot)
            {
                return _workers.TryGetValue(name.Trim(), out worker);
            }
        }
    }
}