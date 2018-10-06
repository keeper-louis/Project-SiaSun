using Kingdee.BOS;
using Kingdee.BOS.ServiceHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Keeper_Louis.K3.FIN.App.PlugIn
{
    static class YCT_Helper
    {
        private static string _defaultDomain = "10.2.1.17";
        public static string SuccessValue = "1000";

        // Methods
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) =>
            true;

        public static string GetDomain(Context ctx)
        {
            List<long> orgIds = new List<long> { 0L };
            Dictionary<long, object> dictionary = SystemParameterServiceHelper.GetParamter(ctx, orgIds, 0L, "PAEZ_PCPARAMETER", "F_YCT_API_DOMAIN");
            if ((dictionary != null) && (dictionary.Count != 0))
            {
                return dictionary[0L].ToString();
            }
            return _defaultDomain;
        }

        public static string GetJsonValue(string url)
        {
            JObject obj2 = (JObject)JsonConvert.DeserializeObject(GetStringValue(url));
            return obj2["Code"].ToString();
        }

        public static string GetStringValue(string url)
        {
            if (url.StartsWith("https", StringComparison.CurrentCultureIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(YCT_Helper.CheckValidationResult);
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
