using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;

namespace OAServiceApp
{

    /// <summary>
    /// OAServiceIOSApp 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://pdsios.org/")]
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    //返回的Json字符串被XML包裹，查了很多资料，无法在服务器端解决。决定在客户端解决该问题。
    public class OAServiceIOSApp : System.Web.Services.WebService
    {

        public OAServiceIOSApp()
        {

            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
        }

        #region  验证登录信息
        [WebMethod(Description = "验证登录信息(JSON)")]//网页上提供该方法声明
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = false)]
        //不需要该属性，Android端设置Http响应头的Content-Type为application/json即可返回JSON数据格式给客户端 
        public string CheckLogin(string floginid, string fpassword)
        {
            //text/plain为纯文本
            //this.Context.Response.ContentType = "application/josn;charset=utf-8";//设置也没用，没用到Response
            HttpContext.Current.Request.ContentType = "application/josn;charset=utf-8";//设置迟了？
            //HttpContext.Current.Response.ContentType = "application/josn;charset=utf-8";
            string userPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(fpassword, "MD5");

            SqlParameter[] para ={
                new SqlParameter("@floginid",floginid),
                new SqlParameter("@fpassword",userPassword)
            };
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_CheckLogin_IOS", para);
            //return ds;
            //HttpContext.Current.Response.Write(DataTable2Json(ds.Tables[0]));//设置Response有效
            return DataTable2Json(ds.Tables[0], false);
            //return new JavaScriptSerializer().Serialize( DataTable2Json(ds.Tables[0]));
            //return "abc";
        }
        #endregion

        #region 获取内部新闻 ftype=1,获取通知公告 ftype=2,公交的一天ftype=3
        [WebMethod(Description = "获取内部新闻 ftype=1,获取通知公告 ftype=2,公交的一天ftype=3")]
        public string GetNewsInfo(int newsType, int pageFor, int pageSize)
        {
            SqlParameter[] para ={
                new SqlParameter("@newsType",newsType), new SqlParameter("@pageFor",pageFor), new SqlParameter("@pageSize",pageSize)
            };
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_GetNewsInfo_IOS", para);
            return DataTable2Json(ds.Tables[0], true);
        }
        #endregion

        #region 根据信息内码获取详细信息
        [WebMethod(Description = "根据信息内码获取详细信息(JSON)")]
        public string GetDetailInfo(int articleID)
        {
            SqlParameter[] para ={
                new SqlParameter("@ArticleID",articleID)//@ArticleID为sqlserver中的参数，大小写不铭感。
            };
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_GetDetailInfo_IOS", para);

            return DataTable2Json(ds.Tables[0], false);
        }
        #endregion

        #region 特殊信息
        [WebMethod(Description = "特殊信息(JSON)")]
        public string GetProfit(int fyear, int fperiod)
        {
            SqlParameter[] para ={
                 new SqlParameter("@fyear",fyear),
                new SqlParameter("@fperiod",fperiod)
            };
            DataSet ds = SqlHelper.Query(SqlHelper.connectionStringSpec, "ds", CommandType.StoredProcedure, "proc_Profit", para);
            return DataTable2Json(ds.Tables[0], false);
        }
        #endregion

        #region 工作汇报时间列表
        [WebMethod(Description = "工作汇报列表(JSON)")]
        public string GetRepTimeList(int pageFor, int pageSize)
        {
            SqlParameter[] para ={
                 new SqlParameter("@pageFor",pageFor),
                new SqlParameter("@pageSize",pageSize)
            };
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_RepList_IOS", para);
            //return DataSet2Json(ds);
            return DataTable2Json(ds.Tables[0], true);
        }
        #endregion

        #region 日工作汇报部门
        [WebMethod(Description = "日工作汇报部门(JSON)")]
        public string GetRepDepList(string repTime)
        {
            SqlParameter[] para ={
                new SqlParameter("@repTime",repTime)
            };
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_RepDep_IOS", para);
            return DataTable2Json(ds.Tables[0], true);
        }
        #endregion

        #region 日工作汇报详细
        [WebMethod(Description = "日工作汇报详细(JSON)")]
        public string GetRepDetail(int repID)
        {
            SqlParameter[] para ={
                new SqlParameter("@repID",repID)
            };
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_RepDetail_IOS", para);
            return DataTable2Json(ds.Tables[0], false);
        }
        #endregion

        //工作流核心
        #region 待办工作 表单类型
        [WebMethod(Description = "待办工作表单类型")]
        public string GetWFForm()
        {
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_GetWFForm", null);
            return DataTable2Json(ds.Tables[0], true);
        }
        #endregion

        #region 用户待办表单列表
        [WebMethod(Description = "用户待办表单列表")]
        public string GetUserForm(string userid, string formname, string state, int pageFor, int pageSize)
        {
            SqlParameter[] para = { new SqlParameter("@userid", userid), 
                                new SqlParameter("@formname",formname),
                                new SqlParameter("@state", state),
                                new SqlParameter("@pageFor",pageFor),
                                new SqlParameter("@pageSize", pageSize)
                              };
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_GetUserForm_IOS", para);//没有表单数据,多条数据客户端不怎么好解析。
            return DataTable2Json(ds.Tables[0], true);
        }
        #endregion

        #region 用户待办表单
        [WebMethod(Description = "用户待办表单")]
        public string GetSingleUserForm(string state, int id)
        {
            SqlParameter[] para = { new SqlParameter("@id", id), 
                                new SqlParameter("@state", state)                              
                              };
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_GetUserFormByID", para);
            return DataTable2Json(ds.Tables[0], false);
        }
        #endregion

        #region 执行sql语句,多条数据，有[]
        [WebMethod(Description = "执行sql语句")]
        public string ExecSql(string sql)
        {
            SqlParameter[] para = { new SqlParameter("@sql", sql) };
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_ExecSql", para);
            return DataTable2Json(ds.Tables[0], true);//多条数据，有[]；一条数据，有[]
        }
        #endregion

        #region 执行sql语句,一条数据，没有[]
        [WebMethod(Description = "执行sql语句")]
        public string ExecSqlSingle(string sql)
        {
            SqlParameter[] para = { new SqlParameter("@sql", sql) };
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_ExecSql", para);
            return DataTable2Json(ds.Tables[0], false);//多条数据，有[]；一条数据，没有[]
        }
        #endregion

        #region 处理委托
        [WebMethod(Description = "处理委托")]
        public string ProcessWt(string id, string userId, string userName, string wtUserId, string wtUserName)
        {
            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@userID", userId),
                new SqlParameter("@userName", userName),
                new SqlParameter("@wtUserID", wtUserId),
                new SqlParameter("@wtUserName", wtUserName)
            };

            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_Wt", para);
            return DataTable2Json(ds.Tables[0], false);
        }
        #endregion

        #region 保存
        [WebMethod(Description = "保存")]
        public string ProcessSave(string id, string userId, string userName, string fileContent, string spContent)
        {
            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@userID", userId),
                new SqlParameter("@userName", userName),
                new SqlParameter("@fileContent", fileContent),
                new SqlParameter("@spContent", spContent)
            };

            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_transaction_save", para);
            return DataTable2Json(ds.Tables[0], false);
        }
        #endregion

        #region 保存并通过
        [WebMethod(Description = "保存并通过")]
        public string ProcessSaveAndPass(string id, string userId, string userName, string fileContent, string spContent)
        {
            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@userID", userId),
                new SqlParameter("@userName", userName),
                new SqlParameter("@fileContent", fileContent),
                new SqlParameter("@spContent", spContent)
            };

            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_transaction_saveandpass", para);
            return DataTable2Json(ds.Tables[0], false);
        }
        #endregion

        #region 保存并驳回
        [WebMethod(Description = "保存并驳回")]
        public string ProcessSaveAndReject(string id, string userId, string userName, string fileContent, string spContent)
        {
            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@userID", userId),
                new SqlParameter("@userName", userName),
                new SqlParameter("@fileContent", fileContent),
                new SqlParameter("@spContent", spContent)
            };

            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_transaction_saveandreject", para);
            return DataTable2Json(ds.Tables[0], false);
        }
        #endregion

        #region 保存并通过后的 提交
        [WebMethod(Description = "保存并通过后的 提交")]
        public string ProcessSubmit(string id, string userId, string userName, string jbrUserId, string jbrUserName, string selectedValue, string innerSms, string phoneSms)
        {
            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@userID", userId),
                new SqlParameter("@userName", userName),
                new SqlParameter("@jbrUserId", jbrUserId),
                new SqlParameter("@jbrUserName", jbrUserName),
                 new SqlParameter("@selectedValue", selectedValue),
                new SqlParameter("@innerSMS", innerSms),//内部短信通知,数据由sql语句插入
                new SqlParameter("@phoneSMS", phoneSms)
            };
            /* if ("1".Equals(innerSms))
             {
             //内部短信通知
             }
             if ("1".Equals(phoneSms))
             {
              //手机短信通知
             }*/
            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_transaction_SpNext", para);
            return DataTable2Json(ds.Tables[0], false);
        }
        #endregion

        #region 保存并驳回后的 驳回
        [WebMethod(Description = "保存并驳回后的 驳回")]
        public string ProcessReject(string id, string userId, string userName, string jbrUserId, string jbrUserName, string selectedValue)
        {
            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@userID", userId),
                new SqlParameter("@userName", userName),
                new SqlParameter("@jbrUserId", jbrUserId),
                new SqlParameter("@jbrUserName", jbrUserName),
                 new SqlParameter("@selectedValue", selectedValue)
                
            };

            DataSet ds = SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_transaction_SpBhNext", para);
            return DataTable2Json(ds.Tables[0], false);
        }
        #endregion

        #region DataTable转换为Json格式，结果名称为表名 Core Method
        /// <summary>
        /// dataTable转换成Json格式
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="isItemParseAsDict">
        /// 如果只有一条数据返回，是否需要确保item0解析成一个Dcit(while循环时用到！必须确保每一个item为dict！):这条数据
        /// false 被解析成一个{a,b,c}(item0=a,item1=b,item2=c)。
        /// true  被解析成[{a,b,c}](item0={a,b,c})!
        /// 一般而言，如果仅返回一条数据，则false，确保没有[],可以解析到a，b，c
        /// 如果返回多条数据，则true,确保在只有一条数据时，也有[]，可以解析到{a,b,c}
        /// isItemParseAsDict 也可叫做 isMutableData
        /// </param>
        /// <returns></returns>
        public static string DataTable2Json(DataTable dt, bool isItemParseAsDict)
        {

            StringBuilder jsonBuilder = new StringBuilder();

            //jsonBuilder.Append("{\"");
            //jsonBuilder.Append(dt.TableName); //不需要表名，否则还要再取一次值，才能得到想要的值数组， {"tablename":[{}]}
            //jsonBuilder.Append("\":[");
            int count = dt.Rows.Count;

            //if (count >= 1 && isItemParseAsDict)
            //一共85条数据，前84条OK，第85条最后一条报错！调试发现，即使只有一条数据，
            //这个[]也是必须的,否则解析不出NSDictionary。但是对于登陆结果，返回一条数据，这个[]是必须没有的！
            //其实用if(count>1 ||(count==1&&isItemParseAsDict)) 更为合理
            //即：多条数据，肯定有[]。单条数据，取决于isItemParseAsDict。
            if (count > 1 || (count == 1 && isItemParseAsDict))
            {
                jsonBuilder.Append("[");
            }
            //解析每条数据
            for (int i = 0; i < count; i++)
            {
                jsonBuilder.Append("{");
                //解析每条数据的每一列，生成Json
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);//如果不起名字，则Column1
                    jsonBuilder.Append("\":\"");
                    //jsonBuilder.Append(HttpUtility.HtmlEncode(dt.Rows[i][j]));//对字符串进行HTML编码，否则IOS客户端解析不了表单HTML(即使编码也无法解析！)；换了一种方法，无需编码。
                    jsonBuilder.Append(dt.Rows[i][j]);
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);//删除Json最后一个字段的“逗号”
                jsonBuilder.Append("},");
            }
            if (jsonBuilder.Length > 0)//判断jsonBuilder非空,因为如果没有任何数据，下面语句会报错
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);//删除最后一个Json的“逗号”
            //if (count >= 1 && isItemParseAsDict)
            if (count > 1 || (count == 1 && isItemParseAsDict))
            {
                jsonBuilder.Append("]");
            }
            //jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        #endregion

        #region  DataTable转换为Json格式，自定义结果名称 Plus1
        /// <summary>
        /// 自定义名称
        /// </summary>
        /// <param name="jsonName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTable2Json(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName + "\":\"" + dt.Rows[i][j] + "\"");
                        if (j < dt.Columns.Count - 1)//只有最后一个不加“逗号”
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        #endregion

        #region DataSet转换成Json格式 Plus2
        /// <summary>  
        /// DataSet转换成Json格式  
        /// </summary>  
        /// <param name="ds">DataSet</param> 
        /// <returns></returns>  
        public static string DataSet2Json(DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"");
                json.Append(dt.TableName);
                json.Append("\":");
                json.Append(DataTable2Json(dt, true));//这里为true，则表示，如果count>=1了，则有[]。如果>1有[]无异议，但是如果=1，也有[],则欠妥，只有多条数据返回时，才可以。
                json.Append("},");
            }
            if (json.Length > 1)//如果ds里面没有数据，那么json只是一个[，此时不用删除最后一个字符 
                json.Remove(json.Length - 1, 1);//删除组后一个json的“逗号”
            json.Append("]");
            return json.ToString();
        }
        #endregion
    }
}
