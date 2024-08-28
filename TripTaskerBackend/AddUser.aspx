<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="TripTaskerBackend.AddUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server" method="post" action="AddUser.aspx">
    <div>
        Usuário: <input type="text" name="username" />
        <br />
        Senha: <input type="password" name="password" />
        <br />
        <input type="submit" value="Adicionar usuário" />
    </div>
</form>
</body>
</html>
