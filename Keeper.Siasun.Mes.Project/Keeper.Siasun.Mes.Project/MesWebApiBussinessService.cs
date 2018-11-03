using Kingdee.BOS.WebApi.ServicesStub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.ServiceFacade.KDServiceFx;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Kingdee.BOS.WebApi.Client;
using Kingdee.BOS.Log;
using System.Data;
using Kingdee.BOS.App.Data;

namespace Keeper.Siasun.Mes.Project
{
    public class MesWebApiBussinessService : AbstractWebApiBusinessService
    {
        public MesWebApiBussinessService(KDServiceContext context) : base(context)
        {
        }
        public string GetMaterialGroup() {
            List <MaterialGroup> result = GetMaterialGroupList();
            return JsonConvert.SerializeObject(result);
        }

        private List<MaterialGroup> GetMaterialGroupList()
        {
            string strSql = string.Format(@"select a.FID,a.FNUMBER,b.FNAME,a.FPARENTID,a.FFULLPARENTID from T_BD_MATERIALGROUP a inner join T_BD_MATERIALGROUP_l b on a.FID =  b.FID where b.FLOCALEID = 2052");
            List<MaterialGroup> result = new List<MaterialGroup>();
            using (IDataReader dr = DBUtils.ExecuteReader(this.KDContext.Session.AppContext, strSql))
            {
                while (dr.Read())
                {
                    result.Add(new MaterialGroup()
                    {
                        FID = Convert.ToString(dr["FID"]),
                        FNUMBER = Convert.ToString(dr["FNUMBER"]),
                        FNAME = Convert.ToString(dr["FNAME"]),
                        FPARENTID = Convert.ToString(dr["FPARENTID"]),
                        FFULLPARENTID = Convert.ToString(dr["FFULLPARENTID"])
                    });
                }
                return result;
            }
        }

