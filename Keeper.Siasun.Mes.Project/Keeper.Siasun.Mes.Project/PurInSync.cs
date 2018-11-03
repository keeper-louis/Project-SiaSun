using Kingdee.BOS.Core.DynamicForm.PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Orm.DataEntity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Keeper.Siasun.Mes.Project
{
    [Description("采购入库审核传入MES")]
    public class PurInSync : AbstractOperationServicePlugIn
    {
        public override void OnPreparePropertys(PreparePropertysEventArgs e)
        {
            base.OnPreparePropertys(e);
            e.FieldKeys.Add("FSSPORDERNO");//采购订单号
            e.FieldKeys.Add("F_PAEZ_XMXX");
            e.FieldKeys.Add("FRealQty");
            e.FieldKeys.Add("FWXTaskNo");//外协任务号
        }
        public override void EndOperationTransaction(EndOperationTransactionArgs e)
        {
            //test();
            base.EndOperationTransaction(e);
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
            if (e.DataEntitys != null && e.DataEntitys.Count<DynamicObject>() > 0)
            {
                foreach (DynamicObject DataEntity in e.DataEntitys)
                {

                    //标准采购入库
                    if (Convert.ToString(((DynamicObject)DataEntity["StockOrgId"])["Number"]).Equals("0168"))
                    {
                        DynamicObjectCollection inStockEntrys = DataEntity["InStockEntry"] as DynamicObjectCollection;
                        if (!Convert.ToString(((DynamicObject)inStockEntrys[0]["MaterialId"])["Name"]).Contains("标准件"))
                        {
                            string sJon = CreateJson(DataEntity);
                            if (!sJon.Equals("传递参数拼接失败"))
                            {
                                //ResManager
                                //调用MES提供的接口
                                WebMesTest.MaterialMessageFromCappServiceService requestMes = new WebMesTest.MaterialMessageFromCappServiceService();
                                string mesResult = requestMes.StorageMessageFromKis(sJon);
                                if (!mesResult.Equals("0"))//传递失败
                                {
                                    throw new Exception(mesResult);
                                }
                            }
                        }
                    }


                    //外协采购入库
                    //else if (Convert.ToString(DataEntity["FBillTypeID"]).Equals(""))
                    //{
                    //    CreateJson(DataEntity);
                    //}
                }
            }
        }

        //测试webapi调用
        //private void test()
        //{
        //    var result = InvokeHelper.Login();
        //    var iResult = JObject.Parse(result)["LoginResultType"].Value<int>();
        //    if (iResult == 1 || iResult == -5)
        //    {
        //        Console.WriteLine("login successed");

        //        //result = InvokeHelper.AbstractWebApiBusinessService("Kingdee.BOS.WebAPI.ServiceExtend.ServicesStub.CustomBusinessService.ExecuteService,Kingdee.BOS.WebAPI.ServiceExtend.ServicesStub", null);
        //        result = InvokeHelper.AbstractWebApiBusinessService("Keeper.Siasun.Mes.Project.MesWebApiBussinessService.GetMaterialGroup,Keeper.Siasun.Mes.Project", null);
        //    }
        //}



        private string CreateJson(DynamicObject dataEntity)
        {
            JObject jsonRoot = new JObject();//存储models
            //JArray models = new JArray();//多model批量保存时使用，存储mBHeader
            JObject mBHeader = new JObject();//model中单据头,存储普通变量、baseData、entrys
            JObject mBEntry = new JObject();//model中单据体，存储普通变量，baseData
            //JObject baseData = new JObject();//model中基础资料
            JArray entrys = new JArray();//单个model中存储多行分录体集合，存储mBentry
            mBHeader.Add("BillNo", Convert.ToString(dataEntity["BillNo"]));//入库单号
            mBHeader.Add("F_PAEZ_XMXX", Convert.ToString(((DynamicObject)dataEntity["F_PAEZ_XMXX"])["Number"]));//项目编号
            mBHeader.Add("FSSPORDERNO", Convert.ToString(dataEntity["FSSPORDERNO"]));//采购订单号


            if (Convert.ToString(dataEntity["FBillTypeID_Id"]).Equals("597056e3c807bc"))
            {
                mBHeader.Add("Isos", "0");//是外协
            }
            else
            {
                mBHeader.Add("Isos", "1");//非外协
            }

            DynamicObjectCollection inStockEntrys = dataEntity["InStockEntry"] as DynamicObjectCollection;
            foreach (DynamicObject inStockEntry in inStockEntrys)
            {
                mBEntry = new JObject();
                mBEntry.Add("FMATERIALLNO", Convert.ToString(((DynamicObject)inStockEntry["MaterialId"])["Number"]));//物料编号
                mBEntry.Add("FQTY", Convert.ToDouble(inStockEntry["RealQty"]));//数量
                mBEntry.Add("FLOT", Convert.ToString(inStockEntry["Lot_Text"]));//批号
                if (!Convert.ToString(((DynamicObject)((DynamicObject)inStockEntrys[0]["MaterialId"])["MaterialGroup"])["Number"]).Equals("") && Convert.ToString(((DynamicObject)((DynamicObject)inStockEntrys[0]["MaterialId"])["MaterialGroup"])["Number"]) != null)
                {
                    mBEntry.Add("FMATERIALGROUP", Convert.ToString(((DynamicObject)((DynamicObject)inStockEntrys[0]["MaterialId"])["MaterialGroup"])["Number"]));//物料分组
                }
                else
                {
                    throw new Exception(Convert.ToString(((DynamicObject)inStockEntry["MaterialId"])["Number"]) + "物料分组未维护");
                }

                //if (Convert.ToString(inStockEntry["FWXTaskNo"]) != null && !Convert.ToString(inStockEntry["FWXTaskNo"]).Equals(""))
                //{
                //    mBEntry.Add("FWXTaskNo", Convert.ToString(inStockEntry["FWXTaskNo"]));
                //}
                entrys.Add(mBEntry);
            }
            mBHeader.Add("FEntity", entrys);
            jsonRoot.Add("Model", mBHeader);
            string sContent = JsonConvert.SerializeObject(jsonRoot);
            return sContent != null && !sContent.Equals(" ") && !sContent.Equals("") ? sContent : "传递参数拼接失败";

        }
    }
}
