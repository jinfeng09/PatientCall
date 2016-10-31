using System.Data;
using System.Data.SqlClient;

namespace CallSystem.DBClass
{
   public class DBMethod
    {
        public DBSqlHelper DBsql;//数据库操作类

        public DBMethod()
        {
            DBsql = new DBSqlHelper();
        }
       /// <summary>
       /// 更新showinfo表的状态信息
       /// </summary>
       /// <param name="ReceptId">叫号名称</param>
       /// <param name="tableid">平板ID</param>
       /// <param name="reshowflag">true，false的值</param>
       public void UpdateShowInfo(string ReceptId,string tableid,string reshowflag)
       {
           IDataParameter[] parameters = {  new SqlParameter("@ReceptID", SqlDbType.NVarChar,30),
                                              new SqlParameter("@TableID",SqlDbType.NVarChar,10),
                                              new SqlParameter("@ReShowFlag",SqlDbType.Int)
                                          };
           parameters[0].Value = ReceptId;
           parameters[1].Value = tableid;
           parameters[2].Value = reshowflag;
           DataTable dt = DBsql.ExecuteDataTableByParams("P_CS_UpdateCallInfo",parameters);
       }
    }
}
