using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.Siasun.Mes.Project
{
    public static class InvokeHelper
    {
        private static string CloudUrl = "http://172.16.10.27/k3cloud/";
        /// <summary>
        /// 登陆
        /// </summary>
        public static string Login()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.Url = string.Concat(CloudUrl, "Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUser.common.kdsvc");

            List<object> Parameters = new List<object>();
            Parameters.Add("5bc53c6e59302c");//帐套Id
            Parameters.Add("yt");//用户名
            Parameters.Add("love0000");//密码

            Parameters.Add(2052);
            httpClient.Content = JsonConvert.SerializeObject(Parameters);
            return httpClient.SysncRequest();
        }

        /// <summary>
        /// 自定义
        /// </summary>
        /// <param name="key">自定义方法标识</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static string AbstractWebApiBusinessService(string key, List<object> args)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.Url = string.Concat(CloudUrl, key, ".common.kdsvc");

            httpClient.Content = JsonConvert.SerializeObject(args);
            return httpClient.SysncRequest();
        }
    }
}
