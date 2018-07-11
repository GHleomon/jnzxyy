using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BeginScreen
{
    public partial class SchedulingPlan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                tbJxrq2.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                Button1_Click(null, null);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Parse(tbJxrq2.Text).Day > DateTime.Now.Day && DateTime.Now.Hour < 12)
                {
                    tishji.Text = "排程未结束 请于12点之后查看！";
                    this.GridView1.DataSource = null; //可以绑定到Gridview 、datalist等数据控件上，此处为Gridview
                    this.GridView1.DataBind();
                    return;
                }
                else
                {
                    string wheresql = "OrderOperationTime>='" + DateTime.Parse(tbJxrq2.Text).ToString("yyyy-MM-dd 00:00:00") + "' and OrderOperationTime<='" + DateTime.Parse(tbJxrq2.Text).ToString("yyyy-MM-dd 23:59:59") + "' and state in (2,3) ";
                    if (tbJxrq1.Text.Trim() != "") wheresql += " and ( ApplyDepartmentName like '%" + tbJxrq1.Text + "%' ) ";
                    if (tbJxrq3.Text.Trim() != "") wheresql += " and ( OperationDoctor like '%" + tbJxrq3.Text + "%') ";
                    DataTable operationApplys = PublicMethod.SelectPlanedOpeByRoom(wheresql);
                    if (operationApplys.Rows.Count > 0)
                    {
                        this.GridView1.DataSource = operationApplys; //可以绑定到Gridview 、datalist等数据控件上，此处为Gridview
                        this.GridView1.DataBind();
                        tishji.Text = DateTime.Parse(tbJxrq2.Text).ToString("yyyy-MM-dd") + " 共(" + operationApplys.Rows.Count + ")台手术";
                        tbJxrq2.Text = DateTime.Parse(tbJxrq2.Text).ToString("yyyy-MM-dd");
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}