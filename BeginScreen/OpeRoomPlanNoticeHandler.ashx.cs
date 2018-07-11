using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml.Linq;
using System.Web.Script.Serialization;

namespace BeginScreen
{

    /// <summary>
    /// OpeRoomPlanNoticeHandler 的摘要说明
    /// </summary>
    public class OpeRoomPlanNoticeHandler : Handler1
    {


        private DateTime dtpBeginDate;
        private DateTime dtpEndDate;
        private int dSum = 0; //数据表的总记录
        private int startRecond; //起始记录
        private int endRecond = 0; //结束记录
        private int PageSum = 9; //每页显示的记录数
        private static int countPage = 0; //总页数
        private int WorkerCurrentPage = 1; //当前页
        private DataTable dts;
        private string opeInColor1 = "Orange";

        #region 配置文件的属性
        public XElement xmlOpe = null;

        private string fontFamily = "黑体";
        private string fontSize = "24px";
        private string bodyBgColor = "#000000";
        private string titleBgColor = "#62D377";
        private string titleColBgColor = "#2D9131";
        private string rowOddBgColor = "#000000";
        private string rowEvenBgColor = "#000000";
        private string opeBeforColor = "#f4fd04";
        private string opeInColor = "#fd0001";
        //private string opeInColor = "lime";
        private string opeAfterColor = "rgb(32, 218, 112)";
        private string defaultColor = "LightSkyBlue";
        private string htmlbody = "";
        private string OpeTime = "";
        #endregion

        private string body = "";



        public void GetHtml(HttpContext context)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory + "ConfigNoticeXML.xml";
            xmlOpe = XElement.Load(baseDirectory);
            WorkerCurrentPage = int.Parse(xmlOpe.Element("WorkerCurrentPage").Value);
            OpeTime = xmlOpe.Element("OpeTime").Value;
            SetViewTime();
            dts = PublicMethod.GetPlanNoticeNew(dtpBeginDate, dtpEndDate, OpeTime == "" ? "5" : OpeTime);
            //总记录数
            dSum = dts.Rows.Count;
            countPage = GetPageCount();
            //currentPage = 1;
            //首先判断数据库中的记录数是否大于每一屏显示的数 chengxg@yahoo.cn;
            string tdRows = string.Empty;
            if (HttpContext.Current.Request["WorkerCurrentPage"] != null && HttpContext.Current.Request["WorkerCurrentPage"] != "")
            {
                tdRows = LoadWaitNurseOpe(Convert.ToInt32(HttpContext.Current.Request["WorkerCurrentPage"]));
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            message msg = new message(true, tdRows, countPage.ToString(),WorkerCurrentPage.ToString());
            context.Response.Write(jss.Serialize(msg));//返回给前台页面  
            context.Response.End();
            //}
            //catch (Exception exp)
            //{
            //    htmlbody = "";
            //    body = "<body style='margin:0 0; padding:0 0; background-color:#000;'>";
            //    body += "<script language='javascript'> setInterval(someFunction, 20000);function someFunction() {window.location.reload();}</script>";
            //    body += " <div style='position: absolute;top: 50%;left: 50%;width: 800px;height: 50px;margin-top: -25px; margin-left: -400px; background-color:#000;font: bold 48px/50px Verdana, Geneva, sans-serif; '>";
            //    body += "<marquee direction='left' scrollamount='5' style='font: bold 48px/50px Verdana, Geneva, sans-serif; color:#FF0000'>";
            //    body += exp.Message + " 与数据服务器连接中断，请稍候......</marquee></div></body>";
            //    htmlbody = body;
            //    PublicMethod.WriteLog(exp);
            //    JavaScriptSerializer jss = new JavaScriptSerializer();
            //    message msg = new message(false, htmlbody);
            //    context.Response.Write(jss.Serialize(msg));//返回给前台页面  
            //    context.Response.End();
            //}
        }

        #region 计算总页数

        public int GetPageCount()
        {
            if (PageSum == 0)
                PageSum = 9; //每页显示的记录条数为"0",则默认为"20" 
            if (dSum % PageSum == 0)
                return (dSum / PageSum);
            else
                return (dSum / PageSum) + 1;
        }

        #endregion


