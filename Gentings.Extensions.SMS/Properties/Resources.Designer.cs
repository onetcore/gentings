﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gentings.Extensions.SMS.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gentings.Extensions.SMS.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 移动 的本地化字符串。
        /// </summary>
        internal static string ServiceType_Mobile {
            get {
                return ResourceManager.GetString("ServiceType_Mobile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 未知 的本地化字符串。
        /// </summary>
        internal static string ServiceType_None {
            get {
                return ResourceManager.GetString("ServiceType_None", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 电信 的本地化字符串。
        /// </summary>
        internal static string ServiceType_Telecom {
            get {
                return ResourceManager.GetString("ServiceType_Telecom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 联通 的本地化字符串。
        /// </summary>
        internal static string ServiceType_Unicom {
            get {
                return ResourceManager.GetString("ServiceType_Unicom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 短信发送客户端不存在！ 的本地化字符串。
        /// </summary>
        internal static string SMSClientNotFound {
            get {
                return ResourceManager.GetString("SMSClientNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 成功发送 的本地化字符串。
        /// </summary>
        internal static string SmsStatus_Completed {
            get {
                return ResourceManager.GetString("SmsStatus_Completed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 发送失败 的本地化字符串。
        /// </summary>
        internal static string SmsStatus_Failured {
            get {
                return ResourceManager.GetString("SmsStatus_Failured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 等待发送 的本地化字符串。
        /// </summary>
        internal static string SmsStatus_Pending {
            get {
                return ResourceManager.GetString("SmsStatus_Pending", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 发送中 的本地化字符串。
        /// </summary>
        internal static string SmsStatus_Sending {
            get {
                return ResourceManager.GetString("SmsStatus_Sending", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 短信发送服务 的本地化字符串。
        /// </summary>
        internal static string SMSTaskService {
            get {
                return ResourceManager.GetString("SMSTaskService", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 发送短信相关服务 的本地化字符串。
        /// </summary>
        internal static string SMSTaskService_Description {
            get {
                return ResourceManager.GetString("SMSTaskService_Description", resourceCulture);
            }
        }
    }
}
