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
    [Description("付款申请单保存时计算订单应付金额汇总，订单已开票金额汇总")]
    public class PaymentReqBill:AbstractOperationServicePlugIn
    {
        public override void EndOperationTransaction(EndOperationTransactionArgs e)
        {
            if (e.DataEntitys!=null&&e.DataEntitys.Count()>0)
            {
                foreach (DynamicObject DataEntity in e.DataEntitys)
                {
                    DynamicObjectCollection PAYAPPLYENTRY = DataEntity["FPAYAPPLYENTRY"] as DynamicObjectCollection;
                    if (PAYAPPLYENTRY!=null&&PAYAPPLYENTRY.Count()>0)
                    {
                        foreach (DynamicObject PayEntity in PAYAPPLYENTRY)
                        {
                            long entryID = Convert.ToInt64(PayEntity["Id"]);
                            if (Convert.ToString(PayEntity["F_PBZS_POONO"]) != null && !Convert.ToString(PayEntity["F_PBZS_POONO"]).Equals(" "))//采购订单号不等null并且不等于空字符串
                            {
                                Double apAmount = CalApAmount(Convert.ToString(PayEntity["F_PBZS_POONO"]));
                                Double invoiceAmount = CalInvoiceAmount(Convert.ToString(PayEntity["F_PBZS_POONO"]));
                                UpdateAmount(entryID, apAmount, invoiceAmount);
                            }
                        }
                    }
                    
                }
            }
        }

        private void UpdateAmount(long entryID, double apAmount, double invoiceAmount)
        {
            string strSql = string.Format(@"/*dialect*/update T_CN_PAYAPPLYENTRY set FAPAMOUNT = {0},FINVOICEAMOUNT={1} where FENTRYID = {2}",apAmount,invoiceAmount,entryID);
            DBUtils.Execute(this.Context,strSql);
        }

        private double CalInvoiceAmount(string orderNo)
        {
            string strSql = string.Format(@"/*dialect*/ select sum(FAFTERTOTALTAXFOR) from T_IV_PURCHASEIC where FSSPORDERNO = '{0}'", orderNo);
            double invoiceAmount = DBUtils.ExecuteScalar<Double>(this.Context, strSql, 0, null);
            return invoiceAmount;
        }

        private double CalApAmount(string orderNo)
        {
            string strSql = string.Format(@"/*dialect*/ select sum(FALLAMOUNTFOR) from T_AP_PAYABLE where FSSPORDERNO = '{0}'", orderNo);
            double apAmount = DBUtils.ExecuteScalar<Double>(this.Context, strSql, 0, null);
            return apAmount;
        }
    }
}
