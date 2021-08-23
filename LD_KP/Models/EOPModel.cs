using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_KP.Model
{
   public class EOPModel
    {
        public JObject OriginInfo { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string result { get; set; }
        public string data { get; set; }
        public string id { get; set; }
        public string username { get; set; }    //用户账号
        public string nickname { get; set; }    //用户名字
        public string phone { get; set; }
    }
}
