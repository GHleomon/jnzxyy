using System;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;
using System.Web.Script.Serialization;


namespace BeginScreen
{
    /// <summary>
    /// RelationsWaitingAreaBigScreenHandler 的摘要说明
    /// </summary>
    public class RelationsWaitingAreaBigScreenHandler : Handler1
    {

        static int glop = 1;
        private DateTime _beginDate;
        private DateTime _endDate;
        private int dSum; //数据表的总记录
        private int startRecond; //起始记录
        private int endRecond; //结束记录
        private static int PageSum = 9; //每页显示的记录数
        private int countPage; //总页数
        private int currentPage; //当前页

        private DataTable dts;


        #region 配置文件的属性
        public static XElement xmlOpe = null;
        private string fontSize = "27px";
        private string fontFamily = "黑体";
        private string bodyBgColor = "#000000";
        private string titleBgColor = "#62D377";
        private string titleColBgColor = "#2D9131";
        private string bottomBgColor = "#2D9131";
        private string rowOddBgColor = "#000000";
        private string rowEvenBgColor = "#000000";
        private string opeBeforColor = "DodgerBlue";
        //private string opeInColor = "red";
        private string opeInColor = "yellow";
        private string opeAfterColor = "rgb(32, 218, 112)";
        private string opeInColor1 = "Orange";
        private string opeAfterColor1 = "Wheat";
        private string defaultColor = "LightSkyBlue";           //1
        private string PatientNames = "";
        private string OpeTime = "";
        #endregion

        private string body = "";

        private static void InitConfig()
        {
            #region 加载配置文件
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory + "ConfigBigScreen.xml";
            xmlOpe = XElement.Load(baseDirectory);
            #endregion
        }

