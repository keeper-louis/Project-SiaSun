using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.ServiceInterface.ClientSDK;
using ServiceStack.ServiceInterface.ServiceModel;

namespace ConsoleApplication1
{
    /// <summary>
    /// 库存单查看请求参数
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "STK_InStock_View", Namespace = "http://schema.kingdee.com/servicetypes")]
    [System.SerializableAttribute()]
    public class STK_InStock_View
    {
        [DataMember]
        public int CreateOrgId { get; set; }
        [DataMember]
        public string Number { get; set; }
    }
    /// <summary>
    /// 库存单查看返回参数
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "STK_InStock_ViewResponse", Namespace = "http://schema.kingdee.com/servicetypes")]
    [System.SerializableAttribute()]
    public class STK_InStock_ViewResponse : ViewDTOBaseResponse<STK_InStock>
    {
    }
    /// <summary>
    /// 部门保存请求参数
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BD_Department_Save", Namespace = "http://schema.kingdee.com/servicetypes")]
    [System.SerializableAttribute()]
    public class BD_Department_Save : SaveDTOBase<BD_Department>
    {
    }
    /// <summary>
    /// 部门保存返回参数
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BD_Department_SaveResponse", Namespace = "http://schema.kingdee.com/servicetypes")]
    [System.SerializableAttribute()]
    public class BD_Department_SaveResponse : DTOBaseResponse
    {
    }
    /// <summary>
    /// 部门删除请求参数
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BD_Department_Delete", Namespace = "http://schema.kingdee.com/servicetypes")]
    [System.SerializableAttribute()]
    public class BD_Department_Delete : DeleteDTOBase
    {
    }
    /// <summary>
    /// 部门删除返回参数
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "BD_Department_DeleteResponse", Namespace = "http://schema.kingdee.com/servicetypes")]
    [System.SerializableAttribute()]
    public class BD_Department_DeleteResponse : DTOBaseResponse
    {
    }
    // 自定义空操作参数
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PUR_ReceiveBill_TakeReferencePrice", Namespace = "http://schema.kingdee.com/servicetypes")]
    [System.SerializableAttribute()]
    public class PUR_ReceiveBill_TakeReferencePrice
    {
        [DataMember]
        public string Parameters { get; set; }
        [DataMember]
        public PUR_ReceiveBill Model { get; set; }
    }
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PUR_ReceiveBill_DoNothingResponse", Namespace = "http://schema.kingdee.com/servicetypes")]
    [System.SerializableAttribute()]
    public class PUR_ReceiveBill_DoNothingResponse : DTOBaseResponse
    {
    }
    //class Class1
    //{
    //}
}
