using Kingdee.BOS;
using Kingdee.BOS.App.Core.DefaultValueService;
using Kingdee.BOS.App.Core.PlugInProxy;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.K3.MES.App.Core
{
    public class MyAppServiceContext
    {
        public static AbstractDynamicFormModel CreateDynamicFormModel(Context ctx,
            BusinessInfo businessInfo,
            Action<AbstractDynamicFormModel> action)
        {
            AbstractDynamicFormModel modelProxy = new DynamicFormModelProxy();
            FormServiceProvider provider = new FormServiceProvider();
            provider.Add(typeof(IDefaultValueCalculator), new DefaultValueCalculator());
            modelProxy.SetContext(ctx, businessInfo, provider);
            modelProxy.BeginIniti();
            modelProxy.CreateNewData();
            modelProxy.ClearNoDataRow();
            if (action != null)
            {
                action(modelProxy);
            }
            modelProxy.EndIniti();
            return modelProxy;
        }
    }
}
