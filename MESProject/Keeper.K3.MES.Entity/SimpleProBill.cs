using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.K3.MES.Entity
{
    public class SimpleProBill
    {
        /// <summary>
        /// 验证用户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// K3Cloud站点地址
        /// </summary>
        public string K3CloudURI { get; set; }

        /// <summary>
        /// 数据中心ID
        /// </summary>
        public string DBCenterId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string CloudUserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        public List<SimpleProBillEntry> FEntity { get; set; }

    }
}
