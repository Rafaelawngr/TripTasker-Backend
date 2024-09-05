<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTasks.aspx.cs" Inherits="TripTaskerBackend.AddTasks" Async="true" %>

<!DOCTYPE html>
<html>
<head>
    <title>Adicionar Tarefas</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Adicionar Tarefa</h2>
            <asp:HiddenField ID="hfSelectedTripId" runat="server" />
            
            <label for="txtTaskTitle">Título:</label>
            <input type="text" id="txtTaskTitle" runat="server" />

            <label for="txtTaskDescription">Descrição:</label>
            <input type="text" id="txtTaskDescription" runat="server" />

            <label for="txtDueDate">Data de Vencimento:</label>
            <input type="date" id="txtDueDate" runat="server" />

            <label for="ddlStatus">Status:</label>
            <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem Value="0">A Fazer</asp:ListItem>
                <asp:ListItem Value="1">Fazendo</asp:ListItem>
                <asp:ListItem Value="2">Completo</asp:ListItem>
            </asp:DropDownList>

            <asp:Button ID="btnCreateTask" runat="server" Text="Criar Tarefa" OnClick="btnCreateTask_Click" />
        </div>

        <div>
            <h2>Tarefas</h2>
            <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Title" HeaderText="Título" />
                    <asp:BoundField DataField="Description" HeaderText="Descrição" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="DueDate" HeaderText="Data de Vencimento" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
