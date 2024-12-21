using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MYTaskSairamBolloju.App_Start
{
    public class DataAccessLayer
    {
        string Name = ConfigurationManager.ConnectionStrings["BSR_Tsk"].ConnectionString;
        SqlConnection Con; SqlCommand Cmd;
        #region DataInsert
        public bool InsertData(string StoredProcedure, string[] ParameterValues, string[] ParameterNames)
        {
            bool Result = false;
            try
            {
                Con = new SqlConnection(Name); Cmd = new SqlCommand(); Cmd.Connection = Con; Cmd.CommandText = StoredProcedure;
                Cmd.Parameters.Clear();
                if (ParameterValues.Length == ParameterNames.Length)
                {
                    Cmd.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < ParameterNames.Length; i++)
                    {
                        Cmd.Parameters.AddWithValue(ParameterValues[i], ParameterNames[i]);
                    }
                    Con.Open();
                    int A = Cmd.ExecuteNonQuery();
                    if (A > 0)
                        Result = true;
                    else
                        Result = false;
                }
            }
            catch (Exception Ex)
            {
                new Exception("Something went wrong.", Ex);
            }
            finally
            {
                if (Con != null && Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
            }
            return Result;
        }
        #endregion
        #region DataRetrieved
        public DataSet RetrievedData(string StoredProcedure, string[] ParameterValues = null, string[] ParameterNames = null)
        {
            DataSet Ds = new DataSet();
            try
            {
                Con = new SqlConnection(Name); Cmd = new SqlCommand(); Cmd.Connection = Con; Cmd.CommandText = StoredProcedure;
                Cmd.Parameters.Clear();
                if (ParameterNames != null && ParameterValues != null && ParameterNames.Length == ParameterValues.Length)
                {
                    Cmd.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < ParameterNames.Length; i++)
                    {
                        Cmd.Parameters.AddWithValue(ParameterValues[i], ParameterNames[i]);
                    }
                }
                SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                Da.Fill(Ds);
            }
            catch (Exception Ex)
            {
                new Exception("Something went wrong.", Ex);
            }
            return Ds;
        }
        #endregion
    }
}