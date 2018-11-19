using Kingdee.BOS.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS;
using Kingdee.BOS.Core;
using Kingdee.BOS.App.Data;
using System.Data;
using System.ComponentModel;

namespace Keeper_Louis.K3.FIN.App.PlugIn
{
    [Description("1小时对出库成本核算单进行一次抓取更新项目号")]
    public class CostUpdateScheduleService : IScheduleService
    {
        public void Run(Context ctx, Schedule schedule)
        {
            //项目号为null，单据类型为出库成本核算单，查询出来distinct源单据号、FID
            string strSql = string.Format(@"/*dialect*/select distinct X.FSRCBILLNO,Z.FID FROM T_HS_AdjustmentBill Z INNER JOIN T_HS_AdjustmentBillEntry X ON Z.FID = X.FID AND Z.FBILLTYPEID = 'd040f688e69b406186aecfb5d9a86101'  AND X.FProjectBillNo = ' ' AND X.FSRCBILLNO <> ' ' AND X.FPROJECTBILLNO <> 0");
            //循环结果集，根据来源单据号查询项目号
            using (IDataReader reader = DBUtils.ExecuteReader(ctx,strSql))
            {
                while (reader.Read())
                {
                    string proSql = string.Format(@"/*dialect*/select distinct a.FProjectBillNo from T_HS_AdjustmentBillEntry a inner join T_HS_AdjustmentBill b on a.FID = b.FID where b.FBILLNO = '{0}'", Convert.ToString(reader["FSRCBILLNO"]));
                    long xmxxID = DBUtils.ExecuteScalar<long>(ctx, proSql, -1, null);
                    //根据来源单据号和FID,对核算单赋值
                    if (xmxxID != -1)
                    {
                        string updateSql = string.Format(@"update T_HS_AdjustmentBillEntry set FProjectBillNo = {0} where FSRCBILLNO = '{1}' and fid = {2}", xmxxID, Convert.ToString(reader["FSRCBILLNO"]), Convert.ToInt64(reader["FID"]));
                        DBUtils.Execute(ctx, updateSql);
                    }
                }
            }
            
        }
    }
}
