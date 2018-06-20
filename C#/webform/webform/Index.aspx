<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" >
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div>
                    <asp:TextBox ID="txtKey" runat="server" AutoPostBack="true" OnTextChanged="txtKey_TextChanged" onkeyup='__doPostBack("ct100$cph$txtKey")'>
                    </asp:TextBox><asp:Button runat="server" Text="搜索" />
                </div>
                <div>
                    <asp:ListBox ID="listBox" runat="server"></asp:ListBox>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
