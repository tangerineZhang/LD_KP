using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LD.DAL;
using System.Data;
using System.Configuration;
using Newtonsoft.Json.Linq;
using LD_KP.Model;
using LDDC.DAL;
using LD.Service;
using BlueThink.Comm;
using Newtonsoft.Json;

namespace LD_KP.Controllers
{
    public class KPController : Controller
    {
        //
        // GET: /Law/
        public ActionResult Index(string name, string editName)
        {
            PublicDao pPublicDao = new PublicDao();
           

            if (Request.QueryString["tokenType"] == null)
            {
                ApiHelper api = new ApiHelper();
                string accessToken = pPublicDao.GetCacheAccessToken();

                if (Request.QueryString["token"] != null)
                {
                    string token = Request.QueryString["token"].ToString();
                    //string token = "a2dffec4-9e70-4b17-86f8-982f0792bd85";
                    JObject seal = api.SelectNeibu(token);

                    EOPModel eOP = new EOPModel();
                    eOP.code = seal.Value<string>("code");
                    if (eOP.code == "0")
                    {
                        eOP.username = seal["data"]["username"].ToString();
                        eOP.nickname = seal["data"]["nickname"].ToString();
                        ViewData["GetToken"] = token;

                        HttpContext.Session["userID"] = eOP.username;
                        HttpContext.Session["userName"] = eOP.nickname;
                    }
                    else
                    {
                        eOP.username = "";
                        eOP.nickname = "";

                        HttpContext.Session["userID"] = eOP.username;
                        HttpContext.Session["userName"] = eOP.nickname;
                        return View("Error");
                    }
                }
                else
                {
                    return View("Index");
                }
            }
            else
            {
                SSH();
            }

            #region MyRegion


            //if (Request.QueryString["code"] != null && Request.QueryString["code"] != String.Empty)
            //{
            //    string code = Request.QueryString["code"].ToString();

            //    ViewData["code"] = "code:" + Request.QueryString["code"].ToString();
            //    ViewData["code"] += "accessToken:" + accessToken + "</br>";

            //    string getjson = String.Empty;
            //    LD.DAL.PublicDao.WXUser OAuthUser_Model = pPublicDao.Get_UserInfo(accessToken, code, ref getjson);
            //    ViewData["code"] += " GetJson:" + getjson;
            //    ViewData["code"] += " UserId:" + OAuthUser_Model.UserId + " DeviceId:" + OAuthUser_Model.DeviceId;
            //    if (OAuthUser_Model.UserId != null && OAuthUser_Model.UserId != "")  //已获取得openid及其他信息
            //    {
            //        //CacheHelper.SetCache("userid", OAuthUser_Model.UserId);
            //        Cookies.SetUserCookie("", "", "", "", OAuthUser_Model.UserId, "", "");

            //        //  div1.InnerText += "b ";
            //        //在页面上输出用户信息
            //        ViewData["code"] += "成员UserID :" + OAuthUser_Model.UserId + "</br>手机设备号:" + OAuthUser_Model.DeviceId + "</br>";

            //    }
            //    else  //未获得openid，回到wxProcess.aspx，访问弹出微信授权页面，提示用户授权
            //    {
            //        // div1.InnerText += "c ";
            //        //Response.Redirect("wx1.aspx?auth=1");
            //    }
            //}
            //else
            //{
            //    ViewData["code"] = "code:没有数据";
            //}
            #endregion


            Users pUsers = new Users();
             string userid = HttpContext.Session["userID"].ToString();

         //   string userid = "zhangwdc";
            BillingDao pBillingDao = new BillingDao();

            //收藏or取消
            //if (editName != String.Empty && editName != null)
            //{
            //    //Login();
            //    int rCount = 0;
            //    string errorInfo = String.Empty;
            //    pBillingDao.EditCollect(editName, userid, ref rCount, ref errorInfo);
            //}

            //绑定发票抬头表
            DataSet ds = new DataSet();
            ds = pBillingDao.GetBillingList(name, userid);
            //ds = pBillingDao.GetBillingList(name, "zhangwdc");

            DataTable dt = new DataTable();
            DataView dataView = ds.Tables[0].DefaultView;
            dataView.Sort = "dsysDate desc";
            dt = dataView.ToTable();

            ViewData["dsBilling"] = dt;



            //微信Config绑定
            //WXConfig();

            return View();
        }


        public ActionResult Click(string name, string editName)
        {
           string userid= HttpContext.Session["userID"].ToString();
           // string userid = "zhangwdc";
            BillingDao pBillingDao = new BillingDao();
            //收藏or取消
            if (editName != String.Empty && editName != null)
            {
                //Login();
                int rCount = 0;
                string errorInfo = String.Empty;
                pBillingDao.EditCollect(editName, userid, ref rCount, ref errorInfo);
            }
            DataSet ds = new DataSet();
            ds = pBillingDao.GetBillingList(name, userid);
            DataTable dt = new DataTable();
            DataView dataView = ds.Tables[0].DefaultView;
            dataView.Sort = "dsysDate desc";
            dt = dataView.ToTable();

            ViewData["dsBilling"] = dt;
            return View("Index");
        }


