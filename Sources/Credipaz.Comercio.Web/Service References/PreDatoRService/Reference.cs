﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Credipaz.Comercio.Web.PreDatoRService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PreDatoRService.IPreDatoRService")]
    public interface IPreDatoRService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreDatoRService/DataToCheck")]
        void DataToCheck(System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>> data, int idApp);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IPreDatoRService/DataToCheck")]
        System.Threading.Tasks.Task DataToCheckAsync(System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>> data, int idApp);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPreDatoRService/OneDataToCheck", ReplyAction="http://tempuri.org/IPreDatoRService/OneDataToCheckResponse")]
        string OneDataToCheck(System.Collections.Generic.Dictionary<string, string> data, int idApp);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPreDatoRService/OneDataToCheck", ReplyAction="http://tempuri.org/IPreDatoRService/OneDataToCheckResponse")]
        System.Threading.Tasks.Task<string> OneDataToCheckAsync(System.Collections.Generic.Dictionary<string, string> data, int idApp);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPreDatoRServiceChannel : Credipaz.Comercio.Web.PreDatoRService.IPreDatoRService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PreDatoRServiceClient : System.ServiceModel.ClientBase<Credipaz.Comercio.Web.PreDatoRService.IPreDatoRService>, Credipaz.Comercio.Web.PreDatoRService.IPreDatoRService {
        
        public PreDatoRServiceClient() {
        }
        
        public PreDatoRServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PreDatoRServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PreDatoRServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PreDatoRServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void DataToCheck(System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>> data, int idApp) {
            base.Channel.DataToCheck(data, idApp);
        }
        
        public System.Threading.Tasks.Task DataToCheckAsync(System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>> data, int idApp) {
            return base.Channel.DataToCheckAsync(data, idApp);
        }
        
        public string OneDataToCheck(System.Collections.Generic.Dictionary<string, string> data, int idApp) {
            return base.Channel.OneDataToCheck(data, idApp);
        }
        
        public System.Threading.Tasks.Task<string> OneDataToCheckAsync(System.Collections.Generic.Dictionary<string, string> data, int idApp) {
            return base.Channel.OneDataToCheckAsync(data, idApp);
        }
    }
}
