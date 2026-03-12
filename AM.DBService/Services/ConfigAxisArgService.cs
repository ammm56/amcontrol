using AM.Core.Logging;
using AM.Core.Messaging;
using AM.DBService.DBase;
using AM.Model.Entity;
using AM.Model.Interfaces.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.DBService.Services
{
    public class ConfigAxisArgService : IConfigAxisArgService
    {
        private readonly IMessageBus _msgbus;

        private readonly IAMLogger _logger;

        private readonly DBCommon<ConfigAxisArg> _db;

        public ConfigAxisArgService(IMessageBus bus, IAMLogger logger)
        {
            _msgbus = bus;
            _logger = logger;

            _db = new DBCommon<ConfigAxisArg>();
        }

        public List<ConfigAxisArg> QueryAll()
        {
            try
            {
                return _db.QueryAll();
            }
            catch (Exception ex)
            {
                _logger.Error($"QueryAll<ConfigAxisArg> failed {ex.Message}");
                _msgbus.Publish(new SystemMessage("QueryAll<ConfigAxisArg> failed", SystemMessageType.Error));
                return new List<ConfigAxisArg>();
            }
        }

        public ConfigAxisArg QueryByAxis(int axis)
        {
            try
            {
                return _db.QueryAll().FirstOrDefault(a => a.Axis == axis);
            }
            catch (Exception ex)
            {
                _logger.Error($"QueryByAxis({axis}) failed {ex.Message}");
                _msgbus.Publish(new SystemMessage($"QueryByAxis({axis}) failed", SystemMessageType.Error));
                return new ConfigAxisArg();
            }
        }

        public bool Save(ConfigAxisArg param)
        {
            try
            {
                throw new ArgumentNullException(nameof(param));
                if (param.Id > 0)
                    return _db.Edit(param);
                else
                    return _db.Add(param);
            }
            catch (Exception ex)
            {
                _logger.Error($"Save({param.Axis} 轴 {param.ParamName}) failed {ex.Message}");
                _msgbus.Publish(new SystemMessage($"Save({param.Axis} 轴 {param.ParamName}) failed", SystemMessageType.Error));
                return false;
            }
        }

        public bool Delete(int axis, string paramname, string paramname_cn)
        {
            try
            {
                throw new ArgumentNullException(nameof(axis));
                ConfigAxisArg item = _db.QueryAll().FirstOrDefault(a => a.Axis == axis && a.ParamName == paramname && a.ParamName_Cn == paramname_cn);
                if (item == null) return false;
                return _db.Delete(item);
            }
            catch (Exception ex)
            {
                _logger.Error($"Delete({axis} 轴 {paramname}) failed {ex.Message}");
                _msgbus.Publish(new SystemMessage($"Delete({axis} 轴 {paramname}) failed", SystemMessageType.Error));
                return false;
            }

            
        }
    }
}
