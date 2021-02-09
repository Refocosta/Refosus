using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Connection
{
    public class Data
    {

        public Data()
        {
        }
        public IConfiguration Configuration { get; }




        #region Shopping
        public bool updShopingToNull(ShopingToNull obj,string cade)
        {
            return SetDataAutoBit(cade, obj.member, obj);
        }
        #endregion












        internal bool SetDataAutoBit(string conexion, string sp, object source)
        {
            using (SqlConnection sqlConn = new SqlConnection(conexion))
            {
                sp = sp;
                SqlCommand command = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = sp
                };
                command.Parameters.AddRange(getParameters(source, sp, conexion).ToArray());
                command.CommandTimeout = 0;
                command.Connection = sqlConn;
                sqlConn.Open();
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
                finally
                {
                    if (sqlConn.State == ConnectionState.Open)
                    {
                        sqlConn.Close();
                    }
                }
            }
        }
        internal List<SqlParameter> getParameters(object source, string SPName,string conexion)
        {
            SqlConnection test = new SqlConnection();
            test.ConnectionString = conexion;
            List<SqlParameter> pars = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@ProcedureName", Value = SPName }
            };
            DataTable dt = GetData(test.ConnectionString, "SP_IH_GetSPInfo", pars).Tables[0];
            //Parametros de salida
            List<SqlParameter> Result = new List<SqlParameter>();
            foreach (DataRow itname in dt.Rows)
            {
                Result.Add(new SqlParameter { ParameterName = itname["name"].ToString(), Value = source.GetType().GetProperty(itname["name"].ToString().Replace("@", "")).GetValue(source,null) });
            }
            return Result;
        }
        internal DataSet GetData(string conexion, string sp, List<SqlParameter> parameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(conexion))
            {

                SqlCommand command = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = sp
                };
                foreach (SqlParameter p in parameters)
                {
                    command.Parameters.Add(p);
                }
                command.CommandTimeout = 0;
                command.Connection = sqlConn;
                sqlConn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter adaptador = new SqlDataAdapter(command);
                try
                {
                    adaptador.Fill(ds);
                    foreach (DataTable d in ds.Tables)
                    {
                        d.TableName = d.Columns[d.Columns.Count - 1].ColumnName;
                    }
                    return ds;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception(ex.Message);
                }
                finally
                {
                    adaptador.Dispose();
                    if (sqlConn.State == ConnectionState.Open)
                    {
                        sqlConn.Close();
                    }
                }
            }
        }
    }
}
