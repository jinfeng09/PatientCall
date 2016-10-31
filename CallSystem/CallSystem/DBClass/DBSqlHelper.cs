using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CallSystem.DBClass
{
   public class DBSqlHelper
    {
        //获取数据库连接字段
        public string AppConfig() //获取App.config位置信息
        {
            int intPos = Application.StartupPath.Trim().IndexOf("bin") - 1;

            string strDirectoryPath = System.IO.Path.Combine(Application.StartupPath.Substring(0, intPos), "App.config");

            return strDirectoryPath;
        }
        public string GetValue(string appKey)//获取App.config值信息
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(AppConfig());
                XmlNode xNode;
                XmlElement xElem;
                xNode = xDoc.SelectSingleNode("//appSettings");
                xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
                if (xElem != null)
                    return xElem.GetAttribute("value");
                else
                    return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
        public string GetConnectStr() //方法：获取APP文件配制信息
        {
            //string strsql = GetValue("ConnectionStr");
            if (Class.Config.ConnStr != null)
            {
                return Class.Config.ConnStr;
            }
            else
            { 
               System.Configuration.AppSettingsReader appSettings = new System.Configuration.AppSettingsReader();
               string strsql = appSettings.GetValue("ConnectionStr", typeof(string)).ToString();
               return strsql;
            }
        }
        /// <summary>
        /// 存储过程查询功能/数据模式为DataSet
        /// </summary>
        /// <param name="storeProcedureName">存储过程名称</param>
        /// <returns></returns>
        public DataSet InquiryDataset(string storeProcedureName)
        {
            SqlConnection SqlConn = new SqlConnection(GetConnectStr());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = SqlConn;            // 设置sql连接  
            cmd.CommandText = storeProcedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlConn.Open();
            SqlDataAdapter dp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            // 填充dataset  
            try
            {
                dp.Fill(ds);
            }
            catch { }
            SqlConn.Close();
            return ds;
        }

        /// <summary>
        /// 存储过程查询功能/数据模式为DataTable
        /// </summary>
        /// <param name="storeProcedureName">存储过程名称</param>
        /// <returns></returns>
        public DataTable InquiryDataTable(string storeProcedureName)
        {
            SqlConnection SqlConn = new SqlConnection(GetConnectStr());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = SqlConn;            // 设置sql连接  
            cmd.CommandText = storeProcedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlConn.Open();
            SqlDataAdapter dp = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            // 填充dataTable 
            try
            {
                dp.Fill(ds);
            }
            catch { }
            SqlConn.Close();
            return ds;
        }
        /// <summary>
        /// 存储过程查询功能(带参数)/数据模式为DataTable/
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <param name="paraValues"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTableByParams(string storeProcedureName, params object[] paraValues)
        {
            SqlConnection SqlConn = new SqlConnection(GetConnectStr());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = SqlConn;            // 设置sql连接  
            cmd.CommandText = storeProcedureName;
            AssignParameterValues(cmd, paraValues);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlConn.Open();
            }
            catch { MessageBox.Show("数据库连接失败，请检查数据库连接!"); }
            SqlDataAdapter dp = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            // 填充dataTable 
            try
            {
                dp.Fill(ds); ListValue.IsYes = true;//返回执行成功的
            }
            catch { ListValue.IsYes = false; }
            SqlConn.Close();
            return ds;
        }
        /// <summary>
        /// 添加用户查询条件
        /// </summary>
        /// <param name="sqlCommand">Command</param>
        /// <param name="paraValues">查询信息</param>
        private void AssignParameterValues(SqlCommand sqlCommand, params object[] paraValues)
        {
            for (int i = 0; i < paraValues.Length; i++)
            {
                sqlCommand.Parameters.Add(paraValues[i]);
            }
        }
    }
}
