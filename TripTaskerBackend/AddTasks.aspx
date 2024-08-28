<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTasks.aspx.cs" Inherits="TripTaskerBackend.AddTasks" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tarefas</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Tarefas para Viagem</h2>
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False" />
            <asp:Label ID="lblTripTitle" runat="server" />
            <asp:GridView ID="GridViewTasks" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="TaskId" HeaderText="ID" />
                    <asp:BoundField DataField="Title" HeaderText="Título" />
                    <asp:BoundField DataField="Description" HeaderText="Descrição" />
                    <asp:BoundField DataField="DueDate" HeaderText="Data de Vencimento" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:ButtonField ButtonType="Link" CommandName="EditTask" Text="Editar" />
                    <asp:ButtonField ButtonType="Link" CommandName="DeleteTask" Text="Excluir" />
                </Columns>
            </asp:GridView>
            <asp:TextBox ID="txtTitle" runat="server" Placeholder="Título" />
            <asp:TextBox ID="txtDescription" runat="server" Placeholder="Descrição" />
            <asp:TextBox ID="txtDueDate" runat="server" Placeholder="Data de Vencimento" />
            <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem Value="Pending">Pendente</asp:ListItem>
                <asp:ListItem Value="InProgress">Em Progresso</asp:ListItem>
                <asp:ListItem Value="Completed">Concluída</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnCreateTask" runat="server" Text="Adicionar Tarefa" OnClick="btnCreateTask_Click" />
        </div>
    </form>
</body>
</html>
