﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace Test.localhostMes {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="MaterialMessageFromCappServiceServiceSoapBinding", Namespace="http://service.webservice.mes.lgcx.com/")]
    public partial class MaterialMessageFromCappServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback MaterialProcessMessageFromCappOperationCompleted;
        
        private System.Threading.SendOrPostCallback MaterialMessageFromCappOperationCompleted;
        
        private System.Threading.SendOrPostCallback StorageMessageFromKisOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MaterialMessageFromCappServiceService() {
            this.Url = global::Test.Properties.Settings.Default.Test_localhostMes_MaterialMessageFromCappServiceService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event MaterialProcessMessageFromCappCompletedEventHandler MaterialProcessMessageFromCappCompleted;
        
        /// <remarks/>
        public event MaterialMessageFromCappCompletedEventHandler MaterialMessageFromCappCompleted;
        
        /// <remarks/>
        public event StorageMessageFromKisCompletedEventHandler StorageMessageFromKisCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.mes.lgcx.com/", ResponseNamespace="http://service.webservice.mes.lgcx.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MaterialProcessMessageFromCapp([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0) {
            object[] results = this.Invoke("MaterialProcessMessageFromCapp", new object[] {
                        arg0});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void MaterialProcessMessageFromCappAsync(string arg0) {
            this.MaterialProcessMessageFromCappAsync(arg0, null);
        }
        
        /// <remarks/>
        public void MaterialProcessMessageFromCappAsync(string arg0, object userState) {
            if ((this.MaterialProcessMessageFromCappOperationCompleted == null)) {
                this.MaterialProcessMessageFromCappOperationCompleted = new System.Threading.SendOrPostCallback(this.OnMaterialProcessMessageFromCappOperationCompleted);
            }
            this.InvokeAsync("MaterialProcessMessageFromCapp", new object[] {
                        arg0}, this.MaterialProcessMessageFromCappOperationCompleted, userState);
        }
        
        private void OnMaterialProcessMessageFromCappOperationCompleted(object arg) {
            if ((this.MaterialProcessMessageFromCappCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.MaterialProcessMessageFromCappCompleted(this, new MaterialProcessMessageFromCappCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.mes.lgcx.com/", ResponseNamespace="http://service.webservice.mes.lgcx.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MaterialMessageFromCapp([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string process) {
            object[] results = this.Invoke("MaterialMessageFromCapp", new object[] {
                        process});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void MaterialMessageFromCappAsync(string process) {
            this.MaterialMessageFromCappAsync(process, null);
        }
        
        /// <remarks/>
        public void MaterialMessageFromCappAsync(string process, object userState) {
            if ((this.MaterialMessageFromCappOperationCompleted == null)) {
                this.MaterialMessageFromCappOperationCompleted = new System.Threading.SendOrPostCallback(this.OnMaterialMessageFromCappOperationCompleted);
            }
            this.InvokeAsync("MaterialMessageFromCapp", new object[] {
                        process}, this.MaterialMessageFromCappOperationCompleted, userState);
        }
        
        private void OnMaterialMessageFromCappOperationCompleted(object arg) {
            if ((this.MaterialMessageFromCappCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.MaterialMessageFromCappCompleted(this, new MaterialMessageFromCappCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.mes.lgcx.com/", ResponseNamespace="http://service.webservice.mes.lgcx.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string StorageMessageFromKis([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string processCheckInfo) {
            object[] results = this.Invoke("StorageMessageFromKis", new object[] {
                        processCheckInfo});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void StorageMessageFromKisAsync(string processCheckInfo) {
            this.StorageMessageFromKisAsync(processCheckInfo, null);
        }
        
        /// <remarks/>
        public void StorageMessageFromKisAsync(string processCheckInfo, object userState) {
            if ((this.StorageMessageFromKisOperationCompleted == null)) {
                this.StorageMessageFromKisOperationCompleted = new System.Threading.SendOrPostCallback(this.OnStorageMessageFromKisOperationCompleted);
            }
            this.InvokeAsync("StorageMessageFromKis", new object[] {
                        processCheckInfo}, this.StorageMessageFromKisOperationCompleted, userState);
        }
        
        private void OnStorageMessageFromKisOperationCompleted(object arg) {
            if ((this.StorageMessageFromKisCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.StorageMessageFromKisCompleted(this, new StorageMessageFromKisCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void MaterialProcessMessageFromCappCompletedEventHandler(object sender, MaterialProcessMessageFromCappCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MaterialProcessMessageFromCappCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal MaterialProcessMessageFromCappCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void MaterialMessageFromCappCompletedEventHandler(object sender, MaterialMessageFromCappCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MaterialMessageFromCappCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal MaterialMessageFromCappCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void StorageMessageFromKisCompletedEventHandler(object sender, StorageMessageFromKisCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class StorageMessageFromKisCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal StorageMessageFromKisCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591