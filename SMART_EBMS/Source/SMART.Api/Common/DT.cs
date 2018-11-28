using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMART.Api
{
    public class DTList
    {
        //Last week（上周）
        public static DTSD LastWeek()
        {
            DateTime NowDate = DateTime.Now;
            DateTime StartDate = NowDate.AddDays(1 - Convert.ToInt32(NowDate.DayOfWeek) - 7); //上周一
            DateTime EndDate = NowDate.AddDays(1 - Convert.ToInt32(NowDate.DayOfWeek) - 7).AddDays(6); //上周周末

            DTSD DT = new DTSD();
            DT.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
            DT.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
            return DT;
        }

        //This week（本周）
        public static DTSD ThisWeek()
        {
            DateTime NowDate = DateTime.Now;
            DateTime StartDate = NowDate.AddDays(1 - Convert.ToInt32(NowDate.DayOfWeek.ToString("d"))); //本周一
            DateTime EndDate = StartDate.AddDays(6); //本周末

            DTSD DT = new DTSD();
            DT.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
            DT.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
            return DT;
        }

        //Next week（下周）
        public static DTSD NextWeek()
        {
            DateTime NowDate = DateTime.Now;
            DateTime StartDate = NowDate.AddDays(1 - Convert.ToInt32(NowDate.DayOfWeek) + 7); //下周一
            DateTime EndDate = NowDate.AddDays(1 - Convert.ToInt32(NowDate.DayOfWeek) + 7).AddDays(6); //下周周末

            DTSD DT = new DTSD();
            DT.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
            DT.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
            return DT;
        }

        //Last Month(上上月)
        public static DTSD LastLastMonth()
        {
            DateTime NowDate = DateTime.Now;
            DateTime StartDate = Convert.ToDateTime(NowDate.AddMonths(-2).ToString("yyyy-MM-01")); //上上月一日
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1); //上上月最后一日
            DTSD DT = new DTSD();

            DT.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
            DT.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
            return DT;
        }

        //Last Month(上月)
        public static DTSD LastMonth()
        {
            DateTime NowDate = DateTime.Now;
            DateTime StartDate = Convert.ToDateTime(NowDate.AddMonths(-1).ToString("yyyy-MM-01")); //上月一日
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1); //上月最后一日
            DTSD DT = new DTSD();

            DT.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
            DT.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
            return DT;
        }

        //This Month(本月)
        public static DTSD ThisMonth()
        {
            DateTime NowDate = DateTime.Now;
            DateTime StartDate = Convert.ToDateTime(NowDate.ToString("yyyy-MM-01")); //本月一日
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1); //本月最后一日
            DTSD DT = new DTSD();

            DT.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
            DT.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
            return DT;
        }

        //Next Month(下月)
        public static DTSD NextMonth()
        {
            DateTime NowDate = DateTime.Now;
            DateTime StartDate = Convert.ToDateTime(NowDate.AddMonths(1).ToString("yyyy-MM-01")); //下月一日
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1); //下月最后一日
            DTSD DT = new DTSD();

            DT.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
            DT.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
            return DT;
        }

        //Next Month(下下月)
        public static DTSD NextNextMonth()
        {
            DateTime NowDate = DateTime.Now;
            DateTime StartDate = Convert.ToDateTime(NowDate.AddMonths(2).ToString("yyyy-MM-01")); //下月一日
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1); //下月最后一日
            DTSD DT = new DTSD();

            DT.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
            DT.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
            return DT;
        }

        //Next Month(下下月)
        public static DTSD NextNextNextMonth()
        {
            DateTime NowDate = DateTime.Now;
            DateTime StartDate = Convert.ToDateTime(NowDate.AddMonths(3).ToString("yyyy-MM-01")); //下月一日
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1); //下月最后一日
            DTSD DT = new DTSD();

            DT.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
            DT.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
            return DT;
        }

        //This Year(今年)
        public static DTSD ThisYear()
        {
            DateTime NowDate = DateTime.Now;
            DateTime StartDate = Convert.ToDateTime(NowDate.ToString("yyyy-01-01")); //本年第一日
            DateTime EndDate = Convert.ToDateTime(NowDate.ToString("yyyy-12-31")); //本年最后一日
            DTSD DT = new DTSD();

            DT.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
            DT.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
            return DT;
        }

        //This Year(去年)
        public static DTSD LastYear()
        {
            DateTime NowDate = DateTime.Now.AddYears(-1);
            DateTime StartDate = Convert.ToDateTime(NowDate.ToString("yyyy-01-01")); //本年第一日
            DateTime EndDate = Convert.ToDateTime(NowDate.ToString("yyyy-12-31")); //本年最后一日
            DTSD DT = new DTSD();

            DT.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
            DT.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
            return DT;
        }


        //获取12个月
        public static List<DTSD> YearAllMonths(string Year)
        {
            List<DTSD> DSEL = new List<DTSD>();
            DTSD DSE = new DTSD();
            for (int i = 1; i <= 12; i++)
            {
                DateTime StartDate = Convert.ToDateTime(Year + "-" + i + "-01 00:00:00"); //当月一日
                DateTime EndDate = StartDate.AddMonths(1).AddDays(-1); //当月最后一日

                DSE = new DTSD();
                DSE.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
                DSE.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
                DSEL.Add(DSE);
            }
            return DSEL;
        }

        //获取时间段内月份数
        public static List<DTSD> MonthsByTimeSlot(DateTime Start, DateTime End)
        {
            int totalMonth = End.Year * 12 + End.Month - Start.Year * 12 - Start.Month;

            List<DTSD> DSEL = new List<DTSD>();
            DTSD DSE = new DTSD();

            DateTime StartDate = Convert.ToDateTime(Start.ToString("yyyy-MM") + "-01 00:00:00"); //开始第一日

            for (int i = 0; i <= totalMonth; i++)
            {
                StartDate = Convert.ToDateTime(Start.ToString("yyyy-MM") + "-01 00:00:00"); //开始第一日
                StartDate = StartDate.AddMonths(i); //当月第一日
                DateTime EndDate = StartDate.AddMonths(1).AddDays(-1); //当月最后一日

                DSE = new DTSD();
                DSE.SD = Convert.ToDateTime(StartDate.ToString("yyyy-MM-dd 00:00:00"));
                DSE.ED = Convert.ToDateTime(EndDate.ToString("yyyy-MM-dd 23:59:59"));
                DSEL.Add(DSE);
            }
            return DSEL;
        }

        //获取三个月月份数
        /// <summary>
        /// Start_Mon = "yyyy-MM"
        /// </summary>
        public static List<DTSD> MonthsByNextList(string Start_Mon)
        {
            List<DTSD> DSEL = new List<DTSD>();

            //当前月份
            DTSD Start_Mon_DT = new DTSD();
            Start_Mon_DT.SD = Convert.ToDateTime(Start_Mon + "-01-01 00:00:00");
            Start_Mon_DT.ED = Convert.ToDateTime(Start_Mon_DT.SD.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            DSEL.Add(Start_Mon_DT);

            //下月
            DTSD Next_Mon_A_DT = new DTSD();
            Next_Mon_A_DT.SD = Convert.ToDateTime(Start_Mon_DT.SD.AddMonths(1).ToString("yyyy-MM-01 00:00:00"));
            Next_Mon_A_DT.ED = Convert.ToDateTime(Next_Mon_A_DT.SD.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            DSEL.Add(Next_Mon_A_DT);


            //下下月
            DTSD Next_Mon_B_DT = new DTSD();
            Next_Mon_B_DT.SD = Convert.ToDateTime(Next_Mon_A_DT.SD.AddMonths(1).ToString("yyyy-MM-01 00:00:00"));
            Next_Mon_B_DT.ED = Convert.ToDateTime(Next_Mon_B_DT.SD.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            DSEL.Add(Next_Mon_B_DT);


            //下下下月
            DTSD Next_Mon_C_DT = new DTSD();
            Next_Mon_C_DT.SD = Convert.ToDateTime(Next_Mon_A_DT.SD.AddMonths(2).ToString("yyyy-MM-01 00:00:00"));
            Next_Mon_C_DT.ED = Convert.ToDateTime(Next_Mon_C_DT.SD.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            DSEL.Add(Next_Mon_C_DT);

            return DSEL;
        }

    }

    public class DTSD
    {
        public DateTime SD { get; set; }
        public DateTime ED { get; set; }
    }

}