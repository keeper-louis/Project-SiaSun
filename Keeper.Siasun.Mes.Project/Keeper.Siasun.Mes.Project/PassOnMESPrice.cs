using Kingdee.BOS.App.Data;
using Kingdee.BOS.Core.Bill.PlugIn;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.Siasun.Mes.Project
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
                string strSql = string.Format(@"/*dialect*/SELECT MAT.FNUMBER,
         sum(spData.FAMOUNT)/sum(spData.FACTUALQTY) avgPrice
FROM T_SP_PICKMTRL sp
INNER JOIN T_SP_PICKMTRLDATA spData
    ON sp.FID = spData.FID
INNER JOIN T_BD_MATERIAL MAT
    ON spData.FMATERIALID = MAT.FMATERIALID
WHERE sp.FPRDORGID = 111934 

group by MAT.FNUMBER");
                DynamicObjectCollection result = DBUtils.ExecuteDynamicObject(this.Context,strSql);
                if (result!=null&&result.Count()>0)
                {
                    string sJson = CreateJson(result);
                    if (!sJson.Equals("传递参数拼接失败"))
                    {
                        WebMesTest.MaterialMessageFromCappServiceService requestMes = new WebMesTest.MaterialMessageFromCappServiceService();
                        requestMes.WeightingAvgNumerAsync(sJson);

                        //ResManager
                        //调用MES提供的接口
                    }
                }
            }

        }

        private string CreateJson(DynamicObjectCollection result)
        {
            JObject jsonRoot = new JObject();//存储models
            JArray models = new JArray();//多model批量保存时使用，存储mBHeader
            JObject mBHeader = new JObject();//model中单据头,存储普通变量、baseData、entrys
            //JObject mBEntry = new JObject();//model中单据体，存储普通变量，baseData
            //JObject baseData = new JObject();//model中基础资料
            //JArray entrys = new JArray();//单个model中存储多行分录体集合，存储mBentry
            //mBHeader.Add("BillNo", Convert.ToString(dataEntity["BillNo"]));//入库单号
            foreach (DynamicObject item in result)
            {
                mBHeader = new JObject();//model中单据头,存储普通变量、baseData、entrys
                mBHeader.Add("FMATERIALNO", Convert.ToString(item["FNUMBER"]));
                mBHeader.Add("FPRICE", Convert.ToString(item["avgPrice"]));
                models.Add(mBHeader);
            }
            jsonRoot.Add("Model", models);
            string sContent = JsonConvert.SerializeObject(jsonRoot);
            return sContent != null && !sContent.Equals(" ") && !sContent.Equals("") ? sContent : "传递参数拼接失败";

        }
    }
}
