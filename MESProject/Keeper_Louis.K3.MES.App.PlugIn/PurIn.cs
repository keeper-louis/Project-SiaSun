using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using System.ComponentModel;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.Resource;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Keeper_Louis.K3.MES.App.PlugIn
{
    [Description("采购入库审核传入MES")]
    public class PurIn:AbstractOperationServicePlugIn
    {
        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            base.OnPreparePropertys(e);
            e.FieldKeys.Add("F_PAEZ_XMXX");
            e.FieldKeys.Add("FRealQty");
            e.FieldKeys.Add("FWXTaskNo");//外协任务号
        }
        public override void AfterExecuteOperationTransaction(AfterExecuteOperationTransaction e)
        {
            base.AfterExecuteOperationTransaction(e);
            /*
             * 标准件入库单
             * 单据头
             * 入库单号
             * 项目号
             * 单据体
             * 物料编号
             * 数量
             * 批号
             * ************************************
             * 外协件入库单
             * 单据头
             * 入库单号
             * 项目号
             * 单据体
             * 物料编号
             * 数量
             * 批号
             * 外协计划号
             */
            if (e.DataEntitys!=null&&e.DataEntitys.Count<DynamicObject>()>0)
            {
                foreach (DynamicObject DataEntity in e.DataEntitys)
                {
                    //标准采购入库
                    if (Convert.ToString(DataEntity["FBillTypeID_Id"]).Equals("a1ff32276cd9469dad3bf2494366fa4f"))
                    {
                        string sJon = CreateJson(DataEntity);
                        if (!sJon.Equals("传递参数拼接失败"))
                        {
                            //ResManager
                            //调用MES提供的接口
                        }

                    }
                    //外协采购入库
                    else if (Convert.ToString(DataEntity["FBillTypeID"]).Equals(""))
                    {
                        CreateJson(DataEntity);
                    }
                }
            }
        }

        private string CreateJson(DynamicObject dataEntity)
        {
            //JObject jsonRoot = new JObject();//存储models
            //JArray models = new JArray();//多model批量保存时使用，存储mBHeader
            JObject mBHeader = new JObject();//model中单据头,存储普通变量、baseData、entrys
            JObject mBEntry = new JObject();//model中单据体，存储普通变量，baseData
            //JObject baseData = new JObject();//model中基础资料
            JArray entrys = new JArray();//单个model中存储多行分录体集合，存储mBentry
            mBHeader.Add("BillNo", Convert.ToString(dataEntity["BillNo"]));//入库单号
            mBHeader.Add("F_PAEZ_XMXX", Convert.ToString(((DynamicObject)dataEntity["F_PAEZ_XMXX"])["Number"]));//项目编号
            DynamicObjectCollection inStockEntrys = dataEntity["InStockEntry"] as DynamicObjectCollection;
            foreach (DynamicObject inStockEntry in inStockEntrys)
            {
                mBEntry = new JObject();
                mBEntry.Add("MaterialId", Convert.ToString(((DynamicObject)inStockEntry["MaterialId"])["Number"]));//物料编号
                mBEntry.Add("RealQty", Convert.ToDouble(inStockEntry["RealQty"]));//数量
                mBEntry.Add("Lot_Text", Convert.ToString(inStockEntry["Lot_Text"]));//批号
                if (Convert.ToString(inStockEntry["FWXTaskNo"])!=null&& !Convert.ToString(inStockEntry["FWXTaskNo"]).Equals(""))
                {
                    mBEntry.Add("FWXTaskNo", Convert.ToString(inStockEntry["FWXTaskNo"]));
                }
                entrys.Add(mBEntry);
            }
            mBHeader.Add("FEntity", entrys);
            string sContent = JsonConvert.SerializeObject(mBHeader);
            return sContent != null && !sContent.Equals(" ") && !sContent.Equals("") ? sContent : "传递参数拼接失败";

        }
    }
}
