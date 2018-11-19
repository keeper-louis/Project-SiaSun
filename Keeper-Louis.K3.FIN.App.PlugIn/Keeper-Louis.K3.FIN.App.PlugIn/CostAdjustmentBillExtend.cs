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
using System.Data;
using Kingdee.BOS.Log;

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
                    if (Convert.ToString(item["BillTYpe_Id"]).Equals("4d20243e32b14834aae8a96525354bcd"))//入库成本调整单
                    {
                        foreach (DynamicObject entity in entityCollection)
                        {
                            long xmxxID = -1;
                            long PURCHASERID = -1;
                            string billNo = Convert.ToString(entity["SrcBillNo"]);
                            string strSql = string.Format(@"/*dialect*/select F_PAEZ_XMXX,FPURCHASERID from t_STK_InStock where fbillno = '{0}'", billNo);
                            using (IDataReader reader = DBUtils.ExecuteReader(this.Context, strSql))
                            {
                                while (reader.Read())
                                {
                                    xmxxID = Convert.ToInt64(reader["F_PAEZ_XMXX"]);
                                    PURCHASERID = Convert.ToInt64(reader["FPURCHASERID"]);
                                }
                            }
                            if (xmxxID != -1)
                            {
                                string updateSql = string.Format(@"update T_HS_AdjustmentBillEntry set FProjectBillNo = {0} where FSRCBILLNO = '{1}' and fid = {2}", xmxxID, billNo, biiID);
                                DBUtils.Execute(this.Context, updateSql);
                            }
                            if (PURCHASERID!=-1)
                            {
                                string updateSql = string.Format(@"update T_HS_AdjustmentBillEntry set F_Detail_PURCHASERID = {0} where FSRCBILLNO = '{1}' and fid = {2}", PURCHASERID, billNo, biiID);
                                DBUtils.Execute(this.Context, updateSql);
                            }
                        }
                    }
                    //if (Convert.ToString(item["BillTYpe_Id"]).Equals("d040f688e69b406186aecfb5d9a86101"))//出库成本调整单
                    //{
                    //    foreach (DynamicObject entity in entityCollection)
                    //    {
                    //        string billNo = Convert.ToString(entity["SrcBillNo"]);
                            
                    //        string strSql = string.Format(@"/*dialect*/select distinct a.FProjectBillNo from T_HS_AdjustmentBillEntry a inner join T_HS_AdjustmentBill b on a.FID = b.FID where b.FBILLNO = '{0}'", billNo);
                    //        long xmxxID = DBUtils.ExecuteScalar<long>(this.Context, strSql, -1, null);
                    //        if (xmxxID != -1)
                    //        {
                    //            string updateSql = string.Format(@"update T_HS_AdjustmentBillEntry set FProjectBillNo = {0} where FSRCBILLNO = '{1}' and fid = {2}", xmxxID, billNo, biiID);
                    //            DBUtils.Execute(this.Context, updateSql);
                    //        }
                    //    }
                    //}
                    
                }
            }
        }
    }
}
