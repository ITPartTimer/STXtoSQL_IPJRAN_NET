using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace STXtoSQL
{
    public class Helpers
    {
        private string _STRATIXDataConnString;

        private string _ODBCDataConnString;

        // SQL Server Database connection string
        public string STRATIXDataConnString
        {
            get
            {
                //_STRATIXDataConnString = "Data Source=VC-030\\SQLDEV2014VC030;Initial Catalog=STRATIXData;User ID=DataImporter;Password=cX63o4qd";
                _STRATIXDataConnString = "Data Source=VC-030\\SQLDEV2014VC030;Initial Catalog=STRATIXData;Integrated Security=SSPI";              

                if (string.IsNullOrEmpty(_STRATIXDataConnString))
                {
                    throw new NullReferenceException("Empty Connection String");
                }
                else
                {
                    return _STRATIXDataConnString;
                }

            }

        } //STRATIXDataConnString

        // ODBC to Stratix using DSN names on local machine
        public string ODBCDataConnString
        {
            get
            {
                // DSN stored in App.config
                string dsn = ConfigurationManager.AppSettings.Get("DSN");

                _ODBCDataConnString = "DSN=" + dsn + ";UID=livcalod;Pwd=livcalod";

                if (string.IsNullOrEmpty(_ODBCDataConnString))
                {
                    throw new NullReferenceException("Empty Connection String");
                }
                else
                {
                    return _ODBCDataConnString;
                }

            }

        }

        // Is the number integer.  True, if conversion is a success
        public static bool IsInteger (string job)
        {
            try
            {
                Convert.ToInt32(job);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ---------------------------------------------------------
        // Add input parameters to a SQL command object.  Overloaded
        // to accept a param value for input parameters
        // ---------------------------------------------------------
        protected void AddParamToSQLCmd(SqlCommand sqlCmd, string paramID,
                                        SqlDbType sqlType, int paramSize,
                                        ParameterDirection paramDirection, object paramValue)
        {
            //See VB code for some error checking to insert here

            SqlParameter newSqlParam = new SqlParameter();

            newSqlParam.ParameterName = paramID;
            newSqlParam.SqlDbType = sqlType;
            newSqlParam.Size = paramSize;
            newSqlParam.Direction = paramDirection;
            newSqlParam.Value = paramValue;

            sqlCmd.Parameters.Add(newSqlParam);
        }

        // ------------------------------------------------------
        // Overload for input parameter to allow proper config.
        // of a Decimal SqlDbType paramSize=Precision, scale=Scale
        // -------------------------------------------------------
        protected void AddParamToSQLCmd(SqlCommand sqlCmd, string paramID,
                                        SqlDbType sqlType, int paramSize, int scale,
                                        ParameterDirection paramDirection, object paramValue)
        {
            //See VB code for some error checking to insert here

            SqlParameter newSqlParam = new SqlParameter();

            newSqlParam.ParameterName = paramID;
            newSqlParam.SqlDbType = sqlType;
            newSqlParam.Precision = (byte)paramSize;
            newSqlParam.Scale = (byte)scale;
            newSqlParam.Direction = paramDirection;
            newSqlParam.Value = paramValue;

            sqlCmd.Parameters.Add(newSqlParam);
        }

        // ---------------------------------------------------
        // Add output parameters to SQL comment object
        // ---------------------------------------------------
        protected void AddParamToSQLCmd(SqlCommand sqlCmd, string paramID,
                                        SqlDbType sqlType, int paramSize,
                                        ParameterDirection paramDirection)
        {
            //See VB code for some error checking to insert here

            SqlParameter newSqlParam = new SqlParameter();

            newSqlParam.ParameterName = paramID;
            newSqlParam.SqlDbType = sqlType;
            newSqlParam.Size = paramSize;
            newSqlParam.Direction = paramDirection;

            sqlCmd.Parameters.Add(newSqlParam);
        }
    }
}
