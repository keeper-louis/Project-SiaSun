using Kingdee.BOS.WebApi.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.K3.MES.Entity
{
    public class LoginResult
    {
        public LoginResult() { }

        /// <summary>
        /// 接入标识Guid：该标识非常重要，登录成功后，该标识将作为后续调用功能接口的首要参数
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserRealName { get; set; }

       // public ApiClient apiClient { get; set; }
    }
}
