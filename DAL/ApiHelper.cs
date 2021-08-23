using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LDDC.DAL
{
   public class ApiHelper
    {

        public static readonly string url = ConfigurationManager.AppSettings["Url"];
        /// <summary>
        /// eop单点登录(通过eop的token获取用户信息)
        /// </summary>
        /// <param name="Nameid"></param>
        /// <returns></returns>


      

        public JObject SelectNeibu (object Nameid)
        {
            string strurl = $"{url}/token/user?token={Nameid}";
            HttpClient client = new HttpClient();
            HttpResponseMessage httpResponse = client.GetAsync(strurl).Result;
            string result = httpResponse.Content.ReadAsStringAsync().Result;
            return (JsonConvert.DeserializeObject<JObject>(result));
        }
    }
}
