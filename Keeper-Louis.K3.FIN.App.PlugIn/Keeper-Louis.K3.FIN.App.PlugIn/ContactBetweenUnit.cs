using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.DynamicForm.PlugIn;
using Kingdee.BOS.Orm.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper_Louis.K3.FIN.App.PlugIn
{
    [Description("费用报销单审核时将申请人的值赋值给往来单位")]
    public class ContactBetweenUnit:AbstractOperationServicePlugIn
    {
        public override void AfterExecuteOperationTransaction(Kingdee.BOS.Core.DynamicForm.PlugIn.Args.AfterExecuteOperationTransaction e)
        {
            base.AfterExecuteOperationTransaction(e);
            if (e.DataEntitys!=null&&e.DataEntitys.Count()>0)
            {
                foreach (DynamicObject item in e.DataEntitys)
                {
                    string formId = item["FFormId"].ToString();
                    if (formId.Equals("ER_ExpReimbursement"))
                    {
                        long ppId = Convert.ToInt64(item["ProposerID_Id"]);
                        string billNo = item["BillNo"].ToString();
                        if (ppId>0)
	{
        string strSql = string.Format(@"/*dialect*/update t_ER_ExpenseReimb set FCONTACTUNIT = {0} where FCONTACTUNITTYPE = 'BD_Empinfo' and fbillno = '{1}'", ppId, billNo);
        DBUtils.Execute(this.Context,strSql);
	}
                    }
                }
            }
        }
    }
}
