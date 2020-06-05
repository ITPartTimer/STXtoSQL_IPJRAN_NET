using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using STXtoSQL.Models;

namespace STXtoSQL.DataAccess
{
    class SQLData : Helpers
    {
        public int Write_IPJRAN_IMPORT(List<IPJRAN> lstIPJRAN)
        {
            // Returning rows inserted into IMPORT
            int r = 0;

            SqlConnection conn = new SqlConnection(STRATIXDataConnString);

            try
            {
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand();

                cmd.Transaction = trans;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                // First empty IMPORT table
                try
                {
                    cmd.CommandText = "DELETE from ST_IMPORT_tbl_IPJRAN";

                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }

                try
                {
                    // Change Text to Insert data into IMPORT
                    cmd.CommandText = "INSERT INTO ST_IMPORT_tbl_IPJRAN (ran_whs,ran_rec_pfx,ran_rec_no,ran_pwc,ran_tot_wgt,ran_actst_ltts,ran_tot_pcs,ran_tot_run,actvy_mn,actvy_dy,actvy_yr) " +
                                        "VALUES (@whs,@pfx,@no,@pwc,@wgt,@actst,@pcs,@run,@actvy_dt,@actvy_mn,@actvy_dy,@actvy_yr)";

                    cmd.Parameters.Add("@whs", SqlDbType.VarChar);
                    cmd.Parameters.Add("@pfx", SqlDbType.VarChar);
                    cmd.Parameters.Add("@no", SqlDbType.Int);
                    cmd.Parameters.Add("@pwc", SqlDbType.VarChar);
                    cmd.Parameters.Add("@wgt", SqlDbType.Int);
                    cmd.Parameters.Add("@actst", SqlDbType.VarChar);
                    cmd.Parameters.Add("@pcs", SqlDbType.Int);
                    cmd.Parameters.Add("@run", SqlDbType.Int);
                    cmd.Parameters.Add("@actvy_mn", SqlDbType.Int);
                    cmd.Parameters.Add("@actvy_dy", SqlDbType.Int);
                    cmd.Parameters.Add("@actvy_yr", SqlDbType.Int);

                    foreach (IPJRAN s in lstIPJRAN)
                    {
                        cmd.Parameters[0].Value = s.whs;
                        cmd.Parameters[1].Value = s.pfx;
                        cmd.Parameters[2].Value = Convert.ToInt32(s.no);
                        cmd.Parameters[3].Value = s.pwc;
                        cmd.Parameters[3].Value = Convert.ToInt32(s.wgt);
                        cmd.Parameters[4].Value = Convert.ToInt32(s.actst);
                        cmd.Parameters[3].Value = Convert.ToInt32(s.pcs);
                        cmd.Parameters[3].Value = Convert.ToInt32(s.run);
                        cmd.Parameters[5].Value = Convert.ToInt32(s.mn);
                        cmd.Parameters[6].Value = Convert.ToInt32(s.dy);
                        cmd.Parameters[7].Value = Convert.ToInt32(s.yr);

                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    // Shouldn't lave a Transaction hanging, so rollback
                    trans.Rollback();
                    throw;
                }
                try
                {
                    // Get count of rows inserted into IMPORT
                    cmd.CommandText = "SELECT COUNT(no) from ST_IMPORT_tbl_IPJRAN";
                    r = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // No matter what close and dispose of the connetion
                conn.Close();
                conn.Dispose();
            }

            return r;
        }

        public int Write_IMPORT_to_IPJRAN(string date1, string date2)
        {
            // Returning rows inserted into IMPORT
            int r = 0;

            SqlConnection conn = new SqlConnection(STRATIXDataConnString);

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;

                // Call SP to copy IMPORT to IPJRAN table.  Return rows inserted.
                cmd.CommandText = "ST_proc_IMPORT_to_IPJRAN";

                AddParamToSQLCmd(cmd, "@date1", SqlDbType.DateTime, 4, ParameterDirection.Input, date1);
                AddParamToSQLCmd(cmd, "@date2", SqlDbType.DateTime, 4, ParameterDirection.Input, date2);
                AddParamToSQLCmd(cmd, "@rows", SqlDbType.Int, 8, ParameterDirection.Output);

                cmd.ExecuteNonQuery();

                r = Convert.ToInt32(cmd.Parameters["@rows"].Value);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // No matter what close and dispose of the connetion
                conn.Close();
                conn.Dispose();
            }

            return r;
        }
    }
}
