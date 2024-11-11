using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Lichvannien
    {
        enum Can { Giáp, Ất, Bính, Đinh, Mậu, Kỷ, Canh, Tân, Nhâm, Quý }
        enum Chi { Tý, Sửu, Dần, Mão, Thìn, Tỵ, Ngọ, Mùi, Thân, Dậu, Tuất, Hợi }
        string[] Arr_Thu = new string[] { "Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };
        string[] Arr_Chi = new string[] { "Tý", "Sửu", "Dần", "Mão", "Thìn", "Tỵ", "Ngọ", "Mùi", "Thân", "Dậu", "Tuất", "Hợi" };
        string[] Arr_Can = new string[] { "Giáp", "Ất", "Bính", "Đinh", "Mậu", "Kỷ", "Canh", "Tân", "Nhâm", "Quý" };

        public string CanChiNam(int yy)
        {
            return "năm " + (Can)((yy + 6) % 10) + " " + (Chi)((yy + 8) % 12) + " (" + yy + ")";
            //Trả về kết quả vd: Tân Mùi (1991)
        }

        public string CanChiThang(int mm, int yy) //mm, yy = tháng, năm âm lịch
        {
            return "tháng " + (Can)((yy * 12 + mm + 3) % 10) + " " + (Chi)((mm + 1) % 12);
        }

        public int INT(double d)
        {
            return (int)Math.Floor(d);
        }

        public int MOD(int x, int y)
        {
            int z = x - (int)(y * Math.Floor(((double)x / y)));
            if (z == 0)
            {
                z = y;
            }
            return z;
        }
        /// <summary>
        /// Đổi ngày dương lịch ra số ngày Julius
        /// </summary>
        /// <param name="D">Ngày</param>
        /// <param name="M">Tháng</param>
        /// <param name="Y">Năm</param>
        /// <returns></returns>
        public double SolarToJD(int D, int M, int Y)
        {
            double JD;
            if (Y > 1582 || (Y == 1582 && M > 10) || (Y == 1582 && M == 10 && D > 14))
            {
                JD = 367 * Y - INT(7 * (Y + INT((M + 9) / 12)) / 4) - INT(3 * (INT((Y + (M - 9) / 7) / 100) + 1) / 4) + INT(275 * M / 9) + D + 1721028.5;
            }
            else
            {
                JD = 367 * Y - INT(7 * (Y + 5001 + INT((M - 9) / 7)) / 4) + INT(275 * M / 9) + D + 1729776.5;
            }
            return JD;
        }
        /// <summary>
        /// Đổi số ngày Julius ra ngày dương lịch
        /// </summary>
        /// <param name="JD"></param>
        /// <returns></returns>
        public int[] SolarFromJD(double JD)
        {
            int Z, A, alpha, B, C, D, E, dd, mm, yyyy;
            double F;
            Z = INT(JD + 0.5);
            F = (JD + 0.5) - Z;
            if (Z < 2299161)
            {
                A = Z;
            }
            else
            {
                alpha = INT((Z - 1867216.25) / 36524.25);
                A = Z + 1 + alpha - INT(alpha / 4);
            }
            B = A + 1524;
            C = INT((B - 122.1) / 365.25);
            D = INT(365.25 * C);
            E = INT((B - D) / 30.6001);
            dd = INT(B - D - INT(30.6001 * E) + F);
            if (E < 14)
            {
                mm = E - 1;
            }
            else
            {
                mm = E - 13;
            }
            if (mm < 3)
            {
                yyyy = C - 4715;
            }
            else
            {
                yyyy = C - 4716;
            }
            return new int[] { dd, mm, yyyy };
        }
        /// <summary>
        /// Chuyển đổi số ngày Julius sang dương lịch theo giờ GMT+7
        /// </summary>
        /// <param name="JD">Ngày Julius</param>
        /// <returns></returns>
        public int[] LocalFromJD(double JD)
        {
            return SolarFromJD(JD + (7.0 / 24.0));
        }
        /// <summary>
        /// Đổi ngày JD theo giờ GMT + 7
        /// </summary>
        /// <param name="D"></param>
        /// <param name="M"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public double LocalToJD(int D, int M, int Y)
        {
            return SolarToJD(D, M, Y) - (7.0 / 24.0);
        }
        /// <summary>
        /// Tính thời điểm Sóc
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public double NewMoon(int k)
        {
            double T = k / 1236.85;
            double T2 = T * T;
            double T3 = T2 * T;
            double dr = Math.PI / 180;
            double Jd1 = 2415020.75933 + 29.53058868 * k + 0.0001178 * T2 - 0.000000155 * T3;
            Jd1 = Jd1 + 0.00033 * Math.Sin((166.56 + 132.87 * T - 0.009173 * T2) * dr);
            double M = 359.2242 + 29.10535608 * k - 0.0000333 * T2 - 0.00000347 * T3;
            double Mpr = 306.0253 + 385.81691806 * k + 0.0107306 * T2 + 0.00001236 * T3;
            double F = 21.2964 + 390.67050646 * k - 0.0016528 * T2 - 0.00000239 * T3;
            double C1 = (0.1734 - 0.000393 * T) * Math.Sin(M * dr) + 0.0021 * Math.Sin(2 * dr * M);
            C1 = C1 - 0.4068 * Math.Sin(Mpr * dr) + 0.0161 * Math.Sin(dr * 2 * Mpr);
            C1 = C1 - 0.0004 * Math.Sin(dr * 3 * Mpr);
            C1 = C1 + 0.0104 * Math.Sin(dr * 2 * F) - 0.0051 * Math.Sin(dr * (M + Mpr));
            C1 = C1 - 0.0074 * Math.Sin(dr * (M - Mpr)) + 0.0004 * Math.Sin(dr * (2 * F + M));
            C1 = C1 - 0.0004 * Math.Sin(dr * (2 * F - M)) - 0.0006 * Math.Sin(dr * (2 * F + Mpr));
            C1 = C1 + 0.0010 * Math.Sin(dr * (2 * F - Mpr)) + 0.0005 * Math.Sin(dr * (2 * Mpr + M));
            double deltat;
            if (T < -11)
            {
                deltat = 0.001 + 0.000839 * T + 0.0002261 * T2 - 0.00000845 * T3 - 0.000000081 * T * T3;
            }
            else
            {
                deltat = -0.000278 + 0.000265 * T + 0.000262 * T2;
            };
            double JdNew = Jd1 + C1 - deltat;
            return JdNew;
        }
        /// <summary>
        /// Tính vị trí của mặt trời
        /// </summary>
        /// <param name="jdn">Ngày Julius</param>
        /// <returns></returns>
        public double SunLongitude(double jdn)
        {
            double T = (jdn - 2451545.0) / 36525;
            double T2 = T * T;
            double dr = Math.PI / 180;
            double M = 357.52910 + 35999.05030 * T - 0.0001559 * T2 - 0.00000048 * T * T2;
            double L0 = 280.46645 + 36000.76983 * T + 0.0003032 * T2;
            double DL = (1.914600 - 0.004817 * T - 0.000014 * T2) * Math.Sin(dr * M);
            DL = DL + (0.019993 - 0.000101 * T) * Math.Sin(dr * 2 * M) + 0.000290 * Math.Sin(dr * 3 * M);
            double L = L0 + DL;
            L = L * dr;
            L = L - Math.PI * 2 * (INT(L / (Math.PI * 2)));
            return L;
        }
        /// <summary>
        /// Tính tháng âm lịch chứa ngày Đông chí
        /// </summary>
        /// <param name="Y">Năm cần tính</param>
        /// <returns></returns>
        public int[] LunarMonth11(int Y)
        {
            double off = LocalToJD(31, 12, Y) - 2415021.076998695;
            int k = INT(off / 29.530588853);
            double jd = NewMoon(k);
            int[] ret = LocalFromJD(jd);
            double sunLong = SunLongitude(LocalToJD(ret[0], ret[1], ret[2]));
            if (sunLong > 3 * Math.PI / 2)
            {
                jd = NewMoon(k - 1);
            }
            return LocalFromJD(jd);
        }
        /// <summary>
        /// Tính năm âm lịch
        /// </summary>
        /// <param name="Y">Năm dương lịch</param>
        /// <returns></returns>
        public int[][] LunarYear(int Y)
        {
            int[][] ret = null;
            int[] month11A = LunarMonth11(Y - 1);
            double jdMonth11A = LocalToJD(month11A[0], month11A[1], month11A[2]);
            int k = (int)Math.Floor(0.5 + (jdMonth11A - 2415021.076998695) / 29.530588853);
            int[] month11B = LunarMonth11(Y);
            double off = LocalToJD(month11B[0], month11B[1], month11B[2]) - jdMonth11A;
            bool leap = off > 365.0;
            if (!leap)
            {
                ret = new int[13][];
            }
            else
            {
                ret = new int[14][];
            }
            ret[0] = new int[] { month11A[0], month11A[1], month11A[2], 0, 0 };
            ret[ret.Length - 1] = new int[] { month11B[0], month11B[1], month11B[2], 0, 0 };
            for (int i = 1; i < ret.Length - 1; i++)
            {
                double nm = NewMoon(k + i);
                int[] a = LocalFromJD(nm);
                ret[i] = new int[] { a[0], a[1], a[2], 0, 0 };
            }
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i][3] = MOD(i + 11, 12);
            }
            if (leap)
            {
                initLeapYear(ret);
            }
            return ret;
        }
        /// <summary>
        /// Tính tháng nhuận
        /// </summary>
        /// <param name="ret">Dữ liệu năm âm lịch</param>
        public void initLeapYear(int[][] ret)
        {
            double[] sunLongitudes = new double[ret.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                int[] a = ret[i];
                double jdAtMonthBegin = LocalToJD(a[0], a[1], a[2]);
                sunLongitudes[i] = SunLongitude(jdAtMonthBegin);
            }
            bool found = false;
            for (int i = 0; i < ret.Length; i++)
            {
                if (found)
                {
                    ret[i][3] = MOD(i + 10, 12);
                    continue;
                }
                double sl1 = sunLongitudes[i];
                double sl2 = sunLongitudes[i + 1];
                bool hasMajorTerm = Math.Floor(sl1 / Math.PI * 6) != Math.Floor(sl2 / Math.PI * 6);
                if (!hasMajorTerm)
                {
                    found = true;
                    ret[i][4] = 1;
                    ret[i][3] = MOD(i + 10, 12);
                }
            }
        }
        /// <summary>
        /// Đổi ngày dương lịch ra âm lịch
        /// </summary>
        /// <param name="D">Ngày</param>
        /// <param name="M">Tháng</param>
        /// <param name="Y">Năm</param>
        /// <returns></returns>
        public int[] Solar2Lunar(int D, int M, int Y)
        {
            int yy = Y;
            int[][] ly = LunarYear(Y);
            int[] month11 = ly[ly.Length - 1];
            double jdToday = LocalToJD(D, M, Y);
            double jdMonth11 = LocalToJD(month11[0], month11[1], month11[2]);
            if (jdToday >= jdMonth11)
            {
                ly = LunarYear(Y + 1);
                yy = Y + 1;
            }
            int i = ly.Length - 1;
            while (jdToday < LocalToJD(ly[i][0], ly[i][1], ly[i][2]))
            {
                i--;
            }
            int dd = (int)(jdToday - LocalToJD(ly[i][0], ly[i][1], ly[i][2])) + 1;
            int mm = ly[i][3];
            if (mm >= 11)
            {
                yy--;
            }
            return new int[] { dd, mm, yy, ly[i][4] };
            //Nếu ly[i][4] == 1 => tháng mm Nhuận
        }
        /// <summary>
        /// Đổi ngày âm lịch ra dương lịch
        /// </summary>
        /// <param name="D">Ngày</param>
        /// <param name="M">Tháng</param>
        /// <param name="Y">Năm</param>
        /// <param name="leap">
        /// 0: Nếu năm âm lịch không nhuận
        /// 1: Nếu năm âm lịch nhuận
        /// </param>
        /// <returns></returns>
        public int[] Lunar2Solar(int D, int M, int Y, int leap)
        {
            int yy = Y;
            if (M >= 11)
            {
                yy = Y + 1;
            }
            int[][] lunarYear = LunarYear(yy);
            int[] lunarMonth = null;
            for (int i = 0; i < lunarYear.Length; i++)
            {
                int[] lm = lunarYear[i];
                if (lm[3] == M && lm[4] == leap)
                {
                    lunarMonth = lm;
                    break;
                }
            }
            if (lunarMonth != null)
            {
                double jd = LocalToJD(lunarMonth[0], lunarMonth[1], lunarMonth[2]);
                return LocalFromJD(jd + D - 1);
            }
            else
            {
                throw new Exception("Incorrect input!");
            }
        }
        /// <summary>
        /// Xem thông tin ngày dương lịch
        /// </summary>
        /// <param name="D">Ngày</param>
        /// <param name="M">Tháng</param>
        /// <param name="Y">Năm</param>
        /// <returns></returns>
        public string[] DayInfo(int D, int M, int Y)
        {
            string[] vResult = new string[] { "", "", "" };
            // Thứ trong tuần
            DateTime vDate = new DateTime(Y, M, D);
            vResult[0] = Arr_Thu[(byte)vDate.DayOfWeek];
            // Chuyển ngày dương lịch sang âm lịch
            int[] vdata = Solar2Lunar(D, M, Y);
            // Can - Chi tháng
            vResult[1] = CanChiThang(vdata[1], vdata[2]);
            vResult[2] = CanChiNam(vdata[2]);

            return vResult;
        }
    }
}
