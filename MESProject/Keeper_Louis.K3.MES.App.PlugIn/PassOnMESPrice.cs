using Kingdee.BOS.Core.Bill.PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.Metadata.ControlElement;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.App.Data;

namespace Keeper_Louis.K3.MES.App.PlugIn
{
    [Description("动态传递领料平均价给MES")]
    public class PassOnMESPrice: AbstractBillPlugIn
    {
        /// <summary>
        /// 点击按钮传递平均单价
        /// </summary>
        /// <param name="e"></param>
        public override void AfterButtonClick(AfterButtonClickEventArgs e)
        {
            base.AfterButtonClick(e);
            if (e.Key.ToUpperInvariant().Equals("FPASSONMEDPRICE"))
            {
                string strSql = string.Format(@"SELECT MAT.FNUMBER,
         sum(spData.FAMOUNT)/sum(spData.FACTUALQTY) avgPrice
FROM T_SP_PICKMTRL sp
INNER JOIN T_SP_PICKMTRLDATA spData
    ON sp.FID = spData.FID
INNER JOIN T_BD_MATERIAL MAT
    ON spData.FMATERIALID = MAT.FMATERIALID
WHERE sp.FPRDORGID = 111934 
AND sp.FDATE>=convert(char,dateadd(dd,-day(getdate()),getdate()),23)
AND sp.FDATE<=convert(char,dateadd(dd,-day(dateadd(month,-1,getdate()))+1,dateadd(month,-1,getdate())),23)
group by MAT.FNUMBER");
                DBUtils.Execute(this.Context,strSql);
            }
                
        }
    }
}
