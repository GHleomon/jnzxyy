<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchedulingPlan.aspx.cs"
    Inherits="BeginScreen.SchedulingPlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>手术排程</title> 
</head>
<body>
    <form id="form1" runat="server">
     <asp:Label ID="Label1" runat="server" Text="Label">科室</asp:Label>
     <asp:TextBox ID="tbJxrq1" runat="server" tag="date"></asp:TextBox>
     <asp:Label ID="Label3" runat="server" Text="Label">术者</asp:Label>
     <asp:TextBox ID="tbJxrq3" runat="server" tag="date"></asp:TextBox>
     <asp:Label ID="Label2" runat="server" Text="Label">排程时间</asp:Label>
     <asp:TextBox ID="tbJxrq2"
        runat="server" tag="date"></asp:TextBox> 
    <asp:Button ID="Button1" runat="server" Text="查询" onclick="Button1_Click" />
    <asp:Label ID="tishji" runat="server" Text="Label"></asp:Label>
    <div>
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="SelectPlanedOpeByRoom"
            TypeName="BeginScreen.PublicMethod"></asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
