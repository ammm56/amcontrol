using AM.Model.Common;
using AM.Model.Entity;

namespace AM.Model.Interfaces.DB
{
    public interface IConfigAxisArgService
    {
        Result<ConfigAxisArg> QueryAll();
        Result<ConfigAxisArg> QueryByAxis(int axis);
        Result Save(ConfigAxisArg param);
        Result Delete(int axis, string paramname, string paramname_cn);
    }
}
