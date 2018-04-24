using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Orm.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper_Louis.K3.FIN.Business.PlugIn
{
    [Description("费用报销模块检查申请人借款余额,从总账取数")]
    public class ER_CheckBorrowAmount:AbstractBillPlugIn
    {
        public override void DataChanged(Kingdee.BOS.Core.DynamicForm.PlugIn.Args.DataChangedEventArgs e)
        {
            base.DataChanged(e);
            string key = e.Field.Key.ToUpperInvariant();//值变化key
            string formID = this.Model.BusinessInfo.GetForm().Id;//单据formID ER_ExpReimbursement-费用报销单;ER_ExpReimbursement_Travel-差旅费报销单
            if (key.Equals("FPROPOSERID"))//申请人
            {
                long proPoserId = Convert.ToInt64(e.NewValue);
                if (proPoserId>0)
                {
                    Decimal borrowAmount = this.ShowCheckedBorrowAmount(proPoserId);
                    this.Model.SetValue("FCheckBorrowAmt", borrowAmount > 0 ? borrowAmount : 0);
                }
                else
                {
                    this.Model.SetValue("FCheckBorrowAmt", 0);
                }
            }
            if (key.Equals("FCHECKBORROWAMT")&&Convert.ToDecimal(this.Model.GetValue("FCheckBorrowAmt"))<0)//借款余额
            {
                this.Model.SetValue("FCheckBorrowAmt", 0);
            }
        }

        /// <summary>
        /// 计算申请人借款余额
        /// 借款余额 = 借款（已付款）金额-已报销金额-已退款金额-冲销金额
        /// 借款（已付款）金额 = 历史借款余额.历史借款余额（本位币）+费用报销付款单.实付金额（本位币）
        /// 已报销金额 = 费用报销单.核定费用金额+差旅费报销单.核定费用金额
        /// 已退款金额 = 费用报销付款退款单.实退金额
        /// 冲销金额 = 借款申请单.冲销金额+出国借款单.冲销金额
        /// </summary>
        /// <param name="proPoserId"></param>
        /// <returns></returns>
        private decimal ShowCheckedBorrowAmount(long proPoserId)
        {
            string strSql = string.Format(@"SELECT sum(result.jkyfk1)+sum(result.jkyfk2) AS jkyfk,
        sum(result.ybxje) AS ybx,
        sum(result.ytkje) AS ytk,
        sum(result.cxje) AS cx
FROM 
    (SELECT sum(FLOCAMOUNT) AS jkyfk1,
        0 AS jkyfk2,
        0 AS ybxje,
        0 AS ytkje,
        0 AS cxje
    FROM T_ER_HISBORROW hw
    INNER JOIN T_ER_HISBORROWENTRY hwy
        ON hw.FID = hwy.FID
    WHERE hw.FDOCUMENTSTATUS = 'C'
            AND hwy.FCONTACTUNITTYPE = 'BD_Empinfo'
            AND hwy.FCONTACTUNIT = {0}
    UNION ALL
    SELECT 0 AS jkyfk1,
        sum(FREALPAYAMOUNTFOR) AS jkyfk2,
        0 AS ybxje,
        0 AS ytkje,
        0 AS cxje
    FROM T_AP_PAYBILL
    WHERE FDOCUMENTSTATUS = 'C'
            AND FBILLTYPEID = '00505694799c955111e32755b2a799d8'
            AND FCONTACTUNITTYPE = 'BD_Empinfo'
            AND FCONTACTUNIT = {0}
    UNION ALL
    SELECT 0 AS jkyfk1,
        0 AS jkyfk2,
        sum(FLOCEXPAMOUNTSUM) AS ybxje,
        0 AS ytkje,
        0 AS cxje
    FROM t_ER_ExpenseReimb
    WHERE FDOCUMENTSTATUS = 'C'
            AND FPROPOSERID = {0}
    UNION ALL
    SELECT 0 AS jkyfk1,
        0 AS jkyfk2,
        0 AS ybxje,
        sum(FREALREFUNDAMOUNT) AS ytkje,
        0 AS cxje
    FROM T_AP_REFUNDBILL
    WHERE FBILLTYPEID = '00505694799c955111e327567f7bac7a'
            AND FDOCUMENTSTATUS = 'C'
            AND FCONTACTUNITTYPE = 'BD_Empinfo'
            AND FCONTACTUNIT = {0}
    UNION ALL
    SELECT 0 AS jkyfk1,
        
        0 AS jkyfk2,
        
        0 AS ybxje,
        
        0 AS ytkje,
        
        sum(ty.FOFFSETAMOUNT) AS cxje
    FROM T_ER_ExpenseRequest tt
    INNER JOIN T_ER_EXPENSEREQUESTENTRY ty
        ON tt.FID = ty.FID
    WHERE tt.FDOCUMENTSTATUS = 'C'
            AND FSTAFFID = {0} ) result", proPoserId);
            DynamicObjectCollection result = DBUtils.ExecuteDynamicObject(this.Context, strSql);
            if (result.Count>0)
	{
        Decimal borrowAmount = Convert.ToDecimal(result[0]["jkyfk"]) - Convert.ToDecimal(result[0]["ybx"]) - Convert.ToDecimal(result[0]["ytk"]) - Convert.ToDecimal(result[0]["cx"]);
        return borrowAmount; 
	}else
	{
                return 0;
	}
            
        }
        
    }
}