        public string GetHtml(HttpContext context)
        {
            //try
            //{
            //设置显示信息的开始和结束时间
            InitConfig();
            SetViewTime();
            currentPage = int.Parse(xmlOpe.Element("currentPage").Value);
            PatientNames = xmlOpe.Element("PatientName").Value;
            OpeTime = xmlOpe.Element("OpeTime").Value;
            DataTable dt = new DataTable();
            dt = PublicMethod.GetPlanNoticeNew(_beginDate, _endDate, OpeTime == "" ? "5" : OpeTime);
            DataRow[] sqzb = dt.Select("Szstate = '0' and InRoomTime  is not null and OperationBeginTime is null and OperationEndTime is null and OutRoomTime is null");
            DataRow[] ssjxz = dt.Select("Szstate = '0' and InRoomTime is not null and OperationBeginTime is not null and OperationEndTime is null and OutRoomTime is null");
            DataRow[] ssjxz1 = dt.Select("Szstate = '0' and InRoomTime is not null and OperationBeginTime is not null and  OperationEndTime is not null and OutRoomTime is null");
            DataRow[] shhf = dt.Select("InRoomTime is not null and OperationBeginTime is not null and OperationEndTime is not null and'" + DateTime.Now + "' < OutRoomTime");
            DataRow[] lkssj = dt.Select("InRoomTime is not null and OperationBeginTime is not null and OperationEndTime is not null and '" + DateTime.Now + "' > OutRoomTime");
            dts = dt.Clone();
            foreach (DataRow row in sqzb)
            {
                dts.Rows.Add(row.ItemArray);
            }

            foreach (DataRow row in ssjxz)
            {
                dts.Rows.Add(row.ItemArray);
            }

            foreach (DataRow row in ssjxz1)
            {
                dts.Rows.Add(row.ItemArray);
            }

            foreach (DataRow row in shhf)
            {
                dts.Rows.Add(row.ItemArray);
            }

            foreach (DataRow row in lkssj)
            {
                dts.Rows.Add(row.ItemArray);
            }
            //总记录数
            dSum = dts.Rows.Count;
            countPage = GetPageCount();
            string tdRows = string.Empty;
            if (HttpContext.Current.Request["WorkerCurrentPage"] != null && HttpContext.Current.Request["WorkerCurrentPage"] != "")
            {
                LoadWaitNurseOpe(Convert.ToInt32(HttpContext.Current.Request["WorkerCurrentPage"]));
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            message msg = new message(true, body, countPage.ToString(), "");
            context.Response.Write(jss.Serialize(msg));//返回给前台页面  
            context.Response.End();

            //}
            //catch (Exception exp)
            //{
            //    body = " <div style='position: absolute;top: 50%;left: 50%;width: 800px;height: 50px;margin-top: -25px; margin-left: -400px; background-color:#000;font: bold 48px/50px Verdana, Geneva, sans-serif; '>";
            //    body += "<marquee direction='left' scrollamount='5' style='font: bold 48px/50px Verdana, Geneva, sans-serif; color:#FF0000'>";
            //    body += exp.Message + " 与数据服务器连接中断，请稍候......</marquee></div>";

            //    PublicMethod.WriteLog(exp);
            //}

            return body;
        }


        #region //计算总页数

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

        /// <summary>
        ///     加载所有手术状态为“术前、术中、”的申请信息
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        private void LoadWaitNurseOpe(int curPage)
        {
            //每次去查询时，要更新一下记录总数，RecondSum,每一页显示的记录数为，PageSum ,
            body = "";

            ///创建表
            //CreateTable();
            //创建表头
            //insertTitle();
            ///创建列名

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
                    #region 内容的着色设置
                    if (i >= dSum)
                    {
                        tdRow += "<td colspan='9'>　</td>";
                    }
                    else
                    {
                        string intApplyID = dts.Rows[i]["OperationApplyId"].ToString();
                        string OperationRoom = dts.Rows[i]["SqOperationRoom"].ToString();// dts.Rows[i]["SZOperationRoom"].ToString() == "" ? dts.Rows[i]["SqOperationRoom"].ToString() : dts.Rows[i]["SZOperationRoom"].ToString();
                        string vchrOpeStatus = PublicMethod.GetOpeStatus(dts.Rows[i]);
                        if (vchrOpeStatus == "") continue;


                        string rowStyle = "";
                        if (i % 2 == 0)
                        {
                            rowStyle = "bgcolor='" + rowEvenBgColor + "' align=left valie=middle style='border-bottom:1px solid #629069;height:50px;font-size:" + 
                                fontSize + ";font-weight:bold; vertical-align: middle;font-family:\"" + fontFamily + "\"; ";
                        }
                        else
                        {
                            rowStyle = "bgcolor='" + rowOddBgColor + "' align=left valie=middle style='border-bottom:1px solid #629069;height:50px;font-size:" + 
                                fontSize + ";font-weight:bold; vertical-align: middle;font-family:\"" + fontFamily + "\"; ";
                        }
                        switch (vchrOpeStatus)
                        {
                            case "等待手术":
                                rowStyle += " color:" + defaultColor + ";'";
                                break;
                            case "手术准备":
                                rowStyle += " color:" + opeBeforColor + ";'";  //2
                                break;
                            case "手术进行中":
                                rowStyle += " color:" + opeInColor + ";'";     //3
                                break;
                            case "术后恢复":
                                rowStyle += " color:" + opeAfterColor + ";'";  //4
                                break;
                            case "手术结束转恢复室":
                                OperationRoom = dts.Rows[i]["PACUBed"].ToString();
                                rowStyle += " color:" + opeInColor1 + ";'";   //6
                                break;
                            case "手术结束安返病房":
                                rowStyle += " color:" + opeAfterColor1 + ";'";  //57
                                break;
                            case "离开恢复室":
                                OperationRoom = dts.Rows[i]["PACUBed"].ToString();
                                rowStyle += " color:" + opeAfterColor1 + ";'";  //57
                                break;
                            case "手术结束":
                                rowStyle += " color:" + opeAfterColor1 + ";'";  //57
                                break;
                            default:
                                break;
                        }

                    #endregion
                        //注释时间列改为从床号 LM
                        //vchrFactBeginTime = dts.Rows[i]["OperationBeginTime"].ToString();// == "" ? dts.Rows[i]["OrderOperationTime"].ToString() : dts.Rows[i]["InRoomTime"].ToString();
                        //if (vchrFactBeginTime.Trim().ToString() != "")
                        //{
                        //    vchrFactBeginTime = Convert.ToDateTime(vchrFactBeginTime).ToString("HH:mm");
                        //}
                        //else vchrFactBeginTime = "- -";
                        tdRow += "<td align='center' " + rowStyle + "></td>";
                        tdRow += "<td align='center' " + rowStyle + ">" + dts.Rows[i]["DepartmentName"] + "</td>";
                        tdRow += "<td align='center' " + rowStyle + ">" + dts.Rows[i]["Bed"] + "</td>";
                        string PatientName = "";
                        if (PatientNames == "")
                        {
                            PatientName = dts.Rows[i]["PatientName"].ToString();
                        }
                        else
                        {
                            string name = dts.Rows[i]["PatientName"].ToString().Length < 2 ? "" : dts.Rows[i]["PatientName"].ToString().Substring(2);
                            PatientName = dts.Rows[i]["PatientName"].ToString().Substring(0, 1) + PatientNames + name;
                        }
                        tdRow += "<td align='center' " + rowStyle + ">&nbsp" + PatientName + "</td>";
                        //tdRow += "<td align='center' " + rowStyle + ">&nbsp" + OperationRoom + "</td>";
                        //tdRow += "<td align='center' " + rowStyle + ">&nbsp" + vchrFactBeginTime + "　</td>";
                        tdRow += "<td align='center' " + rowStyle + ">" + vchrOpeStatus + "</td>";
                    }
                    tdRow += "</tr>";
                    tdRows += tdRow;
                }
                //显示备注等
                viewRemark();
                body += tdRows;
                //插入底部
                insertBottom();
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        /// <summary>
        ///     显示备注
        /// </summary>
        private void viewRemark()
        {
            GetT_Inform();
        }


        /// <summary>
        ///     获取通知档的内容
        /// </summary>
        private void GetT_Inform()
        {
            string messige = PublicMethod.getMessige();
            if (messige.Trim() == "") return;
            //body += "<script language='javascript'> setInterval(someFunction, 20000);function someFunction() {window.location.reload();}</script>";
            body += " <div style='position: absolute;top: 50%;left: 50%;width: 1000px;height: 50px;margin-top: -25px; margin-left:-500px; background-color:#000;font: bold 55px/57px Verdana, Geneva, sans-serif; '>";
            body += "<marquee direction='left' scrollamount='12' style='font: bold 55px/57px Verdana, Geneva, sans-serif; color:#d808d6'>";
            body += messige + " </marquee></div>";
        }

        private void SetViewTime()
        {
            _beginDate = PublicMethod.ServerTime().Date.AddSeconds(1);
            _endDate = PublicMethod.ServerTime().Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        #region 创建大屏公告列表

        private void CreateTable()
        {
            //body += "<script language='javascript'> setInterval(someFunction, 10000);function someFunction() {window.location.reload();}</script>";
            body += "<table border=0 width='100%' height='100%' cellpadding=0 cellspacing=0>";
            body += "{title}";
            body += "{columens}";
            body += "{rows}";
            body += "</table>";
        }

        /// <summary>
        ///     定义列
        /// </summary>
        /// <param name="colNames"></param>
        /// <param name="colWidths"></param>
        private void insertCols(string[] colNames, string[] colWidths)
        {
            string colString = "";
            string colContent = "";
            colString += "<tr style='background:" + titleColBgColor + ";color:white;margin-bottom:6px;font-size:" +
                         fontSize + ";font-weight:bold;font-family:\"" + fontFamily + "\";'>{td}</tr>";
            for (int i = 0; i < colNames.Length; i++)
            {
                colContent += "<td width = '" + colWidths[i] +
                              "' align='center' valign='middle' height='5%' style='padding:6px;'>" + colNames[i] +
                              "</td>";
            }
            colString = colString.Replace("{td}", colContent);
            body = body.Replace("{columens}", colString);

        }

        /// <summary>
        ///     插入表头
        /// </summary>
        private void insertTitle()
        {
            if (countPage == 0)
            {
                currentPage = 0;
            }
            string titleStr = "";
            titleStr += "<tr style='background:black;color:white;margin-bottom:6px;font-size:40px;font-weight:bold;font-family:\"" + fontFamily + "\";'>";
            titleStr += "<td colspan='9' align='center' valign='middle' height='10%' style='padding:6px;'>";
            titleStr += "<DIV style='left:10px; top: 22px; position: absolute;color:" + titleBgColor + ";font-size:" + fontSize + ";'>" + WebConfigurationManager.AppSettings["BigScreenHospitalName"].ToString();
            titleStr += "</DIV>今日手术<DIV style='right: 10px; top: 22px; position: absolute;color:" + titleBgColor + ";font-size:" + fontSize + ";'>" + PublicMethod.ServerTime().ToString("yyyy-MM-dd HH:mm") + "\n（第" + currentPage + "屏/共" + countPage + "屏）  </DIV></TD></tr>";
            body = body.Replace("{title}", titleStr);
        }

        /// <summary>
        ///     插入表底部
        /// </summary>
        private void insertBottom()
        {
            string bottomStr = "";
            bottomStr += "<tr>";
            bottomStr += "<td colspan='9' align='center' valign='middle' id='bottom1'></td>";
            bottomStr += "</tr>";
            //body = body.Replace("{Bootom}", bottomStr);
            body += bottomStr;
        }

        #endregion




    }
}