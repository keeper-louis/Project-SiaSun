using Keeper.K3.MES.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Test
{
    class Program1
    {
        //static void Main(string[] args)
        //{
        //    MaterialEntity material = new MaterialEntity();
        //    material.UserName = "zzz";
        //    material.UseOrgId = "111934";
        //    material.MaterialName = "123";
        //    material.MaterialNum = "123";
        //    material.MaterialSpec = "123";
        //    string sJon = JsonConvert.SerializeObject(material);
        //    //SimpleProBill simple = new SimpleProBill();
        //    //simple.K3CloudURI = "1111111";
        //    //simple.DBCenterId = "222222";
        //    //simple.CloudUserName = "yt";
        //    //simple.PassWord = "love0000";
        //    //List<SimpleProBillEntry> simplEntry = new List<SimpleProBillEntry>();
        //    //SimpleProBillEntry simEntry = new SimpleProBillEntry();
        //    //simEntry.FMaterialId = "001";
        //    //simEntry.FActualQty = 100;
        //    //simplEntry.Add(simEntry);
        //    //simEntry = new SimpleProBillEntry();
        //    //simEntry.FMaterialId = "002";
        //    //simEntry.FActualQty = 200;
        //    //simplEntry.Add(simEntry);
        //    //simple.FEntity = simplEntry;
        //    //string sJon1 = JsonConvert.SerializeObject(simple);

        //    //string url = "http://127.0.0.1:8099/meswebservice.asmx";
        //    ////string methodname = "HelloWorld"; GetMaterialDataList
        //    //string methodname = "GetMaterialDataList";
        //    ////创建一个http请求
        //    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/" + methodname);
        //    ////post请求方式  
        //    //request.Method = "POST";
        //    ////内容类型
        //    //request.ContentType = "application/x-www-form-urlencoded";

        //    ////设置参数，并进行url编码  
        //    ////虽然我们需要传递给服务器端的实际参数是jsonparas(格式：[{\"userid\":\"0206001\",\"username\":\"ceshi\"}])，
        //    ////但是需要将该字符串参数构造成键值对的形式（注："parameterjson=[{\"userid\":\"0206001\",\"username\":\"ceshi\"}]"），
        //    ////其中键parameterjson为webservice接口函数的参数名，值为经过序列化的json数据字符串
        //    ////最后将字符串参数进行url编码
        //    //string paraUrlCoded = System.Web.HttpUtility.UrlEncode("parameterjson");
        //    //paraUrlCoded += "=" + System.Web.HttpUtility.UrlEncode(sJon);

        //    //byte[] payload;
        //    ////将json字符串转化为字节  
        //    //payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
        //    ////设置请求的contentlength   
        //    //request.ContentLength = payload.Length;
        //    ////发送请求，获得请求流  

        //    //Stream writer;
        //    //try
        //    //{
        //    //    writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    writer = null;
        //    //    Console.Write("连接服务器失败!");
        //    //}

        //    ////将请求参数写入流
        //    //writer.Write(payload, 0, payload.Length);
        //    //writer.Close();//关闭请求流

        //    //String strValue = "";//strValue为http响应所返回的字符流
        //    //HttpWebResponse response;
        //    //try
        //    //{
        //    //    //获得响应流
        //    //    response = (HttpWebResponse)request.GetResponse();
        //    //}
        //    //catch (WebException ex)
        //    //{
        //    //    response = ex.Response as HttpWebResponse;
        //    //}

        //    //Stream s = response.GetResponseStream();

        //    ////服务器端返回的是一个XML格式的字符串，XML的Content才是我们所需要的Json数据
        //    //XmlTextReader Reader = new XmlTextReader(s);
        //    //Reader.MoveToContent();
        //    //strValue = Reader.ReadInnerXml();//取出Content中的Json数据
        //    //Reader.Close();
        //    //s.Close();


        //    //localhost20.MESWebService request = new localhost20.MESWebService();
        //    //var result = request.GetMaterialDataList(sJon);
        //    localhostMes.MaterialMessageFromCappServiceService requestMes = new localhostMes.MaterialMessageFromCappServiceService();
        //    var result = requestMes.StorageMessageFromKis(sJon);
        //}
    }
}
