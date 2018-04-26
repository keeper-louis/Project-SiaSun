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
    [Description("费用报销模块检查申请人借款余额,数据来源科目余额表和总账凭证")]
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
                    String voucherIds = "";//需要处理的凭证ID
                    //查询法人账簿中自定义字段年份期间FKLYEARPERIOD为null的凭证id,根据FYEAR,FPERIOD组合，为FKLYEARPERIOD赋值
                    string strSql = string.Format(@"/*dialect*/SELECT  FVOUCHERID FROM T_GL_VOUCHER WHERE FACCOUNTBOOKID = 963793 and FINVALID = 0 and FDOCUMENTSTATUS = 'C' AND (FKLYEARPERIOD = NULL OR FKLYEARPERIOD = ' ' OR FKLYEARPERIOD = '')");
                    DynamicObjectCollection WaitDealVoucher = DBUtils.ExecuteDynamicObject(this.Context,strSql);
                    if (WaitDealVoucher!=null&& WaitDealVoucher.Count()>0)
                    {
                        foreach (DynamicObject item in WaitDealVoucher)
                        {
                            voucherIds += ",'" + item["FVOUCHERID"].ToString() + "'";
                        }
                        voucherIds = voucherIds.Substring(1);
                        List<string> updateArray = new List<string>();
                        string updateSql_1 = string.Format(@"/*dialect*/UPDATE T_GL_VOUCHER SET FKLYEARPERIOD = CONCAT(FYEAR,FPERIOD) WHERE FPERIOD>=10 AND FVOUCHERID IN ({0})", voucherIds);
                        updateArray.Add(updateSql_1);
                        string updateSql_2 = string.Format(@"/*dialect*/UPDATE T_GL_VOUCHER SET FKLYEARPERIOD = CONCAT(FYEAR,0,FPERIOD) WHERE FPERIOD<10 AND FVOUCHERID IN ({0})", voucherIds);
                        updateArray.Add(updateSql_2);
                        DBUtils.ExecuteBatch(this.Context, updateArray, 100);
                    }
                    Decimal borrowAmount = this.ShowCheckedBorrowAmountFormLedger(proPoserId);
                    //Decimal borrowAmount = this.ShowCheckedBorrowAmount(proPoserId);
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
        /// 计算申请人借款余额，数据来源：总账凭证、科目余额
        /// </summary>
        /// <param name="proPoserId"></param>
        /// <returns></returns>
        private decimal ShowCheckedBorrowAmountFormLedger(long proPoserId)
        {
            Decimal amountSum = 0;
            //取科目余额表中倒数第二期的合计数
            string amountFirSql = string.Format(@"SELECT sum(FENDBALANCE) ENDBALANCE,FYEARPERIOD
FROM T_GL_Balance
WHERE FACCOUNTID IN (
		SELECT FACCTID
		FROM T_BD_ACCOUNT
		WHERE fnumber LIKE '1221.%'
			OR FNUMBER = '1123.05'
	)
	AND FACCOUNTBOOKID = 963793
	AND FCURRENCYID = 1
	AND FDETAILID IN (
		SELECT fid
		FROM T_BD_FLEXITEMDETAILV
		WHERE FFLEX7 = '{0}'
	)
	AND FYEARPERIOD = (
		SELECT TOP 1 *
		FROM (
			SELECT DISTINCT TOP 2 FYEARPERIOD
			FROM T_GL_Balance
			WHERE FACCOUNTBOOKID = 963793
			ORDER BY FYEARPERIOD DESC
		) mid
		ORDER BY mid.FYEARPERIOD ASC
	)
	GROUP BY FYEARPERIOD", proPoserId);
            DynamicObjectCollection amountFirCollection = DBUtils.ExecuteDynamicObject(this.Context,amountFirSql);
            if (amountFirCollection!=null&&amountFirCollection.Count()>0)
            {
                Decimal amountFir = Convert.ToDecimal(amountFirCollection[0]["ENDBALANCE"]);
                string yearPeriod = amountFirCollection[0]["FYEARPERIOD"].ToString();
                string sysYear = DateTime.Now.Year.ToString();
                string sysMonth = DateTime.Now.Month.ToString();
                string sysYearPeriod = Convert.ToInt32(sysMonth) < 10 ? sysYear + "0" + sysMonth : sysYear + sysMonth;
                //获取总账凭证中借款数据
                string AmountSecSql = string.Format(@"SELECT SUM(vry.FDEBIT) - SUM(vry.FCREDIT) as AmountSec
FROM T_GL_VOUCHER vr
	INNER JOIN T_GL_VOUCHERENTRY vry ON vr.FVOUCHERID = vry.FVOUCHERID
WHERE vry.FACCOUNTID IN (
		SELECT FACCTID
		FROM T_BD_ACCOUNT
		WHERE fnumber LIKE '1221.%'
			OR FNUMBER = '1123.05'
	)
	AND vr.FKLYEARPERIOD>'{0}'
	AND vr.FKLYEARPERIOD<='{1}'
	AND vr.FACCOUNTBOOKID = 963793
	AND vry.FDETAILID IN (
		SELECT fid
		FROM T_BD_FLEXITEMDETAILV
		WHERE FFLEX7 = '{2}'
	)
	AND FINVALID = 0
	AND FDOCUMENTSTATUS = 'C'",yearPeriod, sysYearPeriod, proPoserId);
                DynamicObjectCollection amountSecCollection = DBUtils.ExecuteDynamicObject(this.Context, AmountSecSql);
                if (amountSecCollection!=null&&amountSecCollection.Count()>0)
                {
                    amountSum = amountFir + Convert.ToDecimal(amountSecCollection[0]["AmountSec"]);
                }
            }
            if (amountSum>0)
            {
                return amountSum;
            }
            else
            {
                return 0;
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
