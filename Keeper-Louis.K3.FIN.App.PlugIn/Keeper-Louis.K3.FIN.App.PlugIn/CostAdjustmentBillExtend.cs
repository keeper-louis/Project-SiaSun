using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.App.Data;

namespace Keeper_Louis.K3.FIN.App.PlugIn
{
    [Description("出库成本核算，由于应付和发票有差额，生成成本调整单携带项目信息到分录")]
    public class CostAdjustmentBillExtend: AbstractOperationServicePlugIn
    {
        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            e.FieldKeys.Add("FSRCBILLNO");
        }

        public override void AfterExecuteOperationTransaction(AfterExecuteOperationTransaction e)
        {
            if (e.DataEntitys!=null&&e.DataEntitys.Count<DynamicObject>()>0)
            {
                foreach (DynamicObject item in e.DataEntitys)
                {
                    long biiID = Convert.ToInt64(item["Id"]);
                    DynamicObjectCollection entityCollection = item["HS_AdjustmentBillEntry"] as DynamicObjectCollection;
                    foreach (DynamicObject entity in entityCollection)
                    {
                        string billNo = Convert.ToString(entity["SrcBillNo"]);
                        string strSql = string.Format(@"/*dialect*/select F_PAEZ_XMXX from t_STK_InStock where fbillno = '{0}'", billNo);
                        long xmxxID = DBUtils.ExecuteScalar<long>(this.Context, strSql, -1, null);
                        if (xmxxID!=-1)
                        {
                            string updateSql = string.Format(@"update T_HS_AdjustmentBillEntry set FProjectBillNo = {0} where FSRCBILLNO = '{1}' and fid = {2}",xmxxID,billNo,biiID);
                            DBUtils.Execute(this.Context, updateSql);
                        }
                    }
                }
            }
        }
    }
}
