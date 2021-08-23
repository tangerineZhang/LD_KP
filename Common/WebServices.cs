namespace LD.Service {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="AuthSoap", Namespace="http://tempuri.org/")]
    public partial class Auth : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private UserValidationSoapHeader userValidationSoapHeaderValueField;
        
        private System.Threading.SendOrPostCallback GetUserInfoByTokenOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetSysRequestTokenOperationCompleted;
        
        /// <remarks/>
        public Auth() {
            string urlSetting = System.Configuration.ConfigurationManager.AppSettings["WebServiceBaseUrl"];
            if ((urlSetting != null)) {
                this.Url = string.Concat(urlSetting, "Service/Auth.asmx");
            }
            else {
                this.Url = "http://zsjtest.lvdu-dc.com:83/Service/Auth.asmx";
            }
        }
        
        public UserValidationSoapHeader UserValidationSoapHeaderValue {
            get {
                return this.userValidationSoapHeaderValueField;
            }
            set {
                this.userValidationSoapHeaderValueField = value;
            }
        }
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        public event GetUserInfoByTokenCompletedEventHandler GetUserInfoByTokenCompleted;
        
        /// <remarks/>
        public event GetSysRequestTokenCompletedEventHandler GetSysRequestTokenCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld() {
            object[] results = this.Invoke("HelloWorld", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginHelloWorld(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("HelloWorld", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public string EndHelloWorld(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("UserValidationSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetUserInfoByToken", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetUserInfoByToken(string Token) {
            object[] results = this.Invoke("GetUserInfoByToken", new object[] {
                        Token});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetUserInfoByToken(string Token, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetUserInfoByToken", new object[] {
                        Token}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGetUserInfoByToken(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetUserInfoByTokenAsync(string Token) {
            this.GetUserInfoByTokenAsync(Token, null);
        }
        
        /// <remarks/>
        public void GetUserInfoByTokenAsync(string Token, object userState) {
            if ((this.GetUserInfoByTokenOperationCompleted == null)) {
                this.GetUserInfoByTokenOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetUserInfoByTokenOperationCompleted);
            }
            this.InvokeAsync("GetUserInfoByToken", new object[] {
                        Token}, this.GetUserInfoByTokenOperationCompleted, userState);
        }
        
        private void OnGetUserInfoByTokenOperationCompleted(object arg) {
            if ((this.GetUserInfoByTokenCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetUserInfoByTokenCompleted(this, new GetUserInfoByTokenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("UserValidationSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetSysRequestToken", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetSysRequestToken(string APPID, string ReceiveTokenUrl, string SecretKey) {
            object[] results = this.Invoke("GetSysRequestToken", new object[] {
                        APPID,
                        ReceiveTokenUrl,
                        SecretKey});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetSysRequestToken(string APPID, string ReceiveTokenUrl, string SecretKey, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetSysRequestToken", new object[] {
                        APPID,
                        ReceiveTokenUrl,
                        SecretKey}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGetSysRequestToken(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetSysRequestTokenAsync(string APPID, string ReceiveTokenUrl, string SecretKey) {
            this.GetSysRequestTokenAsync(APPID, ReceiveTokenUrl, SecretKey, null);
        }
        
        /// <remarks/>
        public void GetSysRequestTokenAsync(string APPID, string ReceiveTokenUrl, string SecretKey, object userState) {
            if ((this.GetSysRequestTokenOperationCompleted == null)) {
                this.GetSysRequestTokenOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetSysRequestTokenOperationCompleted);
            }
            this.InvokeAsync("GetSysRequestToken", new object[] {
                        APPID,
                        ReceiveTokenUrl,
                        SecretKey}, this.GetSysRequestTokenOperationCompleted, userState);
        }
        
        private void OnGetSysRequestTokenOperationCompleted(object arg) {
            if ((this.GetSysRequestTokenCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetSysRequestTokenCompleted(this, new GetSysRequestTokenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/", IsNullable=false)]
    public partial class UserValidationSoapHeader : System.Web.Services.Protocols.SoapHeader {
        
        private string userNameField;
        
        private string passWordField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        public string UserName {
            get {
                return this.userNameField;
            }
            set {
                this.userNameField = value;
            }
        }
        
        /// <remarks/>
        public string PassWord {
            get {
                return this.passWordField;
            }
            set {
                this.passWordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    public delegate void GetUserInfoByTokenCompletedEventHandler(object sender, GetUserInfoByTokenCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetUserInfoByTokenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetUserInfoByTokenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    public delegate void GetSysRequestTokenCompletedEventHandler(object sender, GetSysRequestTokenCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetSysRequestTokenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetSysRequestTokenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
