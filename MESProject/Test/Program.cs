using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication.WebAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now);
            Console.ReadKey();
            //Invoke();
        }

        /// <summary>
        /// Http + Json直接调用
        /// </summary>
        private static void Invoke()
        {
            var result = InvokeHelper.Login();
            var iResult = JObject.Parse(result)["LoginResultType"].Value<int>();
            if (iResult == 1 || iResult == -5)
            {
                Console.WriteLine("login successed");
                string json = "";
                List<object> args = new List<object>();
                args.Add(json);
                //result = InvokeHelper.AbstractWebApiBusinessService("Kingdee.BOS.WebAPI.ServiceExtend.ServicesStub.CustomBusinessService.ExecuteService,Kingdee.BOS.WebAPI.ServiceExtend.ServicesStub", null);
                result = InvokeHelper.AbstractWebApiBusinessService("Keeper.K3.MES.CloudWebApi.Stub.MesWebApiBussinessService.CreateSimpleProBill,Keeper.K3.MES.CloudWebApi.Stub", null);

                {
                    Console.WriteLine(result);
                }
            }
            else
            {
                Console.WriteLine("login failed");
            }

            Console.ReadKey();
        }
    }
}