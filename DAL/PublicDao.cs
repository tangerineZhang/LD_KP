using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using HL.DAL;
using System.Configuration;

using BlueThink.Comm;
using System.Configuration;
using DotNet.Utilities;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;

namespace LD.DAL
{
    /// <summary>
    /// 数据访问类:LotInfo
    /// </summary>
    public partial class PublicDao
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        public PublicDao()
        { }
        #region  BasicMethod


        #endregion  BasicMethod
        #region  ExtensionMethod

        //数据库连接字符串(web.config来配置)，多数据库可使用DbHelperSQLP来实现.
        /// <summary>
        /// 确定
        /// </summary>    

        public string AccessToken()
        {
            string AccessToken = string.Empty;
            string CorpID = ConfigurationManager.AppSettings["appid"];
            string Secret = ConfigurationManager.AppSettings["appsecret"];

            AccessToken = GetAccessToken(CorpID, Secret);

            return AccessToken;
        }

        public string AccessToken(string CorpID, string Secret)
        {
            string AccessToken = string.Empty;

            AccessToken = GetAccessToken(CorpID, Secret);

            return AccessToken;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="corpid"></param>
        /// <param name="corpsecret"></param>
        /// <returns></returns>
        private string GetAccessToken(string corpid, string corpsecret)
        {
            string Gurl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", corpid, corpsecret);
            string html = HttpHelper.HttpGet(Gurl, "");
            string regex = "\"access_token\":\"(?<token>.*?)\"";

            string token = CRegex.GetText(html, regex, "token");
            return token;
        }

        public string GetCacheAccessToken()
        {
            string token = String.Empty;
            if (CacheHelper.GetCache("wxAccessToken") == String.Empty || CacheHelper.GetCache("wxAccessToken") == null)
            {
                token = AccessToken();
                TimeSpan ts = new TimeSpan(1, 30, 0);
                CacheHelper.SetCache("wxAccessToken", token, ts);
            }
            else
            {
                token = CacheHelper.GetCache("wxAccessToken").ToString();
            }

            return token;
        }

        public string GetCacheAccessTokenYQ()
        {
            string token = String.Empty;
            if (CacheHelper.GetCache("wxAccessTokenYQ") == String.Empty || CacheHelper.GetCache("wxAccessTokenYQ") == null)
            {
                token = AccessTokenYQ();
                TimeSpan ts = new TimeSpan(1, 30, 0);
                CacheHelper.SetCache("wxAccessTokenYQ", token, ts);
            }
            else
            {
                token = CacheHelper.GetCache("wxAccessTokenYQ").ToString();
            }

            return token;
        }

        public string AccessTokenYQ()
        {
            string AccessToken = string.Empty;
            string CorpID = ConfigurationManager.AppSettings["appid"];
            string Secret = ConfigurationManager.AppSettings["appsecretYQ"];

            AccessToken = GetAccessToken(CorpID, Secret);

            return AccessToken;
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <param name="corpid"></param>
        /// <param name="corpsecret"></param>
        /// <returns></returns>
        public string GetJsapi_ticket(string accesstoken)
        {
            string Gurl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/get_jsapi_ticket?access_token={0}", accesstoken);
            string html = HttpHelper.HttpGet(Gurl, "");
            string regex = "\"ticket\":\"(?<token>.*?)\"";

            string token = CRegex.GetText(html, regex, "token");
            return token;
        }

        public string GetCachewxTicket(string appId)
        {
            string token = String.Empty;
            if (CacheHelper.GetCache("wxTicket") == String.Empty || CacheHelper.GetCache("wxTicket") == null)
            {
                token = GetJsapi_ticket(appId);
                //TimeSpan ts = new TimeSpan(0, 1, 30);
                TimeSpan ts = new TimeSpan(1, 30, 0);
                CacheHelper.SetCache("wxTicket", token, ts);
            }
            else
            {
                token = CacheHelper.GetCache("wxTicket").ToString();
            }


            return token;
        }


        //获取jssdk所需签名
        public string Signature(string ticket, string noncestr, string timestamp, string url, ref string jsapi_ticket)
        {
            //string noncestr = "Wm3WZYTPz0wzccnW";
            //int timestamp = 1414587457;
            //string ticket = GetTicket();

            string string1 = "jsapi_ticket=" + ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url;
            jsapi_ticket = string1;
            string signature = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(string1, "SHA1");
            return signature.ToLower();
        }


        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns>返回毫秒数</returns>
        public long GetTime()
        {
            return (DateTime.Now.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000;
        }

        //根据openid，access token获得用户信息
        public WXUser Get_UserInfo(string REFRESH_TOKEN, string OPENID, ref string getjson)
        {
            //string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + REFRESH_TOKEN + "&openid=" + OPENID);公众号接口
            string Str = GetJson("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token=" + REFRESH_TOKEN + "&code=" + OPENID);
            getjson = Str;
            //div1.InnerText += " <Get_UserInfo> " + Str + " <> ";

            WXUser OAuthUser_Model = JsonHelper.ParseFromJson<WXUser>(Str);
            return OAuthUser_Model;
        }

        //访问微信url并返回微信信息
        protected string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = System.Text.Encoding.UTF8;
            string returnText = wc.DownloadString(url);

            if (returnText.Contains("errcode"))
            {
                //可能发生错误
                return returnText;
            }
            return returnText;
        }

        public class WXUser
        {
            public WXUser()
            { }
            #region 数据库字段
            private string _UserId;
            private string _DeviceId;

            #endregion

            #region 字段属性
            /// <summary>
            /// 用户的唯一标识
            /// </summary>
            public string UserId
            {
                set { _UserId = value; }
                get { return _UserId; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string DeviceId
            {
                set { _DeviceId = value; }
                get { return _DeviceId; }
            }

            #endregion
        }

        #endregion  ExtensionMethod




        /// <summary>
        /// 添加疫情
        /// </summary>
        /// <param name="company"></param>
        /// <param name="cCity"></param>
        /// <param name="fCity"></param>
        /// <param name="returnDate"></param>
        /// <param name="influence"></param>
        /// <param name="sProperties"></param>
        /// <param name="sScope"></param>
        /// <param name="isaffect"></param>
        /// <param name="projectInfo"></param>
        /// <param name="projectNode"></param>
        /// <param name="iDays"></param>
        /// <param name="describe"></param>
        /// <param name="applicant"></param>
        /// <param name="applicantTel"></param>
        /// <param name="rCount"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public bool TransactionAddYQ(string company, string cCity, string fCity, string returnDate, string influence, string sProperties, string sScope,
         string isaffect, string projectInfo, string projectNode, string iDays, string describe, string applicant, string applicantTel, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {

                strSql = new StringBuilder();

                strSql.Append("insert into [LDXX].[dbo].[YQInfo] (company,cCity,fCity,returnDate,influence,sProperties,");
                strSql.Append("sScope,isaffect,projectInfo,projectNode,iDays,describe,applicant,applicantTel) ");
                strSql.Append("Values('" + company + "','" + cCity + "','" + fCity + "','" + returnDate + "','" + influence + "','" + sProperties + "',");
                strSql.Append("'" + sScope + "','" + isaffect + "','" + projectInfo + "','" + projectNode + "'," + iDays + ",'" + describe + "','" + applicant + "','" + applicantTel + "')");

                comm.CommandText = strSql.ToString();
                //comm.Parameters.Clear();
                //comm.Parameters.AddRange(parametersDM);
                rCount += comm.ExecuteNonQuery();

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }

        public bool TransactionAddEmployee(string Company, string Department, string UserName, string UserID, string IsDeclare,
            string Reason, string VisitCompany, string Address, string VisitDate, string VisitTime,
            string NumVisitors, string SupplierName, ref string ID, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {

                strSql = new StringBuilder();
                strSql.Append("select NEWID() ");
                comm.CommandText = strSql.ToString();
                ID = comm.ExecuteScalar().ToString();

                strSql = new StringBuilder();
                strSql.Append("insert into [LDXX].[dbo].[YQ_EmployeeDeclaration] (ID,Company,Department,UserName,UserID,IsDeclare,Reason,VisitCompany,Address,VisitDate,");
                strSql.Append("BegVisitTime,EndVisitTime,VisitTime,NumVisitors,SupplierName,SortID,CreatorID,Creator) ");
                strSql.Append("Values('" + ID + "','" + Company + "','" + Department + "','" + UserName + "','" + UserID + "','" + IsDeclare + "','" + Reason + "','" + VisitCompany + "','" + Address + "','" + VisitDate + "',");
                strSql.Append("'','','" + VisitTime + "'," + NumVisitors + ",'" + SupplierName + "',null,'','')");

                comm.CommandText = strSql.ToString();
                //comm.Parameters.Clear();
                //comm.Parameters.AddRange(parametersDM);
                rCount += comm.ExecuteNonQuery();

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }


        public bool TransactionAddCustomer(string EmployeeID, string IsPromise, string Supplier, string CName, string Telephone,
             string IDNumber, string Temperature, string IsPass, string IsContact,
            string IsCoincidence, string IsSeeDoctor, string Isolated, string IsInform,
            string CurrentAddress, string VisitAddress, string Oexplain, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {

                strSql = new StringBuilder();

                strSql.Append("insert into [LDXX].[dbo].[YQ_CustomerDeclaration] (EmployeeID,IsPromise,Supplier,CName,Telephone,IDNumber,Temperature,IsPass,IsContact,");
                strSql.Append("IsCoincidence,IsSeeDoctor,Isolated,IsInform,CurrentAddress,VisitAddress,Oexplain,SortID,CreatorID,Creator) ");
                strSql.Append("Values('" + EmployeeID + "','" + IsPromise + "','" + Supplier + "','" + CName + "','" + Telephone + "','" + IDNumber + "','" + Temperature + "','" + IsPass + "','" + IsContact + "',");
                strSql.Append("'" + IsCoincidence + "','" + IsSeeDoctor + "','" + Isolated + "','" + IsInform + "','" + CurrentAddress + "','" + VisitAddress + "','" + Oexplain + "',null,'','')");

                comm.CommandText = strSql.ToString();
                //comm.Parameters.Clear();
                //comm.Parameters.AddRange(parametersDM);
                rCount += comm.ExecuteNonQuery();

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }

        public bool TransactionSetVisitConfirmation(string EmployeeID, int Type, string Describe, int YesOrNo, string CreatorID, string Creator, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {

                strSql = new StringBuilder();
                strSql.Append("insert into  [LDXX].[dbo].[YQ_VisitConfirmation]([EmployeeID],[Type],[Describe],[YesOrNo],[SortID],[Status],[CreatorID],[Creator]) ");
                strSql.Append("values('" + EmployeeID + "'," + Type + ",'" + Describe + "'," + YesOrNo + ",null,1,'" + CreatorID + "','" + Creator + "')");

                comm.CommandText = strSql.ToString();
                //comm.Parameters.Clear();
                //comm.Parameters.AddRange(parametersDM);
                rCount += comm.ExecuteNonQuery();

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }


        /// <summary>
        /// 验证是否已经填报
        /// </summary>
        /// <param name="rGuid"></param>
        /// <param name="begDate"></param>
        /// <param name="endDate"></param>
        /// <param name="vtid"></param>
        /// <returns></returns>
        public DataSet GetIsRepeat(string applicant)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [LDXX].[dbo].[YQInfo] where applicant = '" + applicant + "'");
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetUserInfo(string usercode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CASE WHEN ISNULL(u.F3,'')<>'' THEN u.F3 ELSE u.UserCode END as 工号, ");
            strSql.Append("CASE WHEN ISNULL(substring(substring(o.F3,charindex('/',o.F3)+1,len(o.F3)-charindex('/',o.F3)),0,charindex('/',substring(o.F3,charindex('/',o.F3)+1,len(o.F3)-charindex('/',o.F3)))),'')<>'' THEN ");
            strSql.Append("substring(substring(o.F3,charindex('/',o.F3)+1,len(o.F3)-charindex('/',o.F3)),0,charindex('/',substring(o.F3,charindex('/',o.F3)+1,len(o.F3)-charindex('/',o.F3)))) ");
            strSql.Append("else substring(o.F3,charindex('/',o.F3)+1,len(o.F3)) ");
            strSql.Append("END as 公司,o.F3 as 组织全路径,");
            strSql.Append("CASE WHEN o.OrgUnitLever>4 THEN substring(substring(substring(o.F3,charindex('/',o.F3)+1,len(o.F3)),charindex('/',substring(o.F3,charindex('/',o.F3)+1,len(o.F3)))+1,len(o.F3)),0,charindex('/',substring(substring(o.F3,charindex('/',o.F3)+1,len(o.F3)),charindex('/',substring(o.F3,charindex('/',o.F3)+1,len(o.F3)))+1,len(o.F3)))) ");
            strSql.Append("ELSE CASE WHEN o.OrgUnitLever=4 THEN ISNULL(o.F2,o.OrgUnitName) ELSE NULL END END as 中心,o.OrgUnitName as 部门 , ");
            strSql.Append("p.PositionName as 岗位,u.RankName as 职级编号, ");
            strSql.Append("u.UserName as 姓名,u.UserLoginID as 账号,u.GenderText as 性别,u.MobilePhone as 手机,u.Email as 邮箱,CONVERT(VARCHAR(10), u.BirthDay, 120)  as 生日 ");
            strSql.Append("from dbo.MDM_User u ");
            strSql.Append("inner join dbo.MDM_User_Position_Link up on up.UserGUID=u.UserID and up.IsMainPosition=1 and up.Status=1 ");
            strSql.Append("inner join dbo.MDM_PostOrganization_Link po on po.PositionGuid=up.PositionGUID and po.Status=1 ");
            strSql.Append("inner join dbo.MDM_OrganizationUnit o on o.OrgUnitGUID = po.OrgUnitGUID and o.Status=1 ");
            strSql.Append("left join dbo.MDM_OrganizationUnit q on q.OrgUnitGUID = po.OrgUnitGUID and o.Status=1 and q.CompanyType=102 ");
            strSql.Append("left join dbo.MDM_Position as p on p.PositionGUID=po.PositionGuid ");
            strSql.Append("where u.Status=1 ");
            //strSql.Append("and u.usertypename = '内部员工' ");
            strSql.Append("and u.UserLoginID = '" + usercode + "' ");
            strSql.Append("order by substring(substring(o.F3,charindex('/',o.F3)+1,len(o.F3)-charindex('/',o.F3)),0,charindex('/',substring(o.F3,charindex('/',o.F3)+1,len(o.F3)-charindex('/',o.F3)))),o.OrgUnitName ");
            return DbHelperSQL160.Query(strSql.ToString());
        }

        public DataSet GetVisitAddress(string Company)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [ID],[Company],[Address],[SortID],[Status],[CreatorID],[Creator],[CreateDate] FROM [LDXX].[dbo].[YQ_VisitAddress] where Company = '" + Company + "'");
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetEmployee(string whereinfo, string orderinfo)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [ID],[Company],[Department],[UserName],[UserID],[IsDeclare],[Reason],[VisitCompany],[Address],[VisitDate] ");
            strSql.Append(",[BegVisitTime],[EndVisitTime],[VisitTime],[NumVisitors],[SupplierName],[SortID],[Status],[CreatorID],[Creator],[CreateDate] ");
            strSql.Append("FROM [LDXX].[dbo].[YQ_EmployeeDeclaration] ");
            if (whereinfo != String.Empty)
            {
                strSql.Append(whereinfo + " ");
            }
            if (orderinfo != String.Empty)
            {
                strSql.Append(orderinfo);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetEmployeeCustomer(string whereinfo, string orderinfo)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT e.[ID],e.[Company],e.[Department],e.[UserName],e.[UserID],e.[IsDeclare],e.[Reason],e.[VisitCompany],e.[Address],e.[VisitDate] ");
            strSql.Append(",e.[BegVisitTime],e.[EndVisitTime],e.[VisitTime],e.[NumVisitors],e.[SupplierName], ");
            strSql.Append("c.[ID] cID,c.[EmployeeID],c.[IsPromise],c.[Supplier],c.[CName],c.[Telephone],c.[IDNumber],c.[Temperature],c.[VisitAddress],c.[IsPass],c.[IsContact],c.[IsCoincidence], ");
            strSql.Append("c.[IsSeeDoctor],c.[Isolated],c.[IsInform],c.[CurrentAddress],c.[Oexplain],  ");
            strSql.Append("v1.Type as Type1,v1.Describe as Describe1,v1.YesOrNo as YesOrNo1,");
            strSql.Append("v2.Type as Type2,v2.Describe as Describe2,v2.YesOrNo as YesOrNo2,");
            strSql.Append("v3.Type as Type3,v3.Describe as Describe3,v3.YesOrNo as YesOrNo3 ");
            strSql.Append("FROM [LDXX].[dbo].[YQ_EmployeeDeclaration] e ");
            strSql.Append("left join [LDXX].[dbo].[YQ_CustomerDeclaration] c on e.ID = c.EmployeeID ");
            strSql.Append("left join [dbo].[YQ_VisitConfirmation] v1 on v1.EmployeeID = e.ID and v1.Type = 1 ");
            strSql.Append("left join [dbo].[YQ_VisitConfirmation] v2 on v2.EmployeeID = e.ID and v2.Type = 2 ");
            strSql.Append("left join [dbo].[YQ_VisitConfirmation] v3 on v3.EmployeeID = e.ID and v3.Type = 3 ");
            if (whereinfo != String.Empty)
            {
                strSql.Append(whereinfo + " ");
            }
            if (orderinfo != String.Empty)
            {
                strSql.Append(orderinfo);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        //public DataSet GetEmployeeGroupByCustomerID(string whereinfo, string orderinfo)
        //{//获取该餐厅配餐结束的最大日期
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("SELECT e.[ID],e.[Company],e.[Department],e.[UserName],e.[UserID],e.[IsDeclare],e.[Reason],e.[VisitCompany],e.[Address],e.[VisitDate] ");
        //    strSql.Append(",e.[BegVisitTime],e.[EndVisitTime],e.[VisitTime],e.[NumVisitors],e.[SupplierName],count(c.[ID]) as Num  ");
        //    strSql.Append("FROM [LDXX].[dbo].[YQ_EmployeeDeclaration] e ");
        //    strSql.Append("left join [LDXX].[dbo].[YQ_CustomerDeclaration] c on e.ID = c.EmployeeID ");
        //    if (whereinfo != String.Empty)
        //    {
        //        strSql.Append(whereinfo + " ");
        //    }
        //    strSql.Append("group by e.[ID],e.[Company],e.[Department],e.[UserName],e.[UserID],e.[IsDeclare],e.[Reason],e.[VisitCompany],e.[Address],e.[VisitDate] ");
        //    strSql.Append(" ,e.[BegVisitTime],e.[EndVisitTime],e.[VisitTime],e.[NumVisitors],e.[SupplierName],e.[CreateDate]  ");
        //    if (orderinfo != String.Empty)
        //    {
        //        strSql.Append(orderinfo);
        //    }
        //    return DbHelperSQL.Query(strSql.ToString());
        //}
        public DataSet GetEmployeeGroupByCustomerID(string whereinfo, string orderinfo)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT e.[ID],e.[Company],e.[Department],e.[UserName],e.[UserID],e.[IsDeclare],e.[Reason],e.[VisitCompany],e.[Address],e.[VisitDate] ");
            strSql.Append(",e.[BegVisitTime],e.[EndVisitTime],e.[VisitTime],e.[NumVisitors],e.[SupplierName],count(c.[ID]) as Num,  ");
            strSql.Append("c.[ID] cID,c.[EmployeeID],c.[IsPromise],c.[Supplier],c.[CName],c.[Telephone],c.[IDNumber],c.[Temperature],c.[VisitAddress],c.[IsPass],c.[IsContact],c.[IsCoincidence], ");
            strSql.Append("c.[IsSeeDoctor],c.[Isolated],c.[IsInform],c.[CurrentAddress],c.[Oexplain],  ");
            strSql.Append("v1.Type as Type1,v1.Describe as Describe1,v1.YesOrNo as YesOrNo1,");
            strSql.Append("v2.Type as Type2,v2.Describe as Describe2,v2.YesOrNo as YesOrNo2,");
            strSql.Append("v3.Type as Type3,v3.Describe as Describe3,v3.YesOrNo as YesOrNo3 ");
            strSql.Append("FROM [LDXX].[dbo].[YQ_EmployeeDeclaration] e ");
            strSql.Append("left join [LDXX].[dbo].[YQ_CustomerDeclaration] c on e.ID = c.EmployeeID ");
            strSql.Append("left join [dbo].[YQ_VisitConfirmation] v1 on v1.EmployeeID = e.ID and v1.Type = 1 ");
            strSql.Append("left join [dbo].[YQ_VisitConfirmation] v2 on v2.EmployeeID = e.ID and v2.Type = 2 ");
            strSql.Append("left join [dbo].[YQ_VisitConfirmation] v3 on v3.EmployeeID = e.ID and v3.Type = 3 ");
            if (whereinfo != String.Empty)
            {
                strSql.Append(whereinfo + " ");
            }
            strSql.Append("group by e.[ID],e.[Company],e.[Department],e.[UserName],e.[UserID],e.[IsDeclare],e.[Reason],e.[VisitCompany],e.[Address],e.[VisitDate] ");
            strSql.Append(" ,e.[BegVisitTime],e.[EndVisitTime],e.[VisitTime],e.[NumVisitors],e.[SupplierName],e.[CreateDate],  ");
            strSql.Append(" c.[ID],c.[EmployeeID],c.[IsPromise],c.[Supplier],c.[CName],c.[Telephone],c.[IDNumber],c.[Temperature],c.[VisitAddress],c.[IsPass],c.[IsContact],c.[IsCoincidence], ");
            strSql.Append(" c.[IsSeeDoctor],c.[Isolated],c.[IsInform],c.[CurrentAddress],c.[Oexplain],  ");
            strSql.Append("v1.Type,v1.Describe,v1.YesOrNo,v2.Type,v2.Describe,v2.YesOrNo,v3.Type,v3.Describe,v3.YesOrNo ");
            if (orderinfo != String.Empty)
            {
                strSql.Append(orderinfo);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }



        public DataSet GetPagePermission(string whereinfo, string orderinfo)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [guid],[userID],[userName],[Permission],[F1],[F2],[F3],[creatorID],[creator],[createDate]  ");
            strSql.Append("FROM [LDXX].[dbo].[YQ_PagePermission] ");
            if (whereinfo != String.Empty)
            {
                strSql.Append(whereinfo + " ");
            }
            if (orderinfo != String.Empty)
            {
                strSql.Append(orderinfo);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        public int GetVisitorsCount(string whereinfo)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select case when sum(e.NumVisitors) is null then 0 else sum(e.NumVisitors) end as VisitorsCount  ");
            strSql.Append("   FROM [LDXX].[dbo].[YQ_EmployeeDeclaration] e ");
            strSql.Append("   left join [LDXX].[dbo].[YQ_CustomerDeclaration] c on e.ID = c.EmployeeID ");
            if (whereinfo != String.Empty)
            {
                strSql.Append(whereinfo + " ");
            }
            int count = 0;
            DataSet ds = new DataSet();
            ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }

            return count;
        }

        public int GetUnhealthyCount(string whereinfo)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT count(e.[ID]) as Num  ");
            strSql.Append("FROM [LDXX].[dbo].[YQ_EmployeeDeclaration] e  ");
            strSql.Append("left join [LDXX].[dbo].[YQ_CustomerDeclaration] c on e.ID = c.EmployeeID  ");
            strSql.Append("left join [dbo].[YQ_VisitConfirmation] v1 on v1.EmployeeID = e.ID and v1.Type = 1 ");
            strSql.Append("left join [dbo].[YQ_VisitConfirmation] v2 on v2.EmployeeID = e.ID and v2.Type = 2 ");
            strSql.Append("left join [dbo].[YQ_VisitConfirmation] v3 on v3.EmployeeID = e.ID and v3.Type = 3 ");
            strSql.Append("where (v2.YesOrNo = 1 or v3.YesOrNo = 1) ");

            int count = 0;
            DataSet ds = new DataSet();
            ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }

            return count;
        }




    }


    /// <summary>
    /// 将Json格式数据转化成对象
    /// </summary>
    public class JsonHelper
    {
        /// <summary>  
        /// 生成Json格式  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static string GetJson<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray()); return szJson;
            }
        }
        /// <summary>  
        /// 获取Json的Model  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="szJson"></param>  
        /// <returns></returns>  
        public static T ParseFromJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }
    }






}

public class T//:IEquatable<Person>
{
    public string _sql = null;
    public string _sql2 = null;
    public string _inputdate = null;
    public string _bankaccountno = null;
    public string _cName = null;
    public string _cAreaname = null;

    public T() { }

    public T(string sql, string inputdate, string bankaccountno, string cName, string cAreaname, string sql2)
    {
        this._sql = sql;
        this._sql2 = sql2;
        this._inputdate = inputdate;
        this._bankaccountno = bankaccountno;
        this._cName = cName;
        this._cAreaname = cAreaname;
    }
}


