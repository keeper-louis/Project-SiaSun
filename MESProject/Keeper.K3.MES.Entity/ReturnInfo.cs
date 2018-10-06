using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.K3.MES.Entity
{
    /// <summary>
    /// 返回结果
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class ReturnInfo<T>
    {
        public ReturnInfo()
        {

        }

        public ReturnInfo(T t)
        {
            this.ReturnValue = t;
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 返回值节点数
        /// </summary>
        public int ListCount { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回值
        /// </summary>
        public T ReturnValue { get; set; }
    }
}
