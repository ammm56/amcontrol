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
    /// </summary>
    public class RuntimeTaskManager : ServiceBase, IRuntimeTaskManager
    {
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
            _workers = new Dictionary<string, IRuntimeWorker>(StringComparer.OrdinalIgnoreCase);
        }

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

            if (_workers.ContainsKey(worker.Name))
            {
                return Fail(-3003, "后台工作单元名称重复: " + worker.Name);
            }

            _workers[worker.Name] = worker;
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

            // 自动启动：失败时仅 Warn，不阻断启动流程
            var startResult = worker.Start();
            if (!startResult.Success)
            {
                _reporter?.Warn(
                    MessageSourceName,
                    string.Format("后台工作单元自动启动失败: {0}，{1}", worker.Name, startResult.Message),
                    startResult.Code);
            }

            // 无论启动是否成功，注册本身已完成，返回注册结果
            return regResult;
        }

        public Result<IRuntimeWorker> QueryAll()
        {
            return OkList(_workers.Values.ToList(), "后台工作单元查询成功");
        }

        public Result StartAll()
        {
            foreach (var worker in _workers.Values)
            {
                var result = worker.Start();
                if (!result.Success)
                {
                    return Fail(result.Code, "启动后台工作单元失败: " + worker.Name + "，" + result.Message);
                }
            }

            return Ok("后台工作单元全部启动成功");
        }

        public async Task<Result> StopAllAsync()
        {
            foreach (var worker in _workers.Values.Reverse().ToList())
            {
                var result = await worker.StopAsync();
                if (!result.Success)
                {
                    return Fail(result.Code, "停止后台工作单元失败: " + worker.Name + "，" + result.Message);
                }
            }

            return Ok("后台工作单元全部停止成功");
        }

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

        public async Task<Result> StopAsync(string name)
        {
            IRuntimeWorker worker;
            if (!TryGetWorker(name, out worker))
            {
                return Fail(-3004, "未找到后台工作单元: " + name);
            }

            var result = await worker.StopAsync();
            if (!result.Success)
            {
                return Fail(result.Code, "停止后台工作单元失败: " + worker.Name + "，" + result.Message);
            }

            return Ok("后台工作单元停止成功: " + worker.Name);
        }

        private bool TryGetWorker(string name, out IRuntimeWorker worker)
        {
            worker = null;

            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            return _workers.TryGetValue(name.Trim(), out worker);
        }
    }
}