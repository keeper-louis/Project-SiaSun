using Kingdee.BOS.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS;
using Kingdee.BOS.Core;
using Kingdee.BOS.Util;
using Kingdee.BOS.Orm.DataEntity;

namespace Keeper_Louis.K3.FIN.App.PlugIn.Validator
{
    public class PaybillSubmitBankValidator : AbstractValidator
    {
        public override void Validate(ExtendedDataEntity[] dataEntities, ValidateContext validateContext, Context ctx)
        {
            if (dataEntities.IsNullOrEmpty() || dataEntities.Length == 0)
            {
                return;
            }
            foreach (ExtendedDataEntity item in dataEntities)
            {
                DynamicObject requestDynamic = item.DataEntity;
                if (Convert.ToInt32(requestDynamic["F_PAEZ_SFYQZF"]) == 0)
                {

                    string msg = string.Format("单据：{0},未启用银企支付参数，不可通过银企进行付款", requestDynamic["BillNo"]);
                    var errInfo = new ValidationErrorInfo(
                                    item.BillNo,
                                    item.DataEntity["Id"].ToString(),
                                    item.DataEntityIndex,
                                    item.RowIndex,
                                    "Valid019",
                                    msg,
                                    " ",
                                    Kingdee.BOS.Core.Validation.ErrorLevel.Error);
                    validateContext.AddError(item.DataEntity, errInfo);
                }
            }
        }
    }

}