        public ActionResult Login()
        {
        
            return View();
        }



        public ActionResult Info(string id, string name)
        {
            ViewData["id"] = id;
            ViewData["name"] = name;
            DataSet ds = new DataSet();
            DataSet dsP = new DataSet();
            BillingDao pBillingDao = new BillingDao();
            ds = pBillingDao.GetBillingList(name, "");
            DataTable dt = new DataTable();
            DataView dataView = ds.Tables[0].DefaultView;
            dataView.Sort = "dsysDate desc";
            dt = dataView.ToTable();
            ViewData["dsBillingInfo"] = dt;

            //绑定关联的项目表
            dsP = pBillingDao.GetBillingProjectList(id);
            //if (dsP.Tables[0].Rows.Count > 0)
            //{
            ViewData["dsProject"] = dsP.Tables[0];
            //}
            //else
            //{
            //    ViewData["dsProject"] = null;
            //}



            return View();
        }

        public ActionResult EditInfo(string id, string name)
        {
            ViewData["id"] = id;
            DataSet ds = new DataSet();
            DataSet dsP = new DataSet();
            BillingDao pBillingDao = new BillingDao();
            ds = pBillingDao.GetBillingList(name, "");
            DataTable dt = new DataTable();
            DataView dataView = ds.Tables[0].DefaultView;
            dataView.Sort = "dsysDate desc";
            dt = dataView.ToTable();
            ViewData["dsBillingInfo"] = dt;

            //绑定关联的项目表
            dsP = pBillingDao.GetBillingProjectList(id);
            ViewData["dsProject"] = dsP.Tables[0];

            return View();
        }

        public ActionResult EditPro(string id, string name)
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }


        public ActionResult EditInfo_onclick(string id, string ebCode, string name, string identifyNum, string phone, string bankAccount, string accountNum, string address)
        {
            int rCount = 0;
            string errorinfo = String.Empty;
            PublicDao pPublicDao = new PublicDao();
            BillingDao pBillingDao = new BillingDao();
            bool isb = true;
            DataSet ds = new DataSet();

            isb = pBillingDao.EditInfo(id, ebCode, name, identifyNum, phone, bankAccount, accountNum, address, ref rCount, ref errorinfo);
            if (isb == true)
            {
                return Content("提交成功！");
            }
            else
            {
                return Content("提交失败！");
            }
        }


        #region 单点登录
        public void SSH()
        {
            if (string.IsNullOrEmpty(Request["SysTokenResponse"]))   //主数据的单点登录
            {
                ResModel resp = GetSysRequestToken();
                if (resp.StatusCode == "200")
                {
                    string ssoLogInUrl = AuthLoginConfig.AuthSSO_SSOLoginUrl;
                    ssoLogInUrl += "?SysTokenRequest=" + resp.DATA;//通行证
                    Response.Redirect(ssoLogInUrl);
                }
            }
            else
            {
                string userId = GetUserInfoByToken(Request["SysTokenResponse"].ToString());
                if (!string.IsNullOrEmpty(userId))
                {
                    try
                    {
                        HttpContext.Session["userID"] = userId;
                    }
                    catch (Exception ex) { }
                }
            }
        }

        private string GetUserInfoByToken(string sysTokenResponse)
        {
            Auth client = new Auth();
            UserValidationSoapHeader header = new UserValidationSoapHeader();
            header.UserName = AuthLoginConfig.AuthSSO_HeaderUserName;
            header.PassWord = AuthLoginConfig.AuthSSO_HeaderPassword;
            ResModel res = JsonConvert.DeserializeObject<ResModel>(client.GetUserInfoByToken(sysTokenResponse));

            if (res != null && res.StatusCode == "200")
            {
                AuthUserInfo userInfo = JsonConvert.DeserializeObject<AuthUserInfo>(res.DATA);
                if (userInfo != null)
                {
                    HttpContext.Session["userID"] = userInfo.USERID;
                    return userInfo.USERID;
                }
            }
            return string.Empty;
        }
        public class JsonModel
        {
            public string UserID { get; set; }
        }

        /// <summary>
        /// 主数据的单点登录
        /// </summary>
        private ResModel GetSysRequestToken()
        {
            Auth client = new Auth();
            UserValidationSoapHeader header = new UserValidationSoapHeader();
            header.UserName = AuthLoginConfig.AuthSSO_HeaderUserName;
            header.PassWord = AuthLoginConfig.AuthSSO_HeaderPassword;
            string appId = AuthLoginConfig.AuthSSO_AppIDKP;
            string secretKey = AuthLoginConfig.AuthSSO_SecretKey;
            string receiveTokenUrl = AuthLoginConfig.AuthSSO_ReceiveTokenUrlKP;
            string sysTokenRequest_Json = client.GetSysRequestToken(appId, receiveTokenUrl, secretKey);
            ResModel res = JsonConvert.DeserializeObject<ResModel>(sysTokenRequest_Json);
            return res;
        }
        #endregion

    }
}