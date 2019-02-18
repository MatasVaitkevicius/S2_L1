<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forma1.aspx.cs" Inherits="S2_L1_Web.Forma1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="XX-Large" Text="LD_16 Pažintis"></asp:Label>
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Skaityti ir vykdyti"
                BackColor="#888888" Font-Bold="True" Font-Names="Arial" Font-Size="Large"
                Height="26px" Width="210px" />
            <br />
            <br />
            <asp:TextBox ID="TextBox1" runat="server" Height="900px" TextMode="MultiLine"
                Width="1600px" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
            <br />
        </div>
    </form>
</body>
</html>
