using Kingdee.BOS.Core.Bill.PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.K3.FIN.FA.ServiceHelper;

namespace Keeper_Louis.K3.Siasun.FixedAssetsPlugIn.BusinessPlugIn
{
    [Description("资产卡片表单插件")]
    public class AssetCard:AbstractBillPlugIn
    {
        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            if (e.Field.Key.ToUpperInvariant().Equals("FASSETTYPEID"))
            {
                string assetNo = Convert.ToString(this.Model.GetValue("FASSETNO", 0));
                //this.Model.SetValue("FASSETNO",);
                string[] code = CardServiceHelper.GetAssetCodeByRule(base.Context, base.Model.DataObject, 1);
                this.Model.SetValue("FASSETNO", "1234", 0);
            }
            
        }
    }
}
