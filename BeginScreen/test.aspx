<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="BeginScreen.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<meta http-equiv="Content-Type" content="text/html;charset=gbk">
<head>
    <title>�������</title>
    <!--css��ʽ��������ͷ������-->
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
        //����table���������һ�У�Ȼ���ٰѵ�һ���е�������䵽�����ӵ����У������ɾ��table�ĵ�һ��
        function change(table) {
            var row = table.insertRow(table.rows.length); //��table���������һ��,table.rows.length�Ǳ���������
            for (j = 0; j < table.rows[0].cells.length; j++) {//ѭ����һ�е����е�Ԫ������ݣ�����ӵ�����¼ӵ�һ�������У�ע���±��Ǵ�0��ʼ�ģ�
                var cell = row.insertCell(j); //���²����������ӵ�Ԫ��
                cell.height = "24px"; //һ����Ԫ��ĸ߶ȣ���css��ʽ�е�line-height�߶�һ��
                cell.innerHTML = table.rows[0].cells[j].innerHTML; //�����µ�Ԫ������ݣ����������Ҫ���Լ�����
            }
            table.deleteRow(0); //ɾ��table�ĵ�һ��
        };
        function tableInterval() {
            var table = document.getElementById("test"); //��ñ��
            change(table); //ִ�б��change������ɾ����һ�У��������һ�У������й���
        };
        setInterval("tableInterval()", 2000); //ÿ��2��ִ��һ��change�������൱��table�����Ϲ���һ��
    </script>
</head>
<body align="center">
    <h1 style="font-color: blur;">
        �������</h1>
    <table class="table" align="center">
        <thead class="fixedThead" align="center">
            <tr>
                <th>
                    ����
                </th>
                <th>
                    �Ա�
                </th>
                <th>
                    ����
                </th>
            </tr>
        </thead>
        <tbody id="test" class="scrollTbody" align="center">
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    21
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    18
                </td>
            </tr>
            <tr>
                <td>
                    ����
                </td>
                <td>
                    ��
                </td>
                <td>
                    20
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    19
                </td>
            </tr>
            <tr>
                <td>
                    ���
                </td>
                <td>
                    Ů
                </td>
                <td>
                    21
                </td>
            </tr>
        </tbody>
    </table>
</body>
</html>
