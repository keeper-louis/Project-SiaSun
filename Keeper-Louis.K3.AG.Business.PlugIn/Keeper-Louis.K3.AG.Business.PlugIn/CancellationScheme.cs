using Kingdee.BOS.Core.Bill.PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.App.Data;

namespace Keeper_Louis.K3.AG.Business.PlugIn
{
    [Description("核销方案基础资料界面插件")]
    public class CancellationScheme:AbstractBillPlugIn
    {
        public override void BeforeF7Select(BeforeF7SelectEventArgs e)
        {
            base.BeforeF7Select(e);
            if (e.FieldKey.ToUpperInvariant().Equals("F_LHR_HSWD"))
            {
                DynamicObject dot = (DynamicObject)this.Model.GetValue("F_LHR_ACCOUNT");
                long acctId = Convert.ToInt64(dot["Id"]);
                //根据已选择的科目进行基础资料过滤
                string strSql = string.Format(@"/*dialect*/select af.FFLEXITEMPROPERTYID from T_BD_ACCOUNT ac inner join T_BD_ACCOUNTFLEXENTRY af on ac.FACCTID = af.FACCTID where ac.FACCTID = {0}",acctId);
                DynamicObjectCollection doc = DBUtils.ExecuteDynamicObject(this.Context, strSql) as DynamicObjectCollection;
                string wdId = "";
                foreach (DynamicObject dt in doc)
                {
                    wdId += "," + Convert.ToInt64(dt["FFLEXITEMPROPERTYID"]) + "";
                }
                wdId = wdId.Substring(1);//客户维度
                string filter = string.Format(" F_LHR_HSWD IN ({0})",wdId);
                if (string.IsNullOrEmpty(e.ListFilterParameter.Filter))
                {
                    e.ListFilterParameter.Filter = filter;
                }
                else
                {
                    e.ListFilterParameter.Filter += " AND " + filter;
                }

            }
        }
    }
}
