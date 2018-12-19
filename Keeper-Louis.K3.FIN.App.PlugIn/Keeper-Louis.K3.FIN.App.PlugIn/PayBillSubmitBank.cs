using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Keeper_Louis.K3.FIN.App.PlugIn.Validator;

namespace Keeper_Louis.K3.FIN.App.PlugIn
{
    [Description("提交银行操作校验")]
    public class PayBillSubmitBank:AbstractOperationServicePlugIn
    {
        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            base.OnPreparePropertys(e);
            e.FieldKeys.Add("F_PAEZ_SFYQZF");
        }
        public override void OnAddValidators(AddValidatorsEventArgs e)
        {
            PaybillSubmitBankValidator submitBankValidator = new PaybillSubmitBankValidator();
            submitBankValidator.EntityKey = "FBillHead";
            e.Validators.Add(submitBankValidator);
        }
    }
}
