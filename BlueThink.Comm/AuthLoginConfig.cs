using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BlueThink.Comm
{
    public class AuthLoginConfig
    {

        /// <summary>
        /// 单点登录地址
        /// </summary>
        public static string AuthSSO_SSOLoginUrl
        {
            get
            {
                if (ConfigurationManager.AppSettings["AuthSSO_SSOLoginUrl"] != null)
                {
                    return ConfigurationManager.AppSettings["AuthSSO_SSOLoginUrl"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 单点登录系统返回地址
        /// </summary>
        public static string AuthSSO_ReceiveTokenUrl
        {
            get
            {
                if (ConfigurationManager.AppSettings["AuthSSO_ReceiveTokenUrl"] != null)
                {
                    return ConfigurationManager.AppSettings["AuthSSO_ReceiveTokenUrl"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string AuthSSO_ReceiveTokenUrlKP
        {
            get
            {
                if (ConfigurationManager.AppSettings["AuthSSO_ReceiveTokenUrlKP"] != null)
                {
                    return ConfigurationManager.AppSettings["AuthSSO_ReceiveTokenUrlKP"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 单点登录用户名
        /// </summary>
        public static string AuthSSO_HeaderUserName
        {
            get
            {
                if (ConfigurationManager.AppSettings["AuthSSO_HeaderUserName"] != null)
                {
                    return ConfigurationManager.AppSettings["AuthSSO_HeaderUserName"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 单点登录密钥
        /// </summary>
        public static string AuthSSO_HeaderPassword
        {
            get
            {
                if (ConfigurationManager.AppSettings["AuthSSO_HeaderPassword"] != null)
                {
                    return ConfigurationManager.AppSettings["AuthSSO_HeaderPassword"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 单点登录应用池
        /// </summary>
        public static string AuthSSO_AppID
        {
            get
            {
                if (ConfigurationManager.AppSettings["AuthSSO_AppID"] != null)
                {
                    return ConfigurationManager.AppSettings["AuthSSO_AppID"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string AuthSSO_AppIDKP
        {
            get
            {
                if (ConfigurationManager.AppSettings["AuthSSO_AppIDKP"] != null)
                {
                    return ConfigurationManager.AppSettings["AuthSSO_AppIDKP"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string AuthSSO_SecretKey
        {
            get
            {
                if (ConfigurationManager.AppSettings["AuthSSO_SecretKey"] != null)
                {
                    return ConfigurationManager.AppSettings["AuthSSO_SecretKey"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }

    public class ResModel
    {
        /// <summary>
        /// 状态码，200-成功，500-失败
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 消息 失败时会有错误信息
        /// </summary>
        public string MSG { get; set; }

        /// <summary>
        /// 通行证
        /// </summary>
        public string DATA { get; set; }
    }

    public class AuthUserInfo
    {
        public string USERID { get; set; }
    }
}
