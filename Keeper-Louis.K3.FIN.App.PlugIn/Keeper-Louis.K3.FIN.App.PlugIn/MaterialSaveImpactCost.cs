using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.App.Data;

namespace Keeper_Louis.K3.FIN.App.PlugIn
{
    [Description("物料保存时更新物料纬度批号影响成本标识")]
    public class MaterialSaveImpactCost: AbstractOperationServicePlugIn
    {
        public override void AfterExecuteOperationTransaction(AfterExecuteOperationTransaction e)
        {
            base.AfterExecuteOperationTransaction(e);
            if (e.DataEntitys != null && e.DataEntitys.Count<DynamicObject>() > 0)
            {
                foreach (DynamicObject item in e.DataEntitys)
                {
                    long materialId = Convert.ToInt64(item["id"]);
                    //查询批号主档，确认该物料是否在系统中存在历史批号
                    //通过物料id获取masterID
                    string masterSql = string.Format(@"/*dialect*/SELECT FMASTERID FROM T_BD_MATERIAL WHERE FMATERIALID = {0}",materialId);
                    long masterId = DBUtils.ExecuteScalar<long>(this.Context,masterSql,0,null);
                    if (masterId!=0)
                    {
                        //通过masterId获取所有materialIds
                        string materialidSql = string.Format(@"/*dialect*/SELECT FMATERIALID FROM T_BD_MATERIAL WHERE FMASTERID = {0}", masterId);
                        DynamicObjectCollection doc = DBUtils.ExecuteDynamicObject(this.Context, materialidSql) as DynamicObjectCollection;
                        string materialIds = "";
                        foreach (DynamicObject dt in doc)
                        {
                            materialIds += "," + Convert.ToInt64(dt["FMATERIALID"]) + "";
                        }
                        materialIds = materialIds.Substring(1);
                        string phSql = string.Format(@"/*dialect*/ SELECT FNUMBER FROM T_BD_LOTMASTER WHERE FMATERIALID in ({0})", materialIds);
                        string result = DBUtils.ExecuteScalar<string>(this.Context, phSql, "noResult", null);
                        DynamicObjectCollection phcol = DBUtils.ExecuteDynamicObject(this.Context,phSql) as DynamicObjectCollection;
                        if (phcol.Count()<=0)
                        {
                            //更新语句
                            string strSql = string.Format(@"/*dialect*/UPDATE T_BD_MATERIALINVPTY SET FISAFFECTCOST=1 WHERE FINVPTYID=10004 and FISENABLE=1 and FISAFFECTCOST=0 and FMATERIALID = {0}", materialId);
                            DBUtils.Execute(this.Context, strSql);
                        }
                        ////批号主档没有对应物料批号
                        //if (result.Equals("noResult"))
                        //{
                        //    //更新语句
                        //    string strSql = string.Format(@"/*dialect*/UPDATE T_BD_MATERIALINVPTY SET FISAFFECTCOST=1 WHERE FINVPTYID=10004 and FISENABLE=1 and FISAFFECTCOST=0 and FMATERIALID = {0}", materialId);
                        //    DBUtils.Execute(this.Context, strSql);
                        //}
                    }
                }
            }
        }
    }
}
