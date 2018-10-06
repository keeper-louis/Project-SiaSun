using Keeper.K3.MES.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Keeper.K3.MES.Entity;
using Kingdee.BOS;
using Kingdee.BOS.App.Data;
using System.Data;
using Kingdee.BOS.Core.DynamicForm;
using Kingdee.BOS.Core.Metadata;
using Kingdee.K3.MFG.App;
using Kingdee.BOS.Orm.DataEntity;
using Kingdee.BOS.WebApi.Client;
using System.Configuration;

namespace Keeper.K3.MES.App.Core
{
    public class CommonService:ICommonService
    {
        /// <summary>
        /// 创建简单生产领料单
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="simpleProBill"></param>
        /// <returns></returns>
        public IOperationResult CreateSimpleProBill(Context ctx, SimpleProBill simpleProBill)
        {
            /// <summary>
            /// K3Cloud站点地址
            /// </summary>
            //string K3CloudURI = ConfigurationManager.AppSettings["K3CloudURI"].ToString();
            //ApiClient apiClient = new ApiClient(K3CloudURI);
            //bool isSuccess = apiClient.Login(DBCenterId, EncryptionDecryption.Instance.BOSDeCode(UserName), EncryptionDecryption.Instance.BOSDeCode(PassWord), LocaleId);
            FormMetadata meta = AppServiceContext.MetadataService.Load(ctx, "SP_PickMtrl") as FormMetadata;
            DynamicObject recordData = null;
            //MyAppServiceContext.CreateDynamicFormModel(ctx, meta.BusinessInfo, (model) =>
            //{
            //    model.SetValue("FNumber", customerWeiXin.WeiXinNumber);
            //    model.SetValue("FName", customerWeiXin.CustomerName);
            //    model.SetValue("F_KLM_WeiXinNumber", customerWeiXin.WeiXinNumber);
            //    model.SetValue("F_KLM_Tel", customerWeiXin.TelPhone);
            //    model.SetValue("F_KLM_CustomerId", customerId);
            //    recordData = model.DataObject;
            //});
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="material">物料列表实体</param>
        /// <returns></returns>
        public List<MaterialEntity> GetMateriaDatalList(Context ctx, MaterialEntity material)
        {
            //           string strSql = @"SELECT TOP 100 MAT.FNUMBER,MATL.FNAME,MATL.FSPECIFICATION
            // FROM T_BD_MATERIAL MAT
            //INNER JOIN T_BD_MATERIAL_L MATL
            //   ON MAT.FMATERIALID = MATL.FMATERIALID
            //WHERE MAT.FUSEORGID = @USEORGID
            //  AND MAT.FDOCUMENTSTATUS = '@DOCUMENTSTATUS'
            //  AND MAT.FFORBIDSTATUS = '@FORBIDSTATUS'";
            //           List<SqlParam> paramLst = new List<SqlParam>();
            //           paramLst.Add(new SqlParam("@USEORGID", KDDbType.Int64, Convert.ToInt64(material.UseOrgId)));
            //           paramLst.Add(new SqlParam("@DOCUMENTSTATUS", KDDbType.String, "C"));
            //           paramLst.Add(new SqlParam("@FORBIDSTATUS", KDDbType.String, "A"));
           
            string strSql = string.Format(@"SELECT TOP 100 MAT.FNUMBER,MATL.FNAME,MATL.FSPECIFICATION
             FROM T_BD_MATERIAL MAT
            INNER JOIN T_BD_MATERIAL_L MATL
               ON MAT.FMATERIALID = MATL.FMATERIALID
            WHERE MAT.FUSEORGID = {0}
              AND MAT.FDOCUMENTSTATUS = 'C'
              AND MAT.FFORBIDSTATUS = 'A'",Convert.ToInt64(material.UseOrgId));
            Kingdee.BOS.Log.Logger.Info("MaterialList_sql:", strSql);
            List<MaterialEntity> result = new List<MaterialEntity>();
            using (IDataReader dr = DBUtils.ExecuteReader(ctx, strSql))
            {
                while (dr.Read())
                {
                    result.Add(new MaterialEntity()
                    {
                        MaterialNum = Convert.ToString(dr["FNUMBER"]),
                        MaterialName = Convert.ToString(dr["FNAME"]),
                        MaterialSpec = Convert.ToString(dr["FSPECIFICATION"])
                    });
                }
            }
            return result;
        }
        
    }
}
