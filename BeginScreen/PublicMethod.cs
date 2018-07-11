using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;

namespace BeginScreen
{
    public static class PublicMethod
    {
        public static DateTime ServerTime()
        {
            string sqlStr = "select getdate() as SysDate";
            DataTable dt = DBHelper.GetDataTable(sqlStr);
            return DateTime.Parse(DateTime.Parse(dt.Rows[0]["SysDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        ///  根据时间查询排程公告 
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public static DataTable GetPlanNoticeNew(DateTime dtBegin, DateTime dtEnd, string OpeTime)
        {
            //and sqAnesthesiaDoctor is not null
            string strSql = "select * from (select * from V_OperationInformation where SZstate in(0,4) and  ((InRoomTime >='" + dtBegin.ToString() + "' and InRoomTime<='" + dtEnd.ToString() + "'))  union  select * from V_OperationInformation where   datediff(minute,CONVERT(DATETIME,OutRoomTime,120),GETDATE())<" + OpeTime + " and  datediff(minute,CONVERT(DATETIME,OutRoomTime,120),GETDATE())>-5 and HospitalId=15 ) as a order by SqOperationRoomId asc,InRoomTime desc,SqTableIndex asc";

            return DBHelper.GetDataTable(strSql);
        }

        public static DataTable SelectPlanedOpeByRoom(string whereSql)
        {
            try
            {
                string sql = "select CONVERT(VARCHAR(16),OperationRoom.Orderby)+'-'+CONVERT(VARCHAR(16),TableIndex) as 术间,RIGHT(CONVERT(VARCHAR(16),OrderOperationTime,120),11) as 时间,V_PlanedOpe.Name as 姓名,Sex as 性别,BirthDay as 年龄,subString(InHospitalNo,4,12) as 住院号,Bed as 床号,Diagnose as 术前诊断,operation as 拟施手术,OperationDoctor as 手术者,Assistant1+','+Assistant2+','+Assistant3 as 助手,AnaesthesiaMethodName as 拟施麻醉,AnesthesiaDoctor as 麻醉医生,InstrumentNurse as 洗手,TourNurse as 巡回 from V_PlanedOpe left join OperationRoom on OperationRoom.Id=PlanOperationRoom where " + whereSql + " Order By OperationRoom.OrderBy,TableIndex";
                return DBHelper.GetDataTable(sql);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///  公告显示 
        /// </summary>
        /// <returns></returns>
        public static string getMessige()
        {
            string strSql = "select top 1 Contents from NoticeContent where  datediff(second,CONVERT(DATETIME,OperatorTime,120),GETDATE())<ReleaseTime order by ID desc ";
            DataTable dt = DBHelper.GetDataTable(strSql);
            string messige = "";
            if (dt.Rows.Count > 0)
            {
                messige = dt.Rows[0][0].ToString();
            }
            return messige;
        }
        /// <summary>
        ///  根据手术ID查询恢复室信息 
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public static DataTable GetPlanNoticeNew(string OperationRecordId)
        {
            string strSql = "select * from V_OperationInformation where ParentId=" + OperationRecordId;
            return DBHelper.GetDataTable(strSql);
        }



        public static string GetOpeStatus(DataRow dts)
        {
            //--0.【等待手术】   “已排程”“待访视”“已访视”
            //--1.【术前准备】   点击“手术转入”
            //--2.【手术进行中】 点击“手术开始”
            //--3.【术后恢复】   点击“手术结束”
            //--4.【离开手术室】 从点击手术结束
            //--5.【转入复苏室】 麻醉恢复记录单点击“转入”
            //--6.【离开手术室】 点击“转出”恢复室
            string OperationRecordId = dts["OperationRecordId"].ToString();
            string OpeStatus = dts["Szstate"].ToString();
            string InRoomTime = dts["InRoomTime"].ToString();
            string OperationBeginTime = dts["OperationBeginTime"].ToString();
            string OperationEndTime = dts["OperationEndTime"].ToString();
            string OutRoomTime = dts["OutRoomTime"].ToString();
            string Temp = "";
            if ((dts["SQstate"].ToString() == "2" && OperationRecordId == "") || (dts["SQstate"].ToString() == "1" && OperationRecordId == "" && dts["PlanOperationTime"].ToString() != "" && dts["SQOperationRoomId"].ToString() != "" && dts["APassApply"].ToString() != "0"))//&& dts["sqAnesthesiaDoctor"].ToString() != "" 
            {
                Temp = "等待手术";
            }
            if (OpeStatus == "0" && InRoomTime != "" && OperationBeginTime == "" && OperationEndTime == "" && OutRoomTime == "")
            {
                Temp = "手术准备";
            }
            if (OpeStatus == "0" && InRoomTime != "" && OperationBeginTime != "" && OperationEndTime == "" && OutRoomTime == "")
            {
                Temp = "手术进行中";
            }
            if (OpeStatus == "0" && InRoomTime != "" && OperationBeginTime != "" && OperationEndTime != "" && OutRoomTime == "")
            { 
                if (DateTime.Now < Convert.ToDateTime(OperationEndTime))
                {
                    Temp = "手术进行中";
                }
                else
                {
                    Temp = "术后恢复";
                }
            }
            if (InRoomTime != "" && OperationBeginTime != "" && OperationEndTime != "" && OutRoomTime != "")
            {
                if (DateTime.Now < Convert.ToDateTime(OutRoomTime))
                    Temp = "术后恢复";
                else
                    if (dts["Whereabouts"].ToString() == "病房")
                    {
                        Temp = "手术结束安返病房";
                    }
                    else if (dts["Whereabouts"].ToString() == "恢复室")
                    {
                        Temp = "手术结束转恢复室";
                    }
                    else
                    {
                        Temp = "手术结束";
                    }
            }
            if (OpeStatus == "4" && InRoomTime != "" && OutRoomTime == "")
            {
                Temp = "转入复苏室";
            }
            if (OpeStatus == "5" && InRoomTime != "" && OutRoomTime != "")
            {
                Temp = "离开手术室";
            }
            return Temp;
        }

        /// <summary>
        /// 将异常打印到LOG文件
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="LogAddress">日志文件地址</param>
        public static void WriteLog(Exception ex, string LogAddress = "")
        {
            try
            {
                //如果日志文件为空，则默认在Debug目录下新建 YYYY-mm-dd_Log.log文件
                if (LogAddress == "")
                {
                    LogAddress = Environment.CurrentDirectory + '\\' +
                        DateTime.Now.Year + '-' +
                        DateTime.Now.Month + '-' +
                        DateTime.Now.Day + "_Log.log";
                }
                //把异常信息输出到文件
                StreamWriter sw = new StreamWriter(LogAddress, true);
                sw.WriteLine("当前时间：" + DateTime.Now.ToString());
                sw.WriteLine("异常信息：" + ex.Message);
                sw.WriteLine("异常对象：" + ex.Source);
                sw.WriteLine("调用堆栈：\n" + ex.StackTrace.Trim());
                sw.WriteLine("触发方法：" + ex.TargetSite);
                sw.WriteLine();
                sw.Close();
            }
            catch (Exception exp)
            {
                return;
            }

        }


    }
}