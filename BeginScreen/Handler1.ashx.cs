using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Data.Common;
using System.Xml;
using System.Web.Script.Serialization;

namespace BeginScreen
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public abstract class Handler1 : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public virtual void OnInit()
        {

        }

        #region 虚 方法
        public void ProcessRequest(HttpContext context)
        {
            OnInit();

            context.Response.ContentType = "text/plain";

            //context.Response.ContentType = "application/json";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";


            try
            {

                if (HttpContext.Current.Request["cmd"] != null)
                {
                    //if (HttpContext.Current.Session["userName"] == null && HttpContext.Current.Request["cmd"] != "userLogin")
                    //{
                    //    string json = ToJson("SessionOut");
                    //    context.Response.Write(json);//返回给前台页面  
                    //    context.Response.End();
                    //}
                    string cmd = HttpContext.Current.Request["cmd"];
                    var method = this.GetType().GetMethod(cmd);
                    if (method != null)
                    {
                        method.Invoke(this, new object[] { context });
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region DataTable转换成Json格式
        /// <summary>  
        /// DataTable转换成Json格式    分页格式
        /// </summary>    
        /// <param name="ds">DataSet</param>   
        /// <returns></returns>    
        public static string Dataset2Json(DataTable dt, int total = -1)
        {
            StringBuilder json = new System.Text.StringBuilder();
            //{"total":5,"rows":[  
            json.Append("{\"total\":");
            if (total == -1)
            {
                json.Append(dt.Rows.Count);
            }
            else
            {
                json.Append(total);
            }
            json.Append(",\"rows\":[");
            //json.Append("[");
            json.Append(DataTable2Json(dt));
            json.Append("]}");
            return json.ToString();
        }
        #endregion

        /// <summary>
        /// 不需要分页的Json格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Dataset1Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                if (dt.Columns.Count > 0)
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                }
                jsonBuilder.Append("},");
            }
            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }

        #region  sql语句分页
        public string GetListByPage(string tbName, string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.Id desc");
            }
            strSql.Append(")AS Row, T.*  from " + tbName + " T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return strSql.ToString();
        }
        #endregion

        #region dataTable转换成Json格式
        /// <summary>    
        /// dataTable转换成Json格式   子方法 
        /// </summary>    
        /// <param name="dt"></param>    
        /// <returns></returns>    
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                if (dt.Columns.Count > 0)
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                }
                jsonBuilder.Append("},");
            }
            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }

            return jsonBuilder.ToString();
        }
        #endregion dataTable转换成Json格式

        #region 私有方法
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        }
        #endregion

        #region List转换成Json
        /// <summary>
        /// List转换成Json
        /// </summary>
        public static string ListToJson<T>(IList<T> list)
        {
            object obj = list[0];
            return ListToJson<T>(list, obj.GetType().Name);
        }

        /// <summary>
        /// List转换成Json 
        /// </summary>
        public static string ListToJson<T>(IList<T> list, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName)) jsonName = list[0].GetType().Name;
            Json.Append("{\"" + jsonName + "\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        Type type = pi[j].GetValue(list[i], null).GetType();
                        Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));

                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        #endregion

        #region 对象转换为Json
        /// <summary> 
        /// 对象转换为Json 
        /// </summary> 
        /// <param name="jsonObject">对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(object jsonObject)
        {
            string jsonString = "{";
            PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                string value = string.Empty;
                if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                {
                    value = "'" + objectValue.ToString() + "'";
                }
                else if (objectValue is string)
                {
                    value = "'" + ToJson(objectValue.ToString()) + "'";
                }
                else if (objectValue is IEnumerable)
                {
                    value = ToJson((IEnumerable)objectValue);
                }
                else
                {
                    value = ToJson(objectValue.ToString());
                }
                jsonString += "\"" + ToJson(propertyInfo[i].Name) + "\":" + value + ",";
            }
            //jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            jsonString = jsonString.Substring(0, jsonString.Length - 1);
            return jsonString + "}";
        }
        #endregion

        #region 对象集合转换Json
        /// <summary> 
        /// 对象集合转换Json 
        /// </summary> 
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString += ToJson(item) + ",";
            }
            //jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            jsonString = jsonString.Substring(0, jsonString.Length - 1);
            return jsonString + "]";
        }
        #endregion

        #region 普通集合转换Json
        /// <summary> 
        /// 普通集合转换Json 
        /// </summary> 
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToArrayString(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = ToJson(item.ToString()) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        #endregion

        #region  DataSet转换为Json
        /// <summary> 
        /// DataSet转换为Json 
        /// </summary> 
        /// <param name="dataSet">DataSet对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + ToJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";
        }
        #endregion

        #region Datatable转换为Json
        /// <summary> 
        /// Datatable转换为Json 
        /// </summary> 
        /// <param name="table">Datatable对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }

        /// <summary>
        /// DataTable转换为Json 
        /// </summary>
        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName)) jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        #endregion

        #region DataReader转换为Json
        /// <summary> 
        /// DataReader转换为Json 
        /// </summary> 
        /// <param name="dataReader">DataReader对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(DbDataReader dataReader)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            while (dataReader.Read())
            {
                jsonString.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type type = dataReader.GetFieldType(i);
                    string strKey = dataReader.GetName(i);
                    string strValue = dataReader[i].ToString();
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (i < dataReader.FieldCount - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            dataReader.Close();
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        #endregion

        #region 错误处理方法
        public class message
        {
            public message(bool success, string msg, string countPage, string workerCurrentPage)
            {
                this.msg = msg;
                this.success = success;
                this.countPage = countPage;
                this.workerCurrentPage = workerCurrentPage;
            }
            public bool success { get; set; }
            public string msg { get; set; }
            public string countPage { get; set; }
            public string workerCurrentPage { get; set; }
        }

        public string Error2String(ENTValidationError.ENTValidationErrors validationErrors)
        {
            string errText = "";
            foreach (ENTValidationError validationError in validationErrors)
            {
                if (errText.Length > 0)
                {
                    errText += "\r\n";
                }

                errText += validationError.ErrorMessage;
            }
            return errText;
        }
        #endregion

        #region  字符串int验证
        public static bool CheckInt(string strValue)
        {
            if (strValue == null || strValue == "")
            {
                return false;
            }
            else
            {
                return System.Text.RegularExpressions.Regex.IsMatch(strValue, @"^\d*$");
            }
        }
        #endregion

        #region DataTable转xml格式字符串
        public static string Serialize(DataTable dt)
        {
            string zqSpecial = "";
            string xmlName = "<control>";
            string xmlNameEnd = "</control>";
            string xmlValuse = "<content>";
            string xmlValuseEnd = "</content>";
            if (dt.Rows.Count <= 0) return "";
            foreach (DataRow row in dt.Rows)
            {
                zqSpecial += "<root>";
                zqSpecial += xmlName + row["name"].ToString() + xmlNameEnd;
                zqSpecial += xmlValuse + ConvertXml(row["value"].ToString()) + xmlValuseEnd;
                zqSpecial += "</root>";
            }
            return "<dataset>" + zqSpecial + "</dataset>";
        }
        #endregion

        #region ShiftXML处理
        /// <summary>
        /// 处理交班记录Xml
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string SerializeShiftXml(DataTable dt)
        {
            string zqSpecial = "";
            string sign = "<sign>";
            string signend = "</sign>";
            string xmlName = "<control>";
            string xmlNameEnd = "</control>";
            string xmlValuse = "<content>";
            string xmlValuseEnd = "</content>";
            string xmlFlag = "<Flag>";
            string xmlFlagEnd = "</Flag>";
            string xmlShift = "<shift>";
            string xmlShiftEnd = "</shift>";
            if (dt.Rows.Count <= 0) return "";
            foreach (DataRow row in dt.Rows)
            {
                zqSpecial += "<root>";
                zqSpecial += sign + row["sign"].ToString() + signend;
                zqSpecial += xmlName + row["name"].ToString() + xmlNameEnd;
                zqSpecial += xmlValuse + ConvertXml(row["value"].ToString()) + xmlValuseEnd;
                zqSpecial += xmlFlag + ConvertXml(row["flag"].ToString()) + xmlFlagEnd;
                zqSpecial += xmlShift + ConvertXml(row["shift"].ToString()) + xmlShiftEnd;
                zqSpecial += "</root>";
            }
            return "<dataset>" + zqSpecial + "</dataset>";
        }
        public static DataTable AnalysisShiftXML(string xml, int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            DataTable dt = new DataTable("Datas");
            dt.Columns.Add("sign", Type.GetType("System.String"));
            dt.Columns.Add("name", Type.GetType("System.String"));
            dt.Columns.Add("value", Type.GetType("System.String"));
            dt.Columns.Add("flag", Type.GetType("System.String"));
            XmlNodeList rows = doc.GetElementsByTagName("root");
            foreach (XmlNode xn in rows)
            {
                DataRow dr = dt.NewRow();
                for (int i = 1; i < xn.ChildNodes.Count - 1; i++)
                {
                    dr[i] = xn.ChildNodes[i].InnerText;
                    if (xn.ChildNodes[1].InnerText == "ShiftId")
                    {
                        dr["value"] = id;
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

        #region XML转义字符处理 frank
        /// <summary>
        /// XML转义字符处理
        /// </summary>
        public static string ConvertXml(string xml)
        {

            xml = (char)1 + xml;   //为了避免首字母为要替换的字符，前加前缀

            for (int intNext = 0; true; )
            {
                int intIndexOf = xml.IndexOf("&", intNext);
                intNext = intIndexOf + 1;  //避免&被重复替换
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&amp;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true; )
            {
                int intIndexOf = xml.IndexOf("<");
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&lt;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true; )
            {
                int intIndexOf = xml.IndexOf(">");
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&gt;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true; )
            {
                int intIndexOf = xml.IndexOf("\"");
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&quot;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true; )
            {
                int intIndexOf = xml.IndexOf("'");
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&apos;" + xml.Substring(intIndexOf + 1);
                }
            }


            return xml.Replace(((char)1).ToString(), "");

        }
        #endregion

        #region 解析xml生成DataTable frank
        public static DataTable AnalysisXML(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            DataTable dt = new DataTable("Datas");
            dt.Columns.Add("name", Type.GetType("System.String"));
            dt.Columns.Add("value", Type.GetType("System.String"));
            XmlNodeList rows = doc.GetElementsByTagName("root");
            foreach (XmlNode xn in rows)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < xn.ChildNodes.Count; i++)
                {
                    dr[i] = xn.ChildNodes[i].InnerText;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion


        #region Json 字符串 转换为 DataTable数据集合
        /// <summary>  
        /// Json 字符串 转换为 DataTable数据集合  
        /// </summary>  
        /// <param name="json"></param>  
        /// <returns></returns>  
        public static DataTable ToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化  
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值  
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中  
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}