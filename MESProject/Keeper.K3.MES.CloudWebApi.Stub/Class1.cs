using Kingdee.BOS.Core.Bill.PlugIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Keeper.K3.MES.Entity;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Keeper.K3.MES.CloudWebApi.Stub
{
    [Description("TESt")]
    public class Class1:AbstractBillPlugIn
    {
        public override void AfterButtonClick(AfterButtonClickEventArgs e)
        {
            base.AfterButtonClick(e);
            MaterialEntity material = new MaterialEntity();
            material.UserName = "zzz";
            material.UseOrgId = "111934";
            string sJon = JsonConvert.SerializeObject(material);
            WebReference.MESWebService request = new WebReference.MESWebService();
            request.GetMaterialDataList(sJon);
        }
    }
}
