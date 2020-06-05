using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STXtoSQL.Log;
using STXtoSQL.DataAccess;
using STXtoSQL.Models;

namespace STXtoSQL_IPJRAN_NET
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LogWrite("MSG", "Start: " + DateTime.Now.ToString());

            // Args will change based on STXtoSQL program goal
            string date1 = "";
            string date2 = "";
            int odbcCnt = 0;
            int insertCnt = 0;
            int importCnt = 0;

            // Get args
            try
            {
                if (args.Length > 0)
                {
                    /*
                     * Must be in format mm/dd/yyyy.  No time part
                     */
                    date1 = args[0].ToString();
                    date2 = args[1].ToString();
                }
                else
                {
                    /*
                     * No args
                     * Date range will be 1st of month to yesterday
                     */
                    DateTime dtToday = DateTime.Today;

                    DateTime dtFirst = new DateTime(dtToday.Year, dtToday.Month, 1);

                    /*
                     * Need date part of datetime.
                     * Time and date are separated by a space, so split the string
                     * and only use the 1st element.
                     */
                    string[] date1Split = dtFirst.ToString().Split(' ');
                    string[] date2Split = dtToday.AddDays(-1).ToString().Split(' ');

                    date1 = date1Split[0];
                    date2 = date2Split[0];
                }
            }
            catch (Exception ex)
            {
                Logger.LogWrite("EXC", ex);
                Logger.LogWrite("MSG", "Return");
                return;
                //Console.WriteLine(ex.Message.ToString());
            }

            #region FromSTRATIX
            ODBCData objODBC = new ODBCData();

            List<IPJRAN> lstIPJRAN = new List<IPJRAN>();

            // Get data from Straix by date range
            try
            {
                lstIPJRAN = objODBC.Get_IPJRAN(date1, date2);
            }
            catch (Exception ex)
            {
                Logger.LogWrite("EXC", ex);
                Logger.LogWrite("MSG", "Return");
                return;
                //Console.WriteLine(ex.Message.ToString());
            }
            #endregion

            #region ToSQL
            SQLData objSQL = new SQLData();

            // Only work in SQL database, if records were retreived from Stratix
            if (lstIPJRAN.Count != 0)
            {
                odbcCnt = lstIPJRAN.Count;

                // Put Stratix data in lstIPJRAN into IMPORT IPJRAN table
                try
                {
                    importCnt = objSQL.Write_IPJRAN_IMPORT(lstIPJRAN);
                }
                catch (Exception ex)
                {
                    Logger.LogWrite("EXC", ex);
                    Logger.LogWrite("MSG", "Return");
                    return;
                    //Console.WriteLine(ex.Message.ToString());
                }

                // Call SP to put IMPORT IPJRAN table data into WIP IPJRAN table
                try
                {
                    //insertCnt = objSQL.Write_IMPORT_to_IPJRAN(date1, date2);
                }
                catch (Exception ex)
                {
                    Logger.LogWrite("EXC", ex);
                    Logger.LogWrite("MSG", "Return");
                    return;
                    //Console.WriteLine(ex.Message.ToString());
                }

                Logger.LogWrite("MSG", "Range=" + date1 + ":" + date2 + " ODBC/IMPORT/INSERT=" + odbcCnt.ToString() + ":" + importCnt.ToString() + ":" + insertCnt.ToString());
            }
            else
                Logger.LogWrite("MSG", "No data");

            Logger.LogWrite("MSG", "End: " + DateTime.Now.ToString());
            #endregion

            // Testing
            Console.WriteLine("Press key to exit");
            Console.ReadKey();
        }
    }
}
