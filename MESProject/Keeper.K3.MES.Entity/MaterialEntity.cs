using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.K3.MES.Entity
{
    public class MaterialEntity
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialNum { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料规格型号
        /// </summary>
        public string MaterialSpec { get; set; }
        /// <summary>
        /// 使用组织ID
        /// </summary>
        public string UseOrgId { get; set; }
    }
}
