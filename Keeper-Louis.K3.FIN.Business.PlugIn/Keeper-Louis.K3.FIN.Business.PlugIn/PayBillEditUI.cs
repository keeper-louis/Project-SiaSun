using Kingdee.BOS.Core.Bill.PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;

namespace Keeper_Louis.K3.FIN.Business.PlugIn
{
    [Description("付款单表单界面网银提交付款校验")]
    public class PayBillEditUI: AbstractBillPlugIn
    {
        public override void BarItemClick(BarItemClickEventArgs e)
        {
            base.BarItemClick(e);
            if (e.BarItemKey.Equals("tbSubmitBank"))
            {
                if (Convert.ToInt32(this.Model.GetValue("F_PAEZ_SFYQZF")) ==0)
                {
                    throw new Exception("该单据没有勾选支持银企付款参数，不允许通过银企进行付款！");
                } 
            }
        }
    }
}
