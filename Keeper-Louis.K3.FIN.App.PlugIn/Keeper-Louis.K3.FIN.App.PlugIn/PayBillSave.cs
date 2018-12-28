using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using System.ComponentModel;
using Kingdee.BOS.Orm.DataEntity;

namespace Keeper_Louis.K3.FIN.App.PlugIn
{
    [Description("付款单保存时携带源单明细开户行地址、联行号、收款银行信息到付款单明细")]
    public class PayBillSave:AbstractOperationServicePlugIn
    {
        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            e.FieldKeys.Add("FBankAddr");//开户行地址
            e.FieldKeys.Add("FBankCode");//联行号
            e.FieldKeys.Add("FDueBank");//收款银行        
        }
        public override void BeginOperationTransaction(BeginOperationTransactionArgs e)
        {
            base.BeginOperationTransaction(e);
            if (e.DataEntitys != null && e.DataEntitys.Count() > 0)
            {
                foreach (DynamicObject item in e.DataEntitys)
                {
                    DynamicObjectCollection SRCENTRY = item["PAYBILLSRCENTRY"] as DynamicObjectCollection;
                    DynamicObjectCollection ENTRY = item["PAYBILLENTRY"] as DynamicObjectCollection;
                    if (SRCENTRY != null && SRCENTRY.Count() > 0)
                    {
                        ENTRY[0]["OpenAddressRec"] = Convert.ToString(SRCENTRY[0]["FBankAddr"]) != null && !Convert.ToString(SRCENTRY[0]["FBankAddr"]).Equals(" ") ? Convert.ToString(SRCENTRY[0]["FBankAddr"]) : string.Empty;
                        ENTRY[0]["CNAPS"] = Convert.ToString(SRCENTRY[0]["FBankCode"]) != null && !Convert.ToString(SRCENTRY[0]["FBankCode"]).Equals(" ") ? Convert.ToString(SRCENTRY[0]["FBankCode"]) : string.Empty;
                        ENTRY[0]["BankTypeRec"] = (DynamicObject)SRCENTRY[0]["FDueBank"] != null ? (DynamicObject)SRCENTRY[0]["FDueBank"] : null;
                    }
                }
            }
        }
        
    }
}
