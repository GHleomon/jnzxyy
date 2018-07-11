<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpeRoomPlanNotice.aspx.cs"
    Inherits="BeginScreen.OpeRoomPlanNotice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="js/jquery.min.js" type="text/javascript"></script>
</head>
<body style='margin: 0;' bgcolor='#000000'>

    <table id="table" table border=0 width='100%' height='100%' cellpadding=0 cellspacing=0>
        <thead>
            <tr style="background: #62D377; color: white; margin-bottom: 6px; font-size: 24px;
                font-weight: bold;">
                <td colspan="10" style="padding: 6px;" valign="middle" height="5%" align="center">
                    <div id="currentTime" style="position: absolute; left: 10px; top: 10px;">
                    </div>
                    每日手术情况一览表
                    <div id="ys" style="position: absolute; right: 10px; top: 10px;">
                    </div>
                </td>
            </tr>
            <tr style="background: #2D9131; color: white; margin-bottom: 6px; font-size: 24px;
                font-weight: bold;">
                <td width="1%%" align="center" valign="middle" height="5%" style="padding: 6px;">
                </td>
                <td width="5%%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    术间
                </td>
                <td width="13%%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    科室
                </td>
                <td width="8%%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    姓名
                </td>
                <td width="19%%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    手术名称
                </td>
                <td width="9%%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    手术医生
                </td>
                <td width="14%%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    麻醉医生
                </td>
                <td width="14%%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    护士
                </td>
                <td width="8%%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    时间
                </td>
                <td width="9%%" align="center" valign="middle" height="5%" style="padding: 6px;">
                    状态
                </td>
            </tr>
        </thead>
        <tbody id="Mytbody">
        </tbody>
    </table>
</body>
<script type="text/javascript">
    setInterval(load, 10000)
    var tbodyobj = document.getElementById('Mytbody');
    var WorkerCurrentPage = 1
    var countPage

    function load() {

        if (WorkerCurrentPage > countPage) {
            WorkerCurrentPage = 1;
        }
        $.ajax({
            type: "POST",
            url: "OpeRoomPlanNoticeHandler.ashx?cmd=GetHtml&WorkerCurrentPage=" + WorkerCurrentPage,
            dataType: 'json',
            async: false,
            success: function (result) {
                var data = eval(result);
                if (data.success) {
                    tbodyobj.innerHTML = data.msg


                    countPage = data.countPage
                    $("#ys").html('                        （第' + WorkerCurrentPage + '屏/共' + data.countPage + '屏）')
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
                    $("#currentTime").text(new Date(_timestamp).toLocaleString());
                    _timestamp += 1000;
                }, 1000);

            }
        });
    });  

</script>
</html>
