using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.Security;
namespace OAServiceApp
{
    

    [WebService(Namespace = "http://pdsandroid.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class OAServiceAndroidApp: System.Web.Services.WebService
    {
        public OAServiceAndroidApp()
        {
            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
        }

        #region  验证登录信息
        [WebMethod(Description = "验证登录信息（DataSet）")]
        public DataSet CheckLogin(string floginid, string fpassword)
        {
            string userPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(fpassword, "MD5");

            SqlParameter[] para ={
                new SqlParameter("@floginid",floginid),
                new SqlParameter("@fpassword",userPassword)
            };
            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_CheckLogin", para);
        }
        #endregion

        #region 获取内部新闻 ftype=1,获取通知公告 ftype=2/公交的一天ftype=3 支持分页
        //方法名唯一，不可以重载，所以无法同名兼容以前的版本。存储过程提供了默认参数，兼容性较好。
        [WebMethod(Description = "获取内部新闻，分页（DataSet）")]
        public DataSet GetNewsInfoPage(int newsType, int pageFor, int pageSize)
        {
            SqlParameter[] para ={
                new SqlParameter("@newsType",newsType),
                new SqlParameter("@pageFor",pageFor),
                new SqlParameter("@pageSize",pageSize)
            };
            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_GetNewsInfo", para);
        }
        #endregion

        #region 获取内部新闻 ftype=1,获取通知公告 ftype=2/公交的一天ftype=3

        [WebMethod(Description = "获取内部新闻（DataSet）")]
        public DataSet GetNewsInfo(int newsType)
        {
            SqlParameter[] para ={
                new SqlParameter("@newsType",newsType)         
            };
            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_GetNewsInfo", para);
        }
        #endregion

        #region 根据信息内码获取详细信息
        [WebMethod(Description = "根据信息内码获取详细信息（DataSet）")]
        public DataSet GetDetailInfo(int fid)
        {
            SqlParameter[] para ={
                new SqlParameter("@FID",fid)
            };
            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_GetDetailInfo", para);
        }
        #endregion

        #region 特殊信息
        [WebMethod(Description = "特殊信息（DataSet）")]
        public DataSet GetProFit(int fyear, int fperiod)
        {
            SqlParameter[] para ={
                 new SqlParameter("@fyear",fyear),
                new SqlParameter("@fperiod",fperiod)
            };
            //执行的是 server=192.168.0.40;database=UFSystem;上的存储过程
            return SqlHelper.Query(SqlHelper.connectionStringSpec, "ds", CommandType.StoredProcedure, "proc_profit", para);

        }
        #endregion

        #region 工作汇报列表
        [WebMethod(Description = "工作汇报列表（DataSet）")]
        public DataSet GetRepList()
        {
            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "pro_repList", null);
        }
        #endregion

        #region 日工作汇报部门
        [WebMethod(Description = "日工作汇报部门（DataSet）")]
        public DataSet GetRepDep(string repTime)
        {
            SqlParameter[] para ={
                new SqlParameter("@repTime",repTime)
            };
            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "pro_repDep", para);
        }
        #endregion

        #region 日工作汇报详细
        [WebMethod(Description = "日工作汇报详细（DataSet）")]
        public DataSet GetRepDetail(int repID)
        {
            SqlParameter[] para ={
                new SqlParameter("@repID",repID)
            };
            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "pro_repDetail", para);
        }
        #endregion

        //工作流核心
        #region 待办工作 表单类型
        [WebMethod(Description = "待办工作表单类型（DataSet）")]
        public DataSet GetWFForm()
        {
            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_GetWFForm", null);
        }
        #endregion

        #region 用户待办表单
        [WebMethod(Description = "用户待办表单（DataSet）")]
        public DataSet GetUserForm(string userid, string formname, string state, int pageFor, int pageSize)
        {
            SqlParameter[] para = { new SqlParameter("@userid", userid), 
                                new SqlParameter("@formname",formname),
                                new SqlParameter("@state", state),
                                new SqlParameter("@pageFor",pageFor),
                                new SqlParameter("@pageSize", pageSize)
                              };
            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_GetUserForm", para);
        }
        #endregion

        #region 执行sql语句
        [WebMethod(Description = "执行sql语句（DataSet）")]
        public DataSet ExecSql(string sql)
        {
            SqlParameter[] para = { new SqlParameter("@sql", sql) };
            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_ExecSql", para);
        }
        #endregion

        #region 处理委托
        [WebMethod(Description = "处理委托（DataSet）")]
        public DataSet ProcessWt(string id, string userID, string userName, string wtUserID, string wtUserName)
        {
            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@userID", userID),
                new SqlParameter("@userName", userName),
                new SqlParameter("@wtUserID", wtUserID),
                new SqlParameter("@wtUserName", wtUserName)
            };

            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_Wt", para);
        }
        #endregion

        #region 保存
        [WebMethod(Description = "保存（DataSet）")]
        public DataSet ProcessSave(string id, string userId, string userName, string fileContent, string spContent)
        {
            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@userID", userId),
                new SqlParameter("@userName", userName),
                new SqlParameter("@fileContent", fileContent),
                new SqlParameter("@spContent", spContent)
            };

            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_transaction_save", para);
        }
        #endregion

        #region 保存并通过
        [WebMethod(Description = "保存并通过（DataSet）")]
        public DataSet ProcessSaveAndPass(string id, string userId, string userName, string fileContent, string spContent)
        {
            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@userID", userId),
                new SqlParameter("@userName", userName),
                new SqlParameter("@fileContent", fileContent),
                new SqlParameter("@spContent", spContent)
            };

            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_transaction_saveandpass", para);
        }
        #endregion

        #region 保存并驳回
        [WebMethod(Description = "保存并通过（DataSet）")]
        public DataSet ProcessSaveAndReject(string id, string userId, string userName, string fileContent, string spContent)
        {
            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@userID", userId),
                new SqlParameter("@userName", userName),
                new SqlParameter("@fileContent", fileContent),
                new SqlParameter("@spContent", spContent)
            };

            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_transaction_saveandreject", para);
        }
        #endregion

        #region 提交
        [WebMethod(Description = "提交（DataSet）")]
        public DataSet ProcessSubmit(string id, string userId, string userName, string jbrUserId, string jbrUserName, string selectedValue, string innerSms, string phoneSms)
        {
            SqlParameter[] para =
            {
                new SqlParameter("@id", id),
                new SqlParameter("@userID", userId),
                new SqlParameter("@userName", userName),
                new SqlParameter("@jbrUserId", jbrUserId),
                new SqlParameter("@jbrUserName", jbrUserName),
                 new SqlParameter("@selectedValue", selectedValue),
                new SqlParameter("@innerSMS", innerSms),
                new SqlParameter("@phoneSMS", phoneSms)
            };

            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_transaction_SpNext", para);
        }
        #endregion

        #region 驳回
        [WebMethod(Description = "驳回（DataSet）")]
        public DataSet ProcessReject(string id, string userId, string userName, string jbrUserId, string jbrUserName, string selectedValue)
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

            return SqlHelper.Query(SqlHelper.connectionString, "ds", CommandType.StoredProcedure, "proc_transaction_SpBhNext", para);
        }
        #endregion
    }

}
