using System;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Text;
using STXtoSQL.Models;

namespace STXtoSQL.DataAccess
{
    public class ODBCData : Helpers
    {
        public List<IPJRAN> Get_IPJRAN(string date1, string date2)
        {

            List<IPJRAN> lstIPJRAN = new List<IPJRAN>();

            OdbcConnection conn = new OdbcConnection(ODBCDataConnString);
            //OdbcConnection conn = new OdbcConnection("DSN=Invera;UID=livcalod;Pwd=livcalod");


            try
            {
                conn.Open();

                // Try to split with verbatim literal
                OdbcCommand cmd = conn.CreateCommand();

                cmd.CommandText = @"select ran_whs,ran_rec_pfx,ran_rec_no,ran_pwc,ran_tot_wgt,ran_actst_ltts,ran_tot_pcs,ran_tot_run_tm,
                                        MONTH(ran_actst_ltts) as mn,DAY(ran_actst_ltts) as dy,YEAR(ran_actst_ltts) as yr
                                        from ipjran_rec
	                                    where ran_whs = 'SW' and ran_rec_no != 0 and ran_pwc in('60S','72S','CTL','MSB','SHR')
                                        and ran_actvy_dt >= '" + date1 + "' and ran_actvy_dt <= '" + date2 + "'";



                OdbcDataReader rdr = cmd.ExecuteReader();

                using (rdr)
                {
                    while (rdr.Read())
                    {
                        IPJRAN b = new IPJRAN();

                        b.whs = rdr["ran_whs"].ToString();
                        b.pfx = rdr["ran_rec_pfx"].ToString();
                        b.no = Convert.ToInt32(rdr["ran_rec_no"]);
                        b.pwc = rdr["ran_pwc"].ToString();
                        b.wgt = Convert.ToInt32(rdr["ran_tot_wgt"]);
                        b.actst = rdr["ran_actst_ltts"].ToString();
                        b.pcs = Convert.ToInt32(rdr["ran_tot_pcs"]);
                        b.run = Convert.ToInt32(rdr["ran_tot_run_tm"]);
                        b.mn = Convert.ToInt32(rdr["mn"]);
                        b.dy = Convert.ToInt32(rdr["dy"]);
                        b.yr = Convert.ToInt32(rdr["yr"]);

                        lstIPJRAN.Add(b);
                    }
                }
            }
            catch (OdbcException)
            {
                throw;
                //Console.WriteLine("MultDetail odbc ex: " + ex.Message);
            }
            catch (Exception)
            {
                throw;
                //Console.WriteLine("MultDetail other ex: " + ex.Message);
            }
            finally
            {
                // No matter what close and dispose of the connetion
                conn.Close();
                conn.Dispose();
            }

            return lstIPJRAN;
        }
    }
}
