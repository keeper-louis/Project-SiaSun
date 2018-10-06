using Keeper.K3.MES.Contracts;
using Keeper.K3.MES.Entity;
using Kingdee.BOS.WebApi.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Keeper_Louis.K3.MES.WebService
{
    /// <summary>
    /// MESWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class MESWebService : System.Web.Services.WebService
    {
        /// <summary>
        /// 字典(Session,ApiClient)
        /// </summary>
        /// <remarks>
        /// ApiClient需引用Kingdee.BOS.WebApi.Client.dll并using Kingdee.BOS.WebApi.Client;
        /// </remarks>
        private static ConcurrentDictionary<string, ApiClient> DctTokenApiClient = new ConcurrentDictionary<string, ApiClient>();

        /// <summary>
        /// K3Cloud站点地址
        /// </summary>
        string K3CloudURI = ConfigurationManager.AppSettings["K3CloudURI"].ToString();

        /// <summary>
        /// 数据中心ID
        /// </summary>
        string DBCenterId = ConfigurationManager.AppSettings["DBCenterId"].ToString();

        /// <summary>
        /// 用户
        /// </summary>
        string UserName = ConfigurationManager.AppSettings["UserName"].ToString();

        /// <summary>
        /// 密码
        /// </summary>
        string PassWord = ConfigurationManager.AppSettings["PassWord"].ToString();

        /// <summary>
        /// 语言环境ID：简体中文2052，英文1033，繁体中文
        /// </summary>
        int LocaleId = Convert.ToInt16(ConfigurationManager.AppSettings["LocaleId"]);


        [WebMethod(Description = "Test WebService")]
        public String HelloWorld(String parameterJson)
        {
            // MaterialEntity material = JsonConvert.DeserializeObject<MaterialEntity>(parameterJson);
            //return parameterJson;
            return "调用成功"+parameterJson;
        }
        /// <summary>
        /// 登录到K3Cloud系统
        /// </summary>
        /// <param name="weixinNumber">微信号</param>
        /// <returns></returns>
        [WebMethod(Description = "登录到K3Cloud系统")]
        public ReturnInfo<LoginResult> Login(string userName,string password)
        {
            LoginResult loginResult = new LoginResult();
            ApiClient apiClient = new ApiClient(K3CloudURI);
            ReturnInfo<LoginResult> returnInfo = new ReturnInfo<LoginResult>();
            if (userName.Equals(EncryptionDecryption.Instance.BOSEnCode(UserName))&&password.Equals(EncryptionDecryption.Instance.BOSEnCode(PassWord)))
            {
                bool isSuccess = apiClient.Login(DBCenterId, EncryptionDecryption.Instance.BOSDeCode(UserName), EncryptionDecryption.Instance.BOSDeCode(PassWord), LocaleId); //此处借用BOS的WebApi登录接口登录到K3Cloud系统
                string accessToken = string.Empty;
                if (isSuccess)
                {
                    returnInfo.IsSuccess = true;
                    returnInfo.Message = "登录成功";

                    //生成GUID作为登录标识
                    //accessToken = string.Format("{0}{1}", weixinNumber, System.DateTime.Now.Ticks);
                    loginResult.AccessToken = userName;
                    loginResult.UserRealName = userName;
                    ApiClient oldApiClient;
                    if (DctTokenApiClient.TryGetValue(userName, out oldApiClient))
                    {
                        DctTokenApiClient.TryRemove(userName, out oldApiClient);
                    }

                    DctTokenApiClient.AddOrUpdate(userName, apiClient, (k, v) =>
                    {
                        return v;
                    });
                    //loginResult.apiClient = apiClient;
                }
                else
                {
                    returnInfo.IsSuccess = false;
                    returnInfo.Message = "登录失败，用户名或密码错误";
                }
            }
            else
            {
                returnInfo.IsSuccess = false;
                returnInfo.Message = "登录失败，用户名或密码错误";
            }
            return returnInfo;
        }

        [WebMethod(Description ="获取物料信息列表")]
        public string GetMaterialDataList(string parameterJson)
        {
            MaterialEntity material = JsonConvert.DeserializeObject<MaterialEntity>(parameterJson);
            ApiClient apiClient;
            if (!DctTokenApiClient.TryGetValue(material.UserName, out apiClient))
            {
                ReturnInfo<LoginResult> loginResult = Login(material.UserName,material.MaterialName);
                if (!loginResult.IsSuccess)
                {
                    return JsonConvert.SerializeObject(loginResult);
                }
            }
            DctTokenApiClient.TryGetValue(material.UserName, out apiClient);
            object[] obj = new object[] { parameterJson };
            Kingdee.BOS.Log.Logger.Info("View_MaterialList", string.Format("parameterJson:{0}", parameterJson));
            string result = apiClient.Execute<string>("Keeper.K3.MES.CloudWebApi.Stub.MesWebApiBussinessService.GetMaterialDataList,Keeper.K3.MES.CloudWebApi.Stub", obj);
            return result;
        }
        [WebMethod(Description = "创建简单生产领料")]
        public string CreateSimpleProBill(string parameterJson)
        {
            SimpleProBill simpleBill = JsonConvert.DeserializeObject<SimpleProBill>(parameterJson);
            ApiClient apiClient;
            if (!DctTokenApiClient.TryGetValue(simpleBill.UserName, out apiClient))
            {
                ReturnInfo<LoginResult> loginResult = Login(simpleBill.UserName,simpleBill.PassWord);
                if (!loginResult.IsSuccess)
                {
                    return JsonConvert.SerializeObject(loginResult);
                }
            }
            DctTokenApiClient.TryGetValue(simpleBill.UserName, out apiClient);
            object[] obj = new object[] { parameterJson };
            Kingdee.BOS.Log.Logger.Info("Save_SimpleProBill", string.Format("parameterJson:{0}", parameterJson));
            string result = apiClient.Execute<string>("Keeper.K3.MES.CloudWebApi.Stub.MesWebApiBussinessService.GetMaterialDataList,Keeper.K3.MES.CloudWebApi.Stub", obj);
            return result;
        }

    }
}
