using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.App.Data;
using System.Data;

namespace Keeper_Louis.K3.FIN.App.PlugIn
{
    class PurInStkUnchkPlugin: AbstractOperationServicePlugIn
    {
        // Fields
        private string _postUrl = "http://{1}/Orders/SyncReceivingOrder.ashx?ReceivingOrderNo={0}&StockInOrderNo=";
        private string _yct_domain;
        public override void EndOperationTransaction(EndOperationTransactionArgs e)
        {
            base.EndOperationTransaction(e);
            foreach (DynamicObject obj2 in e.DataEntitys)
            {
                int num = Convert.ToInt32(obj2["Id"]);
                Convert.ToString(obj2["BILLNO"]);
                using (DataSet set = DBUtils.ExecuteDataSet(base.Context, string.Format(@"select FBILLNO from T_PUR_RECEIVE where FID in(
select distinct b.FSBILLID from T_STK_INSTOCKENTRY a join T_STK_INSTOCKENTRY_LK b on a.FENTRYID=b.FENTRYID
 where a.FID=151475 and a.FSRCBILLTYPEID='PUR_ReceiveBill') and FBILLNO LIKE 'SH%'")))
                {
                    foreach (DataRow row in set.Tables[0].Rows)
                    {
                        string str;
                        if ((str = YCT_Helper.GetJsonValue(string.Format(this._postUrl, row[0].ToString(), this.GetDomain()))) != YCT_Helper.SuccessValue)
                        {
                            throw new Exception("一采通webapi调用失败：" + str);
                        }

                    }

                }

            }
        }

        private string GetDomain()
        {
            if (string.IsNullOrEmpty(this._yct_domain))
            {
                this._yct_domain = YCT_Helper.GetDomain(base.Context);
            }
            return this._yct_domain;
        }
        
    }
}