        /// <summary>
        /// 创建简单生产领料单
        /// </summary>
        /// <param name="parameterJson"></param>
        /// <returns></returns>
        public string CreateSimpleProBill(string parameterJson)
        {
            JObject jsonRoot = new JObject();//存储models
            //JArray models = new JArray();//多model批量保存时使用，存储mBHeader
            JObject mBHeader = new JObject();//model中单据头,存储普通变量、baseData、entrys
            JObject mBEntry = new JObject();//model中单据体，存储普通变量，baseData
            JObject baseData = new JObject();//model中基础资料
            JArray entrys = new JArray();//单个model中存储多行分录体集合，存储mBentry

            //0 解析parameter Json
            try
            {
                JObject Jo = (JObject)JsonConvert.DeserializeObject(parameterJson);
                string ServerUrl = "http://172.16.10.28/k3cloud/";
                string DBID = "5bc53c6e59302c";
                string UserName = "yt";
                string PassWord = "love0000";
                int ICID = 2052;
                /*单据头json封装*/
                mBHeader.Add("FID", 0);//FID
                baseData = new JObject();
                baseData.Add("FNumber", "YCL001");
                mBHeader.Add("FBILLTYPE", baseData);//单据类型
                mBHeader.Add("FDate", Convert.ToString(DateTime.Now));//业务日期--new
                baseData = new JObject();
                baseData.Add("FNumber", "0168");
                mBHeader.Add("FStockOrgId", baseData);//发料组织
                baseData = new JObject();
                baseData.Add("FStaffNumber", "01010002");//写死，需要根据实际情况进行修改***********************************************
                mBHeader.Add("FPickerId", baseData);//领料人
                baseData = new JObject();
                baseData.Add("FNumber", "PRE001");
                mBHeader.Add("FCurrId", baseData);//币别

                mBHeader.Add("FOwnerTypeId0", "BD_OwnerOrg");//货主类型
                baseData = new JObject();
                baseData.Add("FNumber", "0168");
                mBHeader.Add("FOwnerId0", baseData);//货主
                baseData = new JObject();
                baseData.Add("FNumber", "0168");
                mBHeader.Add("FPrdOrgId", baseData);//生产组织
                baseData = new JObject();
                baseData.Add("FNumber", "OverOrgPick");
                mBHeader.Add("FTransferBizTypeId", baseData);//跨组织业务类型





                baseData = new JObject();
                baseData.Add("FNumber", Convert.ToString(Jo["F_PAEZ_XMXX"]));
                mBHeader.Add("F_PAEZ_XMXX", baseData);//领用项目号
                mBHeader.Add("FBizType", "NORMAL");//业务类型
                baseData = new JObject();
                baseData.Add("FNumber", Convert.ToString(Jo["F_xs_NBXMH"]));
                mBHeader.Add("F_xs_NBXMH", baseData);//内部项目号

                mBHeader.Add("F_xs_CGDAH", Convert.ToString(Jo["FSSPORDERNO"]));//采购订单单号
                mBHeader.Add("FMesOutStockNo", Convert.ToString(Jo["MesOutStockNo"]));//Mes出库单号


                mBHeader.Add("FISTRANSMITWMS", "false");//是否传递wms



                JArray Ja = (JArray)JsonConvert.DeserializeObject(Jo["FEntity"].ToString());
                foreach (JObject item in Ja)
                {
                    mBEntry = new JObject();
                    if (Convert.ToString(Jo["Isos"]).Equals("0"))//外协件 写死001
                    {
                        baseData = new JObject();
                        baseData.Add("FNumber", "96.01.001.00001");
                        mBEntry.Add("FMaterialId", baseData);//物料编码
                    }
                    else
                    {
                        baseData = new JObject();
                        baseData.Add("FNumber", Convert.ToString(item["FMaterialId"]));
                        mBEntry.Add("FMaterialId", baseData);//物料编码
                    }
                    //\"FUnitID\":{\"FNumber\":\"Pcs\"}

                    mBEntry.Add("FActualQty", Convert.ToString(item["FActualQty"]));//实发数量
                    baseData = new JObject();
                    baseData.Add("FNumber", "D.0168");
                    mBEntry.Add("FStockId", baseData);//仓库，制造三部一级库***********************************************

                    //baseData = new JObject();
                    //baseData.Add("FNumber", "00-00-01");
                    //JObject CWZJ = new JObject();//仓位值级
                    //CWZJ.Add("FSTOCKLOCID__FF100062", baseData);
                    ////mBEntry.Add("FCHILDUNITID", CWZJ);//仓位，不启用，先写死***********************************************
                    //mBEntry.Add("FStockLocId", CWZJ);
                    baseData = new JObject();
                    baseData.Add("FNumber", Convert.ToString(item["FLot"]));
                    mBEntry.Add("FLot", baseData);//批号

                    mBEntry.Add("FIsAffectCost", "false");//生产编号影响成本,default：fasle
                    mBEntry.Add("FStockActualQty", Convert.ToString(item["FActualQty"]));//库存单位实发数量

                    
                    mBEntry.Add("FOwnerTypeId", "BD_OwnerOrg");//货主类型
                    baseData = new JObject();
                    baseData.Add("FNumber", "0168");
                    mBEntry.Add("FOwnerId", baseData);//货主

                    mBEntry.Add("FParentOwnerTypeId", "BD_OwnerOrg");//产品货主类型
                    baseData = new JObject();
                    baseData.Add("FNumber", "0168");
                    mBEntry.Add("FParentOwnerId", baseData);//产品货主
                    //\"FBaseActualQty\":10101.0,

                    baseData = new JObject();
                    baseData.Add("FNumber", "KCZT01_SYS");
                    mBEntry.Add("FStockStatusId", baseData);//库存状态

                 //   mBEntry.Add("FKeeperTypeId", "BD_OwnerOrg");//保管者类型
                  //  baseData = new JObject();
                  //  baseData.Add("FNumber", "0168");
                 //   mBEntry.Add("FKeeperId", baseData);//保管者


                    entrys.Add(mBEntry);
                }
                mBHeader.Add("FEntity", entrys);


                jsonRoot.Add("Creator", "");
                jsonRoot.Add("IsDeleteEntry", "True");
                jsonRoot.Add("SubSystemId", "");
                jsonRoot.Add("IsVerifyBaseDataField", "false");//有问题
                jsonRoot.Add("Model", mBHeader);


                string sFormId = "SP_PickMtrl";
                string sContent = JsonConvert.SerializeObject(jsonRoot);
                object[] saveInfo = new object[] { sFormId, sContent };


                ApiClient client = new ApiClient(ServerUrl);
                bool bLogin = client.Login(DBID, UserName, PassWord, ICID);
                if (bLogin)
                {
                    var ret = client.Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Save", saveInfo);
                    JObject sResult = JObject.Parse(ret);
                    JObject saveStatus = sResult["Result"]["ResponseStatus"] as JObject;
                    if (saveStatus["IsSuccess"].ToString().Equals("True"))
                    {
                        //将保存成功信息写入日志ret
                        Logger.Info("saveSuccess:", ret);

                        JArray successEntity = JArray.Parse(saveStatus["SuccessEntitys"].ToString());
                        JObject jo = new JObject();
                        jo.Add("Ids", successEntity[0]["Id"]);
                        //jo.Add("Numbers", successEntity[0]["Number"]);
                        string submitJson = JsonConvert.SerializeObject(jo);
                        var submitResult = client.Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Submit",
                        new object[] { "SP_PickMtrl", submitJson });
                        JObject bResult = JObject.Parse(submitResult);
                        JObject submitStatus = bResult["Result"]["ResponseStatus"] as JObject;
                        if (submitStatus["IsSuccess"].ToString().Equals("True"))
                        {
                            //将提交成功信息写入日志submitResult
                            Logger.Info("submitSuccess:", submitResult);

                            JArray succEntity = JArray.Parse(saveStatus["SuccessEntitys"].ToString());
                            JObject joi = new JObject();
                            joi.Add("Ids", succEntity[0]["Id"]);
                            //jo.Add("Numbers", successEntity[0]["Number"]);
                            string auditJson = JsonConvert.SerializeObject(joi);
                            var auditResult = client.Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Audit",
                            new object[] { "SP_PickMtrl", auditJson });
                            JObject aResult = JObject.Parse(auditResult);
                            JObject audittStatus = aResult["Result"]["ResponseStatus"] as JObject;
                            if (audittStatus["IsSuccess"].ToString().Equals("True"))
                            {
                                //将审核成功信息写入日志auditResult
                                Logger.Info("auditSuccess:", auditResult);
                                return "syncSuccess";
                            }
                            else
                            {
                                JArray audit_errors_Entity = JArray.Parse(audittStatus["Errors"].ToString());
                                //将审核失败信息写入日志auditResult
                                Logger.Error("auditFaild:", auditResult, null);
                                //返回审核失败信息
                                return audit_errors_Entity[0]["FieldName"].ToString() + audit_errors_Entity[0]["Message"].ToString();
                            }

                        }
                        else
                        {
                            JArray submit_errors_Entity = JArray.Parse(submitStatus["Errors"].ToString());
                            //将错误信息写入日志submitResult
                            Logger.Error("submitFaild:", submitResult, null);
                            //返回错误信息
                            return submit_errors_Entity[0]["FieldName"].ToString() + submit_errors_Entity[0]["Message"].ToString();
                        }

                    }
                    else
                    {
                        JArray save_errors_Entity = JArray.Parse(saveStatus["Errors"].ToString());
                        //将错误信息写入日志ret
                        Logger.Error("saveFaild:", ret, null);
                        //返回错误信息
                        return save_errors_Entity[0]["FieldName"].ToString() + save_errors_Entity[0]["Message"].ToString();
                    }

                }
                else
                {
                    //将错误信息写到日志
                    Logger.Error("Login:", "登录失败", null);
                    //返回错误信息
                    return returnJsonError("Login", "登录失败");
                }
            }
            catch (Exception ex)
            {
                //将异常写到日志
                Logger.Error("exception:", ex.Message, null);
                //返回错误信息
                return returnJsonError("捕获异常：", ex.Message);
            }

        }

        string returnJsonError(string fieldName, string message)
        {
            return "{\"Result\":{\"ResponseStatus\":{\"ErrorCode\":500,\"IsSuccess\":false,\"Errors\":[{\"FieldName\":\"" + fieldName + "\",\"Message\":\"" + message + "\"}]}}}";
        }

        string returnJsonSuccess()
        {
            return "{\"Result\":{\"ResponseStatus\":{\"ErrorCode\":0,\"IsSuccess\":true,\"Errors\":[]}}}";
        }
    }
}
