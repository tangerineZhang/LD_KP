using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LD.DAL;
using System.Data;
using System.Configuration;
//using System.Xml;
//using System.Xml.Serialization;
//using System.Web.Script.Serialization
using BlueThink.Comm;
using LDDC.DAL;
using Newtonsoft.Json.Linq;
using LD_KP.Model;
using Newtonsoft.Json;
using LD.Service;

namespace LD_KP.Controllers
{
    public class YQController : Controller
    {
        //
        // GET: /Law/
        public ActionResult Index()
        {
    

            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        public ActionResult EmployeeList()
        {
            try
            {
                PublicDao pPublicDao = new PublicDao();
                if (Request.QueryString["tokenType"] == null)
                {
                    ApiHelper api = new ApiHelper();
                    if (HttpContext.Session["userIDs"] == null)
                    {
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

                                HttpContext.Session["userIDs"] = eOP.username;
                                HttpContext.Session["userName"] = eOP.nickname;
                            }
                            else
                            {
                                eOP.username = "";
                                eOP.nickname = "";

                                HttpContext.Session["userIDs"] = eOP.username;
                                HttpContext.Session["userName"] = eOP.nickname;
                                return View("Error");
                            }
                        }
                        else
                        {
                            return View("Error");
                        }
                    }
                }
                else
                {
                    SSH();
                }

                #region 
                //string accessToken = pPublicDao.GetCacheAccessTokenYQ();

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
                //  HttpContext.Session["userIDs"] = "zhang-h2";

                DataSet dsUser = new DataSet();
                //
                Users pUsers = new Users();
                // string userid = "lihana";

                string userid = HttpContext.Session["userIDs"].ToString();
                string Company = String.Empty;
               // Session["userid"] = HttpContext.Session["userIDs"].ToString();
                //发布需注释 beg------------------
                //userid = "zhangwdc";
                //Session["userid"] = userid;
                //发布需注释 end------------------
                pPublicDao = new PublicDao();
                dsUser = pPublicDao.GetUserInfo(userid);
                if (dsUser.Tables[0].Rows.Count > 0)
                {
                    Company = dsUser.Tables[0].Rows[0]["公司"].ToString();
                    ViewData["Company"] = dsUser.Tables[0].Rows[0]["公司"].ToString();
                    ViewData["Department"] = dsUser.Tables[0].Rows[0]["部门"].ToString();
                    ViewData["UserName"] = dsUser.Tables[0].Rows[0]["姓名"].ToString();
                    Session["UserName"] = dsUser.Tables[0].Rows[0]["姓名"].ToString();
                    Session["UserID"] = dsUser.Tables[0].Rows[0]["账号"].ToString();
                }

                DataSet ds = new DataSet();
                DataSet dsUntreated = new DataSet();
                DataSet dsWillVisit = new DataSet();
                DataSet dsVisited = new DataSet();
                DataSet dsAbnormal = new DataSet();

                DataSet dsPermission = new DataSet();
                pPublicDao = new PublicDao();
                //权限控制
                dsPermission = pPublicDao.GetPagePermission("where userID='" + userid + "' and Status=1", "");
                ViewData["dsPermission"] = dsPermission.Tables[0];
                if (Request.QueryString["page"] == "all")
                {
                    //if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "超级管理员")
                    //{
                    //    pPublicDao = new PublicDao();
                    //    ds = pPublicDao.GetEmployeeGroupByCustomerID("", "order by e.VisitDate desc");
                    //    ViewData["VisitorsCount"] = pPublicDao.GetVisitorsCount("");
                    //    ViewData["UnhealthyCount"] = pPublicDao.GetUnhealthyCount("");
                    //}
                    //else if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "事业部管理员")
                    //{
                    //    pPublicDao = new PublicDao();
                    //    ds = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Company] = '" + Company + "'", "order by e.VisitDate desc");
                    //    ViewData["VisitorsCount"] = pPublicDao.GetVisitorsCount("where e.[Company] = '" + Company + "'");
                    //    ViewData["UnhealthyCount"] = pPublicDao.GetUnhealthyCount(" e.[Company] = '" + Company + "'");
                    //}
                    //else if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "办公区管理员")
                    //{
                    //    pPublicDao = new PublicDao();
                    //    ds = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "'", "order by e.VisitDate desc");
                    //    ViewData["VisitorsCount"] = pPublicDao.GetVisitorsCount("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "'");
                    //    ViewData["UnhealthyCount"] = pPublicDao.GetUnhealthyCount(" e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "'");
                    //}

                    //new
                    if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "超级管理员")
                    {
                        //未处理来访数
                        pPublicDao = new PublicDao();
                        dsUntreated = new DataSet();
                        dsUntreated = pPublicDao.GetEmployeeGroupByCustomerID("where (v1.YesOrNo is null or v1.YesOrNo = 1 ) and ( v2.YesOrNo is null or v3.YesOrNo is null ) ", "order by e.VisitDate desc");
                        ViewData["Untreated"] = dsUntreated.Tables[0].Rows.Count;
                        //将来访人数
                        pPublicDao = new PublicDao();
                        dsWillVisit = new DataSet();
                        dsWillVisit = pPublicDao.GetEmployeeGroupByCustomerID("where e.VisitDate >= '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                        ViewData["WillVisit"] = dsWillVisit.Tables[0].Rows.Count;
                        //已来访人数
                        pPublicDao = new PublicDao();
                        dsVisited = new DataSet();
                        dsVisited = pPublicDao.GetEmployeeGroupByCustomerID("where e.VisitDate < '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                        ViewData["Visited"] = dsVisited.Tables[0].Rows.Count;
                        //来访异常人数
                        pPublicDao = new PublicDao();
                        dsAbnormal = new DataSet();
                        dsAbnormal = pPublicDao.GetEmployeeGroupByCustomerID("where v2.YesOrNo = 1 or v3.YesOrNo = 1 ", "order by e.VisitDate desc");
                        ViewData["Abnormal"] = dsAbnormal.Tables[0].Rows.Count;
                    }
                    else if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "事业部管理员")
                    {
                        //未处理来访数
                        pPublicDao = new PublicDao();
                        dsUntreated = new DataSet();
                        dsUntreated = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Company] = '" + Company + "' and (v1.YesOrNo is null or v1.YesOrNo = 1 ) and ( v2.YesOrNo is null or v3.YesOrNo is null ) ", "order by e.VisitDate desc");
                        ViewData["Untreated"] = dsUntreated.Tables[0].Rows.Count;
                        //将来访人数
                        pPublicDao = new PublicDao();
                        dsWillVisit = new DataSet();
                        dsWillVisit = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Company] = '" + Company + "' and e.VisitDate >= '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                        ViewData["WillVisit"] = dsWillVisit.Tables[0].Rows.Count;
                        //已来访人数
                        pPublicDao = new PublicDao();
                        dsVisited = new DataSet();
                        dsVisited = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Company] = '" + Company + "' and e.VisitDate < '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                        ViewData["Visited"] = dsVisited.Tables[0].Rows.Count;
                        //来访异常人数
                        pPublicDao = new PublicDao();
                        dsAbnormal = new DataSet();
                        dsAbnormal = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Company] = '" + Company + "' and v2.YesOrNo = 1 or v3.YesOrNo = 1 ", "order by e.VisitDate desc");
                        ViewData["Abnormal"] = dsAbnormal.Tables[0].Rows.Count;
                        
                    }
                    else if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "办公区管理员")
                    {
                        //未处理来访数
                        pPublicDao = new PublicDao();
                        dsUntreated = new DataSet();
                        dsUntreated = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "' and (v1.YesOrNo is null or v1.YesOrNo = 1 ) and ( v2.YesOrNo is null or v3.YesOrNo is null ) ", "order by e.VisitDate desc");
                        ViewData["Untreated"] = dsUntreated.Tables[0].Rows.Count;
                        //将来访人数
                        pPublicDao = new PublicDao();
                        dsWillVisit = new DataSet();
                        dsWillVisit = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "' and e.VisitDate >= '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                        ViewData["WillVisit"] = dsWillVisit.Tables[0].Rows.Count;
                        //已来访人数
                        pPublicDao = new PublicDao();
                        dsVisited = new DataSet();
                        dsVisited = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "' and e.VisitDate < '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                        ViewData["Visited"] = dsVisited.Tables[0].Rows.Count;
                        //来访异常人数
                        pPublicDao = new PublicDao();
                        dsAbnormal = new DataSet();
                        dsAbnormal = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "' and v2.YesOrNo = 1 or v3.YesOrNo = 1 ", "order by e.VisitDate desc");
                        ViewData["Abnormal"] = dsAbnormal.Tables[0].Rows.Count;
                    }
                }
                else
                {
                    pPublicDao = new PublicDao();
                    ds = pPublicDao.GetEmployeeGroupByCustomerID("where e.UserID ='" + userid + "' and (v1.YesOrNo is null or v1.YesOrNo = 1 ) and ( v2.YesOrNo is null or v3.YesOrNo is null )  ", "order by e.VisitDate asc");
                    //ds = pPublicDao.GetEmployeeGroupByCustomerID("where e.UserID ='" + userid + "' and e.VisitDate>='" + DateTime.Now.ToString("d") + " 00:00:00'", "order by e.VisitDate asc");
                }

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                if (Request.QueryString["page"] == "all" && Request.QueryString["type"] == "Untreated")
                {
                    ViewData["ds"] = dsUntreated.Tables[0];
                    ViewData["title"] = "未处理";
                }
                else if(Request.QueryString["page"] == "all" && Request.QueryString["type"] == "WillVisit")
                {
                    ViewData["ds"] = dsWillVisit.Tables[0];
                    ViewData["title"] = "将来访";
                }
                else if (Request.QueryString["page"] == "all" && Request.QueryString["type"] == "Visited")
                {
                    ViewData["ds"] = dsVisited.Tables[0];
                    ViewData["title"] = "已来访";
                }
                else if (Request.QueryString["page"] == "all" && Request.QueryString["type"] == "Abnormal")
                {
                    ViewData["ds"] = dsAbnormal.Tables[0];
                    ViewData["title"] = "来访异常";
                }
                else
                {
                    //普通用户起始页
                    ViewData["ds"] = ds.Tables[0];
                    ViewData["title"] = "来访处理";
                }
                
                //}


                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message.ToString());
            }
        }

        public ActionResult EmployRole()
        {
            PublicDao pPublicDao = new PublicDao();
            DataSet dsUser = new DataSet();
            //
            Users pUsers = new Users();
          //  string userid = "lihana";

             string userid = HttpContext.Session["userIDs"].ToString();
            string Company = String.Empty;
             Session["userid"] = HttpContext.Session["userIDs"].ToString();

            pPublicDao = new PublicDao();
            dsUser = pPublicDao.GetUserInfo(userid);
            if (dsUser.Tables[0].Rows.Count > 0)
            {
                Company = dsUser.Tables[0].Rows[0]["公司"].ToString();
                ViewData["Company"] = dsUser.Tables[0].Rows[0]["公司"].ToString();
                ViewData["Department"] = dsUser.Tables[0].Rows[0]["部门"].ToString();
                ViewData["UserName"] = dsUser.Tables[0].Rows[0]["姓名"].ToString();
                Session["UserName"] = dsUser.Tables[0].Rows[0]["姓名"].ToString();
                Session["UserID"] = dsUser.Tables[0].Rows[0]["账号"].ToString();
            }

            DataSet ds = new DataSet();
            DataSet dsUntreated = new DataSet();
            DataSet dsWillVisit = new DataSet();
            DataSet dsVisited = new DataSet();
            DataSet dsAbnormal = new DataSet();

            DataSet dsPermission = new DataSet();
            pPublicDao = new PublicDao();
            dsPermission = pPublicDao.GetPagePermission("where userID='" + userid + "' and Status=1", "");
            ViewData["dsPermission"] = dsPermission.Tables[0];
            if (Request.QueryString["page"] == "all")
            {
                //if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "超级管理员")
                //{
                //    pPublicDao = new PublicDao();
                //    ds = pPublicDao.GetEmployeeGroupByCustomerID("", "order by e.VisitDate desc");
                //    ViewData["VisitorsCount"] = pPublicDao.GetVisitorsCount("");
                //    ViewData["UnhealthyCount"] = pPublicDao.GetUnhealthyCount("");
                //}
                //else if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "事业部管理员")
                //{
                //    pPublicDao = new PublicDao();
                //    ds = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Company] = '" + Company + "'", "order by e.VisitDate desc");
                //    ViewData["VisitorsCount"] = pPublicDao.GetVisitorsCount("where e.[Company] = '" + Company + "'");
                //    ViewData["UnhealthyCount"] = pPublicDao.GetUnhealthyCount(" e.[Company] = '" + Company + "'");
                //}
                //else if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "办公区管理员")
                //{
                //    pPublicDao = new PublicDao();
                //    ds = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "'", "order by e.VisitDate desc");
                //    ViewData["VisitorsCount"] = pPublicDao.GetVisitorsCount("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "'");
                //    ViewData["UnhealthyCount"] = pPublicDao.GetUnhealthyCount(" e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "'");
                //}

                //new
                if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "超级管理员")
                {
                    //未处理来访数
                    pPublicDao = new PublicDao();
                    dsUntreated = new DataSet();
                    dsUntreated = pPublicDao.GetEmployeeGroupByCustomerID("where (v1.YesOrNo is null or v1.YesOrNo = 1 ) and ( v2.YesOrNo is null or v3.YesOrNo is null ) ", "order by e.VisitDate desc");
                    ViewData["Untreated"] = dsUntreated.Tables[0].Rows.Count;
                    //将来访人数
                    pPublicDao = new PublicDao();
                    dsWillVisit = new DataSet();
                    dsWillVisit = pPublicDao.GetEmployeeGroupByCustomerID("where e.VisitDate >= '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                    ViewData["WillVisit"] = dsWillVisit.Tables[0].Rows.Count;
                    //已来访人数
                    pPublicDao = new PublicDao();
                    dsVisited = new DataSet();
                    dsVisited = pPublicDao.GetEmployeeGroupByCustomerID("where e.VisitDate < '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                    ViewData["Visited"] = dsVisited.Tables[0].Rows.Count;
                    //来访异常人数
                    pPublicDao = new PublicDao();
                    dsAbnormal = new DataSet();
                    dsAbnormal = pPublicDao.GetEmployeeGroupByCustomerID("where v2.YesOrNo = 1 or v3.YesOrNo = 1 ", "order by e.VisitDate desc");
                    ViewData["Abnormal"] = dsAbnormal.Tables[0].Rows.Count;
                }
                else if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "事业部管理员")
                {
                    //未处理来访数
                    pPublicDao = new PublicDao();
                    dsUntreated = new DataSet();
                    dsUntreated = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Company] = '" + Company + "' and (v1.YesOrNo is null or v1.YesOrNo = 1 ) and ( v2.YesOrNo is null or v3.YesOrNo is null ) ", "order by e.VisitDate desc");
                    ViewData["Untreated"] = dsUntreated.Tables[0].Rows.Count;
                    //将来访人数
                    pPublicDao = new PublicDao();
                    dsWillVisit = new DataSet();
                    dsWillVisit = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Company] = '" + Company + "' and e.VisitDate >= '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                    ViewData["WillVisit"] = dsWillVisit.Tables[0].Rows.Count;
                    //已来访人数
                    pPublicDao = new PublicDao();
                    dsVisited = new DataSet();
                    dsVisited = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Company] = '" + Company + "' and e.VisitDate < '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                    ViewData["Visited"] = dsVisited.Tables[0].Rows.Count;
                    //来访异常人数
                    pPublicDao = new PublicDao();
                    dsAbnormal = new DataSet();
                    dsAbnormal = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Company] = '" + Company + "' and v2.YesOrNo = 1 or v3.YesOrNo = 1 ", "order by e.VisitDate desc");
                    ViewData["Abnormal"] = dsAbnormal.Tables[0].Rows.Count;

                }
                else if (dsPermission.Tables[0].Rows[0]["Permission"].ToString() == "办公区管理员")
                {
                    //未处理来访数
                    pPublicDao = new PublicDao();
                    dsUntreated = new DataSet();
                    dsUntreated = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "' and (v1.YesOrNo is null or v1.YesOrNo = 1 ) and ( v2.YesOrNo is null or v3.YesOrNo is null ) ", "order by e.VisitDate desc");
                    ViewData["Untreated"] = dsUntreated.Tables[0].Rows.Count;
                    //将来访人数
                    pPublicDao = new PublicDao();
                    dsWillVisit = new DataSet();
                    dsWillVisit = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "' and e.VisitDate >= '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                    ViewData["WillVisit"] = dsWillVisit.Tables[0].Rows.Count;
                    //已来访人数
                    pPublicDao = new PublicDao();
                    dsVisited = new DataSet();
                    dsVisited = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "' and e.VisitDate < '" + System.DateTime.Now.ToShortDateString() + "' ", "order by e.VisitDate desc");
                    ViewData["Visited"] = dsVisited.Tables[0].Rows.Count;
                    //来访异常人数
                    pPublicDao = new PublicDao();
                    dsAbnormal = new DataSet();
                    dsAbnormal = pPublicDao.GetEmployeeGroupByCustomerID("where e.[Address] = '" + dsPermission.Tables[0].Rows[0]["F1"].ToString() + "' and v2.YesOrNo = 1 or v3.YesOrNo = 1 ", "order by e.VisitDate desc");
                    ViewData["Abnormal"] = dsAbnormal.Tables[0].Rows.Count;
                }
            }
            else
            {
                pPublicDao = new PublicDao();
                ds = pPublicDao.GetEmployeeGroupByCustomerID("where e.UserID ='" + userid + "' and (v1.YesOrNo is null or v1.YesOrNo = 1 ) and ( v2.YesOrNo is null or v3.YesOrNo is null )  ", "order by e.VisitDate asc");
                //ds = pPublicDao.GetEmployeeGroupByCustomerID("where e.UserID ='" + userid + "' and e.VisitDate>='" + DateTime.Now.ToString("d") + " 00:00:00'", "order by e.VisitDate asc");
            }

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            if (Request.QueryString["page"] == "all" && Request.QueryString["type"] == "Untreated")
            {
                ViewData["ds"] = dsUntreated.Tables[0];
                ViewData["title"] = "未处理";
            }
            else if (Request.QueryString["page"] == "all" && Request.QueryString["type"] == "WillVisit")
            {
                ViewData["ds"] = dsWillVisit.Tables[0];
                ViewData["title"] = "将来访";
            }
            else if (Request.QueryString["page"] == "all" && Request.QueryString["type"] == "Visited")
            {
                ViewData["ds"] = dsVisited.Tables[0];
                ViewData["title"] = "已来访";
            }
            else if (Request.QueryString["page"] == "all" && Request.QueryString["type"] == "Abnormal")
            {
                ViewData["ds"] = dsAbnormal.Tables[0];
                ViewData["title"] = "来访异常";
            }
            else
            {
                //普通用户起始页
                ViewData["ds"] = ds.Tables[0];
                ViewData["title"] = "来访处理";
            }
            return View("EmployeeList");
        }

        public ActionResult EmployeeInfo()
        {
            PublicDao pPublicDao = new PublicDao();
            if (Request.QueryString["code"] != null && Request.QueryString["code"] != String.Empty)
            {
                ViewData["code"] = Request.QueryString["code"];
                string code = Request.QueryString["code"].ToString();
                DataSet ds = new DataSet();
                //ds = pPublicDao.GetEmployeeCustomer("where e.ID = '" + code + "'", "");
                ds = pPublicDao.GetEmployeeGroupByCustomerID("where e.ID = '" + code + "'", "");
                ViewData["ds"] = ds.Tables[0];
            }
            return View();
        }

        public ActionResult Employee()
        {
            PublicDao pPublicDao = new PublicDao();

            //string accessToken = pPublicDao.GetCacheAccessTokenYQ();

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



         //   Users pUsers = new Users();
            string userid = HttpContext.Session["userIDs"].ToString();
          // string userid ="lihana";
            userid = Session["userid"].ToString();

            DataSet ds = new DataSet();
            pPublicDao = new PublicDao();
            ds = pPublicDao.GetUserInfo(userid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewData["Company"] = ds.Tables[0].Rows[0]["公司"].ToString();
                ViewData["Department"] = ds.Tables[0].Rows[0]["部门"].ToString();
                ViewData["UserName"] = ds.Tables[0].Rows[0]["姓名"].ToString();
                Session["UserID"] = ds.Tables[0].Rows[0]["账号"].ToString();
            }
            //ViewData["Company"] = "";
            //ViewData["Department"] = "";
            //ViewData["UserName"] = "";

            return View();
        }

        public ActionResult CustomerBef()
        {
            string Code = Request.QueryString["Code"].ToString();
            Session["Code"] = Code;
            DataSet ds = new DataSet();
            PublicDao pPublicDao = new PublicDao();
            ds = pPublicDao.GetEmployee("WHERE ID='" + Code + "'", "Order by [CreateDate] desc");

            if (ds.Tables[0].Rows.Count > 0)
            {

                ViewData["VisitAddress"] = ds.Tables[0].Rows[0]["Address"].ToString();
            }


            return View();
        }

        public ActionResult Customer()
        {
            ViewData["VisitAddress"] = Request.QueryString["VisitAddress"].ToString();

            return View();
        }



        [HttpPost]
        public ActionResult GetAddress(string VisitCompany)
        {
            DataSet ds = new DataSet();
            PublicDao pPublicDao = new PublicDao();
            ////【1】调用模型处理业务
            //List<Student> stuList = new StudentManager().GetStudentsByClass(className);
            ////【2】JSON格式转换
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //string stringList = jss.Serialize(stuList);//将当前的List对象集合转换成字符串（JSON格式）
            ////【3】返回JSON格式数据 来自Http的所有Get方式都能响应
            //return Json(obj, JsonRequestBehavior.AllowGet);
            ds = pPublicDao.GetVisitAddress(VisitCompany);
            JSSerializer serializer = new JSSerializer();
            string stringList = serializer.Serialize(ds, false);
            stringList = stringList.Substring(1, stringList.Length - 2);

            return Json(stringList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddYQ(string company, string cCity, string fCity, string returnDate, string influence, string sProperties, string sScope,
         string isaffect, string projectInfo, string projectNode, string iDays, string describe, string applicant, string applicantTel)
        {
            int rCount = 0;
            string errorinfo = String.Empty;
            PublicDao pPublicDao = new PublicDao();
            bool isb = true;
            DataSet ds = new DataSet();

            ds = pPublicDao.GetIsRepeat(applicant);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return Content("您已参加过问卷，不能重复提交！");
            }

            isb = pPublicDao.TransactionAddYQ(company, cCity, fCity, returnDate, influence, sProperties, sScope,
                isaffect, projectInfo, projectNode, iDays, describe, applicant, applicantTel, ref rCount, ref errorinfo);
            if (isb == true)
            {
                return Content("提交成功！");
            }
            else
            {
                return Content("提交失败！");
            }
        }

        [HttpPost]
        public ActionResult AddEmployee(string Company, string Department, string UserName, string IsDeclare,
            string Reason, string VisitCompany, string Address, string VisitDate, string VisitTime,
            string NumVisitors, string SupplierName)
        {
            int rCount = 0;
            string errorinfo = String.Empty;
            string ID = String.Empty;
            string UserID = Session["UserID"].ToString();
            PublicDao pPublicDao = new PublicDao();
            bool isb = true;
            DataSet ds = new DataSet();
            //随机码-暂时不需要
            //VerificationCode pVerificationCode = new VerificationCode();
            //string randomNo = pVerificationCode.CreateValidateNumber(6);

            isb = pPublicDao.TransactionAddEmployee(Company, Department, UserName, UserID, IsDeclare,
             Reason, VisitCompany, Address, VisitDate, VisitTime,
             NumVisitors, SupplierName, ref ID, ref rCount, ref errorinfo);
            if (isb == true)
            {
                ViewData["ID"] = ID;
                return Content(ID);
            }
            else
            {
                return Content("提交失败！");
            }
        }

        [HttpPost]
        public ActionResult AddCustomer(string IsPromise, string Supplier, string CName, string Telephone,
            string IDNumber, string Temperature, string IsPass, string IsContact,
            string IsCoincidence, string IsSeeDoctor, string Isolated, string IsInform,
            string CurrentAddress, string VisitAddress, string Oexplain)
        {
            int rCount = 0;
            string errorinfo = String.Empty;
            PublicDao pPublicDao = new PublicDao();
            bool isb = true;
            DataSet ds = new DataSet();


            isb = pPublicDao.TransactionAddCustomer(Session["Code"].ToString(), IsPromise, Supplier, CName, Telephone,
                IDNumber, Temperature, IsPass, IsContact,
                IsCoincidence, IsSeeDoctor, Isolated, IsInform,
                CurrentAddress, VisitAddress, Oexplain, ref rCount, ref errorinfo);
            if (isb == true)
            {
                return Content("提交成功！");
            }
            else
            {
                return Content(errorinfo);
                //return Content("提交失败！");
            }
        }


        [HttpPost]
        public ActionResult SetVisitConfirmation(string EmployeeID, int Type, string Describe, int YesOrNo)
        {
            int rCount = 0;
            string errorinfo = String.Empty;
            PublicDao pPublicDao = new PublicDao();
            bool isb = true;
            DataSet ds = new DataSet();


            isb = pPublicDao.TransactionSetVisitConfirmation(EmployeeID, Type, Describe, YesOrNo, Session["UserID"].ToString(), Session["UserName"].ToString(), ref rCount, ref errorinfo);

            if (isb == true)
            {
                return Content("提交成功！");
            }
            else
            {
                return Content(errorinfo);
                //return Content("提交失败！");
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
                        HttpContext.Session["userIDs"] = userId;
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
                    HttpContext.Session["userIDs"] = userInfo.USERID;
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
            string appId = AuthLoginConfig.AuthSSO_AppID;
            string secretKey = AuthLoginConfig.AuthSSO_SecretKey;
            string receiveTokenUrl = AuthLoginConfig.AuthSSO_ReceiveTokenUrl;
            string sysTokenRequest_Json = client.GetSysRequestToken(appId, receiveTokenUrl, secretKey);
            ResModel res = JsonConvert.DeserializeObject<ResModel>(sysTokenRequest_Json);
            return res;
        }
        #endregion

    }
}