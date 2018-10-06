using Keeper.K3.MES.Entity;
using Kingdee.BOS;
using Kingdee.BOS.Core.DynamicForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.K3.MES.Contracts
{
    /// <summary>
    /// 服务契约
    /// </summary>
    public interface ICommonService
    {
        List<MaterialEntity> GetMateriaDatalList(Context ctx,MaterialEntity material);

        IOperationResult CreateSimpleProBill(Context ctx,SimpleProBill simpleProBill);
    }
}
