using Kingdee.BOS.Core.List.PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.List;
using Kingdee.BOS.Core.SqlBuilder;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.ServiceHelper;

namespace Keeper_Louis.K3.FIN.Business.PlugIn
{
    [Description("付款单列表界面")]
    public class PayBillListUI: AbstractListPlugIn
    {
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            base.BarItemClick(e);
            if (e.BarItemKey.Equals("tbSubmitBank"))
            {
                QueryBuilderParemeter queryParam = new QueryBuilderParemeter();
                queryParam.FormId = this.View.BillBusinessInfo.GetForm().Id;
                queryParam.BusinessInfo = this.View.BillBusinessInfo;
                queryParam.SelectItems.Add(new SelectorItemInfo("F_PAEZ_SFYQZF"));
                List<long> billIds = (from p in this.ListView.SelectedRowsInfo
                                      select Convert.ToInt64(p.PrimaryKeyValue)).ToList();
                queryParam.FilterClauseWihtKey = string.Format(" {0} IN ( {1} ) ",
                    this.ListView.BillBusinessInfo.GetForm().PkFieldName,
                    string.Join(",", billIds));
                var rows = QueryServiceHelper.GetDynamicObjectCollection(this.Context, queryParam);
                List<Boolean> result = (from z in rows.Where(z => !Convert.ToBoolean(z["F_PAEZ_SFYQZF"]))
                                        select Convert.ToBoolean(z["F_PAEZ_SFYQZF"])).ToList();
                if (result.Count()>0)
                {
                    throw new Exception("所选单据存在没有勾选支持银企付款参数，不允许通过银企进行付款！");
                }
            }
            
        }
        public override void ToolBarItemClick(BarItemClickEventArgs e)
        {
            base.ToolBarItemClick(e);
        }
        public override void BeforeDoOperation(BeforeDoOperationEventArgs e)
        {
            base.BeforeDoOperation(e);

            
        }
    }
}
