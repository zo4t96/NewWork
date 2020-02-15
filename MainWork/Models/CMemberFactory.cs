using MainWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace prjSpotifyProject.Models.Member
{
    public class tMemberFactory
    {
        public tMember getByAccount(string account)
        {
            List<tMember> list = queryBySql("SELECT * FROM tMember WHERE fAccount='" + account + "'");
            if (list.Count == 0)
                return null;
            return list[0];
        }
        public List<tMember> queryBySql(string sql)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbProjectMusicStore;Integrated Security=True";
            con.Open();

            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            con.Close();

            List<tMember> list = new List<tMember>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                tMember x = new tMember();
                x.fAccount = r["fAccount"].ToString();
                x.fPassword = r["fPassword"].ToString();                
                x.fEmail = r["fEmail"].ToString();
                x.fNickName = r["fNickName"].ToString();
                
                list.Add(x);
            }
            return list;
        }

        //新增新增方法
        public void create(tMember m)
        {
            string sql = "INSERT INTO tMember";
            sql += "(fAccount,fPassword,fEmail,fNickName)";
            sql += "VALUES";
            sql += $"('{m.fAccount}','{m.fPassword}','{m.fEmail}','{m.fNickName}')";

            executeSql(sql); //call excutesql()方法
        }
        //把重複的方法拉出來(反白=>編輯=>重構=>擷取方法)
        private static void executeSql(string sql)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbProjectMusicStore;Integrated Security=True";
            con.Open();

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}