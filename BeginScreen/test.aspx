<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="BeginScreen.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<meta http-equiv="Content-Type" content="text/html;charset=gbk">
<head>
    <title>滚动表格</title>
    <!--css样式是锁定表头不动的-->
    <style type="text/css">
        .table
        {
            width: 100%;
            left: 100%;
            border-collapse: collapse;
            border-spacing: 0;
        }
        .fixedThead
        {
            display: block;
            width: 100%;
        }
        .scrollTbody
        {
            display: block;
            height: 416px;
            overflow: hidden;
            width: 100%;
        }
        .table td
        {
            width: 500px;
            border-bottom: #333 1px dashed;
            padding: 5px;
            background-color: #ddd;
        }
        .table th
        {
            width: 500px;
            border-bottom: #333 1px dashed;
            border-top: #333 1px dashed;
            padding: 5px;
            line-height: 24px;
            background-color: #cfc;
        }
        .table tr
        {
            border-bottom: #333 1px dashed;
            line-height: 24px;
            padding: 5px;
        }
        thead.fixedThead tr th:last-child
        {
            color: #FF0000;
        }
    </style>
    <script language="JavaScript">
        //先在table的最后增加一行，然后再把第一行中的数据填充到新增加的行中，最后再删除table的第一行
        function change(table) {
            var row = table.insertRow(table.rows.length); //在table的最后增加一行,table.rows.length是表格的总行数
            for (j = 0; j < table.rows[0].cells.length; j++) {//循环第一行的所有单元格的数据，让其加到最后新加的一行数据中（注意下标是从0开始的）
                var cell = row.insertCell(j); //给新插入的行中添加单元格
                cell.height = "24px"; //一个单元格的高度，跟css样式中的line-height高度一样
                cell.innerHTML = table.rows[0].cells[j].innerHTML; //设置新单元格的内容，这个根据需要，自己设置
            }
            table.deleteRow(0); //删除table的第一行
        };
        function tableInterval() {
            var table = document.getElementById("test"); //获得表格
            change(table); //执行表格change函数，删除第一行，最后增加一行，类似行滚动
        };
        setInterval("tableInterval()", 2000); //每隔2秒执行一次change函数，相当于table在向上滚动一样
    </script>
</head>
<body align="center">
    <h1 style="font-color: blur;">
        滚动表格</h1>
    <table class="table" align="center">
        <thead class="fixedThead" align="center">
            <tr>
                <th>
                    姓名
                </th>
                <th>
                    性别
                </th>
                <th>
                    年龄
                </th>
            </tr>
        </thead>
        <tbody id="test" class="scrollTbody" align="center">
            <tr>
                <td>
                    张三
                </td>
                <td>
                    男
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    李四
                </td>
                <td>
                    男
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    王昕
                </td>
                <td>
                    女
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    李佳
                </td>
                <td>
                    女
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    张三
                </td>
                <td>
                    男
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    李四
                </td>
                <td>
                    男
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    王昕
                </td>
                <td>
                    女
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    李佳
                </td>
                <td>
                    女
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    张三
                </td>
                <td>
                    男
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    李四
                </td>
                <td>
                    男
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    王昕
                </td>
                <td>
                    女
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    李佳
                </td>
                <td>
                    女
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    张三
                </td>
                <td>
                    男
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    李四
                </td>
                <td>
                    男
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    王昕
                </td>
                <td>
                    女
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    李佳
                </td>
                <td>
                    女
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    张三
                </td>
                <td>
                    男
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    李四
                </td>
                <td>
                    男
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    王昕
                </td>
                <td>
                    女
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    李佳
                </td>
                <td>
                    女
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    张三
                </td>
                <td>
                    男
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    李四
                </td>
                <td>
                    男
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    王昕
                </td>
                <td>
                    女
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    李佳
                </td>
                <td>
                    女
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    张三
                </td>
                <td>
                    男
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    李四
                </td>
                <td>
                    男
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    王昕
                </td>
                <td>
                    女
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    李佳
                </td>
                <td>
                    女
                </td>
                <td>
                    21
                </td>
            </tr>
        </tbody>
    </table>
</body>
</html>
