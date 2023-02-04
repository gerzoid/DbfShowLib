using System;
using System.Collections.Generic;
using System.Text;

namespace DbfShowLib.DBF
{
    public partial class Dbf : StandartBase, IDataBase
    {
        public static long ToJulian(DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;

            if (month < 3)
            {
                month = month + 12;
                year = year - 1;
            }

            return day + (153 * month - 457) / 5 + 365 * year + (year / 4) - (year / 100) + (year / 400) + 1721119;
        }
        public long ConvertDateToJulian(DateTime date)
        {
            int y, m, d, h, mn, s;
            y = date.Year;
            d = date.Day;
            m = date.Month;
            h = date.Hour;
            mn = date.Minute;
            s = date.Millisecond;
            double jy;
            double ja;
            double jm;
            if (m > 2)
            {
                jy = y;
                jm = m + 1;
            }
            else
            {
                jy = y - 1;
                jm = m + 13;
            }
            double intgr = Math.Floor(Math.Floor(365.25 * jy) + Math.Floor(30.6001 * jm) + d + 1720995);
            //check for switch to Gregorian calendar  
            int gregcal = 15 + 31 * (10 + 12 * 1582);
            if (d + 31 * (m + 12 * y) >= gregcal)
            {
                ja = Math.Floor(0.01 * jy);
                intgr += 2 - ja + Math.Floor(0.25 * ja);
            }

            //correct for half-day offset  
            double dayfrac = h / 24.0 - 0.5;
            if (dayfrac < 0.0)
            {
                dayfrac += 1.0;
                --intgr;
            }
            //now set the fraction of a day  
            double frac = dayfrac + (mn + s / 60.0) / 60.0 / 24.0;
            //round to nearest second  
            double jd0 = (intgr + frac) * 100000;
            double jd = Math.Floor(jd0);
            if (jd0 - jd > 0.5) ++jd;

            return Convert.ToInt64(jd / 100000);
        }
        private DateTime ConvertJulianToDateTime(double julianDate)
        {
            DateTime date;
            double dblA, dblB, dblC, dblD, dblE, dblF;
            double dblZ, dblW, dblX;
            int day, month, year;
            try
            {
                dblZ = Math.Floor(julianDate + 0.5);
                dblW = Math.Floor((dblZ - 1867216.25) / 36524.25);
                dblX = Math.Floor(dblW / 4);
                dblA = dblZ + 1 + dblW - dblX;
                dblB = dblA + 1524;
                dblC = Math.Floor((dblB - 122.1) / 365.25);
                dblD = Math.Floor(365.25 * dblC);
                dblE = Math.Floor((dblB - dblD) / 30.6001);
                dblF = Math.Floor(30.6001 * dblE);
                day = Convert.ToInt32(dblB - dblD - dblF);
                if (dblE > 13)
                {
                    month = Convert.ToInt32(dblE - 13);
                }
                else
                {
                    month = Convert.ToInt32(dblE - 1);
                }
                if ((month == 1) || (month == 2))
                {
                    year = Convert.ToInt32(dblC - 4715);
                }
                else
                {
                    year = Convert.ToInt32(dblC - 4716);
                }
                date = new DateTime(year, month, day);
                return date;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Julian date could not be converted: { ex.Message}");
                date = new DateTime(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting Julian date:  {ex.Message}");
                date = new DateTime(0);
            }
            return date;
        }
        public static DateTime JulianToDateTime(long lJDN)
        {
            double p = Convert.ToDouble(lJDN);
            double s1 = p + 68569;
            double n = Math.Floor(4 * s1 / 146097);
            double s2 = s1 - Math.Floor((146097 * n + 3) / 4);
            double i = Math.Floor(4000 * (s2 + 1) / 1461001);
            double s3 = s2 - Math.Floor(1461 * i / 4) + 31;
            double q = Math.Floor(80 * s3 / 2447);
            double d = s3 - Math.Floor(2447 * q / 80);
            double s4 = Math.Floor(q / 11);
            double m = q + 2 - 12 * s4;
            double j = 100 * (n - 49) + i + s4;
            try
            {
                if ((j > 2100) || (j < 0)) return new DateTime(1800, 01, 01);
                else
                    return new DateTime(Convert.ToInt32(j), Convert.ToInt32(m), Convert.ToInt32(d));
            }
            catch (Exception E)
            {
                return new DateTime(1800, 01, 01);
            }
        }

        public void OpenFile()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