        private string LoadWaitNurseOpe(int curPage)
        {
            htmlbody = "";

            //每次去查询时，要更新一下记录总数，RecondSum,每一页显示的记录数为，PageSum ,
            body = "";
            ///创建表
            //CreateTable();
            //插入表头
            //InsertTitle();
            ///创建列名{ "状态", "手术间","科室", "时间", "姓名", "手术名称", "手术医生", "麻醉医生", "洗手护士", "巡回护士" };
            //string[] colsName = { "", "术间", "科室", "姓名", "手术名称", "手术医生", "麻醉医生", "护士", "时间", "状态" };
            //string[] colsWidth = { "1%", "5%", "13%", "8%", "19%", "9%", "14%", "14%", "8%", "9%" };
            //InsertCols(colsName, colsWidth);

            curPage -= 1;
            startRecond = curPage * PageSum;
            endRecond = startRecond + PageSum;
            try
            {
                //状态vchrOpeStatus 手术间vchrOperatingRoomName 时间BeginTime 姓名vchrPatientName 
                //手术名称vchrOperationName术者 麻醉者 器械 巡回
                string tdRows = "";
                for (int i = startRecond; i < endRecond; i++)
                {
                    string tdRow = "<tr>";
                    if (i >= dSum)
                    {
                        tdRow += "<td colspan='9'>　</td>";
                    }
                    else
                    {
                        #region 内容的着色设置
                        string rowStyle = "";
                        if (i % 2 == 0)
                        {
                            rowStyle = "bgcolor='" + rowEvenBgColor +
                                       "' align=center valie=middle style='border-bottom:1px solid #629069; height:50px; font-size:" +
                                       fontSize + ";font-weight:bold; vertical-align: middle; font-family:\"" + fontFamily + "\"; ";
                        }
                        else
                        {
                            rowStyle = "bgcolor='" + rowOddBgColor +
                                       "' align=center valie=middle style='border-bottom:1px solid #629069; height:50px; font-size:" +
                                       fontSize + ";font-weight:bold;  vertical-align: middle; font-family:\"" + fontFamily + "\"; ";
                        }
                        string intApplyID = dts.Rows[i]["OperationApplyId"].ToString();
                        string vchrOpeStatus = PublicMethod.GetOpeStatus(dts.Rows[i]);
                        //临时手术状态
                        if (vchrOpeStatus == "") continue;
                        string opeStatu = "";
                        string vchrFactBeginTime = "";
                        vchrFactBeginTime = dts.Rows[i]["OperationBeginTime"].ToString();// == "" ? dts.Rows[i]["OrderOperationTime"].ToString() : dts.Rows[i]["InRoomTime"].ToString();
                        if (vchrFactBeginTime.Trim().ToString() != "")
                        {
                            vchrFactBeginTime = Convert.ToDateTime(vchrFactBeginTime).ToString("HH:mm");
                        }
                        else vchrFactBeginTime = "- -";
                        string OperationRoom = dts.Rows[i]["SqOperationRoom"].ToString();// dts.Rows[i]["SZOperationRoom"].ToString() == "" ? dts.Rows[i]["SqOperationRoom"].ToString() : dts.Rows[i]["SZOperationRoom"].ToString();
                        switch (vchrOpeStatus)
                        {
                            case "等待手术":
                                opeStatu = "等待手术";
                                rowStyle += " color:" + defaultColor + ";'";
                                OperationRoom = dts.Rows[i]["SqOperationRoom"].ToString();
                                break;
                            case "术前准备":
                                opeStatu = "术前准备";
                                rowStyle += " color:" + opeBeforColor + ";'";  //2
                                OperationRoom = dts.Rows[i]["SqOperationRoom"].ToString();
                                break;
                            case "手术进行中":
                                opeStatu = "手术中";
                                rowStyle += " color:" + opeInColor + ";'";
                                OperationRoom = dts.Rows[i]["SZOperationRoom"].ToString();
                                break;
                            case "术后恢复":
                                break;
                            case "转入复苏室":
                                opeStatu = "转入复苏室";
                                OperationRoom = dts.Rows[i]["PACUBed"].ToString();
                                rowStyle += " color:" + opeInColor1 + ";'";   //6
                                //OperationRoom = dts.Rows[i]["PACUBed"].ToString();
                                break;
                            case "离开手术室":
                                opeStatu = "手术结束";
                                rowStyle += " color:" + opeAfterColor + ";'";
                                OperationRoom = dts.Rows[i]["SZOperationRoom"].ToString();
                                //OperationRoom = dts.Rows[i]["PACUBed"].ToString();
                                break;
                            case "手术拒绝":
                                opeStatu = "手术拒绝";
                                rowStyle += " color:#B559F5;'";
                                break;
                            default:
                                break;
                        }
                        #endregion
                        tdRow += "<td align='center' " + rowStyle + "></td>";
                        tdRow += "<td align='center' " + rowStyle + ">" + OperationRoom + "</td>";
                        #region 科室和申请科室
                        string Depatrtment = dts.Rows[i]["DepartmentName"].ToString() == "" ? dts.Rows[i]["ApplyDepartmentName"].ToString() : dts.Rows[i]["DepartmentName"].ToString();
                        tdRow += "<td align='center'" + rowStyle + ">" + Depatrtment + "</td>";
                        #endregion
                        tdRow += "<td align='center' " + rowStyle + ">" + dts.Rows[i]["PatientName"] + "</td>";
                        string SZOperation = dts.Rows[i]["SZOperation"].ToString() == "" ? dts.Rows[i]["SQOperation"].ToString() : dts.Rows[i]["SZOperation"].ToString();
                        tdRow += "<td align='center' " + rowStyle + ">" + SZOperation + "</td>";

                        #region 读取医生的条件
                        string SZOperationDoctor = dts.Rows[i]["SZOperationDoctor"].ToString() == "" ? dts.Rows[i]["SQOperationDoctor"].ToString() : dts.Rows[i]["SZOperationDoctor"].ToString();
                        tdRow += "<td align='center' " + rowStyle + ">" + SZOperationDoctor + "</td>";
                        #endregion

                        #region 读取麻醉医生2
                        string SZAnesthesiaDoctor = dts.Rows[i]["SZAnesthesiaDoctor"].ToString() == "" ? dts.Rows[i]["SqAnesthesiaDoctor"].ToString() : dts.Rows[i]["SZAnesthesiaDoctor"].ToString();
                        SZAnesthesiaDoctor = SZAnesthesiaDoctor == "" ? "- -" : SZAnesthesiaDoctor;
                        tdRow += "<td align='center' " + rowStyle + ">" + SZAnesthesiaDoctor + "</td>";
                        #endregion

                        #region 读取器械护士巡回护士
                        string SZTourNurse = dts.Rows[i]["SZTourNurse"].ToString() == "" ? dts.Rows[i]["SQTourNurse"].ToString() : dts.Rows[i]["SZTourNurse"].ToString();
                        string SZInstrumentNurse = dts.Rows[i]["SZInstrumentNurse"].ToString() == "" ? dts.Rows[i]["SQInstrumentNurse"].ToString() : dts.Rows[i]["SZInstrumentNurse"].ToString();
                        if (SZInstrumentNurse == "") SZInstrumentNurse = "- -";
                        if (SZTourNurse == "") SZTourNurse = "- -";
                        tdRow += "<td align='center' " + rowStyle + ">" + SZInstrumentNurse + "/" + SZTourNurse + "</td>";
                        #endregion

                        tdRow += "<td align='center' " + rowStyle + ">" + vchrFactBeginTime + "</td>";
                        tdRow += "<td align='center' " + rowStyle + ">" + opeStatu + "</td>";

                    }
                    tdRow += "</tr>";
                    tdRows += tdRow;
                }
                //htmlbody = tdRows == "" ? htmlbody : body.Replace("{rows}", tdRows);
                return tdRows;
            }
            catch (Exception exp)
            {
                return "";
                throw;
            }
        }


        private void SetViewTime()
        {
            dtpBeginDate = PublicMethod.ServerTime().Date.AddSeconds(1);
            dtpEndDate = PublicMethod.ServerTime().Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            //dtpBeginDate = Convert.ToDateTime("2018-01-16 00:00:01");
            //dtpEndDate = Convert.ToDateTime("2018-01-16 23:59:59");
        }


        public void GetHtmlTime(HttpContext context)
        {
            DateTime dt = PublicMethod.ServerTime();
            context.Response.Write(dt.ToString());//返回给前台页面  
            context.Response.End();

        }
    }
}