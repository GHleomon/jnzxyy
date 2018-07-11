<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="D.aspx.cs" Inherits="BeginScreen.RelationsWaitingAreaBigScreen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script src="js/jquery.min.js" type="text/javascript"></script>
</head>
<body style="margin: 0;" bgcolor="#000000" id="Mybody">
    <table id="table" table border="0" width='100%' height='100%' cellpadding="0" cellspacing="0">
        <thead>
            <tr style="background: black; color: white; margin-bottom: 6px; font-size: 40px;
                font-weight: bold;">
                <td colspan="10" align="center" valign="middle" height="10%" style="padding: 6px;">
                    <div style="left: 10px; top: 22px; position: absolute; color: #62D377; font-size: 27px;">
                        内蒙古医科大学附属医院
                    </div>
                    今日手术
                    <div style="right: 250px; top: 22px; position: absolute; color: #62D377; font-size: 27px;">
                        <div id="currentTime">
                        </div>
                    </div>
                    <div style="right: 20px; top: 22px; position: absolute; color: #62D377; font-size: 27px;">
                        <div id ="js">
                        </div>
                    </div>
                </td>
            </tr>
            <tr style="background: #2D9131; color: white; margin-bottom: 6px; font-size: 24px;
                font-weight: bold;">
                <td width="1%" align="center" valign="middle" height="5%" style="padding: 6px;">
                </td>
                <td width="20%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    科室
                </td>
                <td width="15%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    床号
                </td>
                <td width="16%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    姓名
                </td>
<%--                <td width="24%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    手术间
                </td>--%>
                <td width="24%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    状态
                </td>
            </tr>
        </thead>
        <tbody id="Mytbody">
        </tbody>
    </table>
</body>
<script type="text/javascript">

    var tbodyobj = document.getElementById('Mytbody');
    setInterval(load, 10000)
    var WorkerCurrentPage = 1
    var countPage
    function load() {
        if (WorkerCurrentPage > countPage) {
            WorkerCurrentPage = 1;
        }
        $.ajax({
            type: "POST",
            url: "RelationsWaitingAreaBigScreenHandler.ashx?cmd=GetHtml&WorkerCurrentPage=" + WorkerCurrentPage,
            dataType: 'json',
            async: false,
            success: function (result) {
                var data = eval(result);
                if (data.success) {
                    tbodyobj.innerHTML = data.msg
                    countPage = data.countPage
                    $("#js").html('（第' + WorkerCurrentPage + '屏/共' + data.countPage + '屏）')
                }
            }
        });
        WorkerCurrentPage = WorkerCurrentPage + 1
    }

    $(function () {
        var _timestamp
        //setInterval("$('#currentTime').text(new Date().toLocalsString());",1000);   
        $.ajax({
            type: "POST",
            url: "OpeRoomPlanNoticeHandler.ashx?cmd=GetHtmlTime",
            async: false,
            success: function (result) {
                _timestamp = Date.parse(result);
                //_timestamp = _timestamp.toString().match(/^\d$/) ? _timestamp : new Date().getTime();
                setInterval(function () {
                    $("#currentTime").text(new Date(_timestamp).toLocaleTimeString());
                    _timestamp += 1000;
                }, 1000);

            }
        });
    });  


</script>
</html>
