using Kingdee.BOS.Core.Bill.PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.K3.FIN.FA.App.Core;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.App;
using Kingdee.BOS.Contracts;
using Kingdee.BOS.Core.Metadata;
using Kingdee.BOS.App.Core;
using Kingdee.BOS.Orm;
using Kingdee.BOS.Util;

namespace Keeper_Louis.K3.Siasun.FixedAssetsPlugIn.BusinessPlugIn
{
    [Description("资产申请单资产类别自动获取资产编码规则")]
    public class AssetApply:AbstractBillPlugIn
    {
        public override void DataChanged(DataChangedEventArgs e)
        {
            base.DataChanged(e);
            if (e.Field.Key.ToUpperInvariant().Equals("F_PAEZ_ZCLB1"))
            {
                if ((DynamicObject)base.View.Model.GetValue("F_PAEZ_ZCLB1")!=null)
                {
                   // CodeRuleService service = new CodeRuleService();
                    DynamicObject obj = this.Model.DataObject as DynamicObject;
                    DynamicObject obj2 = obj["F_PAEZ_ZCLB1"] as DynamicObject;
                    if (obj2==null)
                    {
                        
                    }
                    DynamicObject obj3 = obj2["FAssetApplyRuleCodeID"] as DynamicObject;
                    FormMetadata metadata = ServiceHelper.GetService<IMetaDataService>().Load(this.Context, "FA_REQUISITION", true) as FormMetadata;
                    DynamicObject[] dataEntites = new DynamicObject[1];
                    dataEntites[0] =  OrmUtils.Clone(obj, false, true) as DynamicObject;
                    BusinessInfo info = ObjectUtils.CreateCopy(metadata.BusinessInfo) as BusinessInfo;
                    info.GetBillNoField().Entity.TableName = "";
                    IBusinessDataService instance = ServiceHelper.GetService<IBusinessDataService>();
                    string [] billno = (from p in ServiceHelper.GetService<IBusinessDataService>().GetBillNo(this.Context, info, dataEntites, false, obj3["ID"].ToString()) select p.BillNo).ToList<string>().ToArray();
                }
                
            }
        }
    }
}
