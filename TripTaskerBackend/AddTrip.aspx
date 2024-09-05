<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTrip.aspx.cs" Inherits="TripTaskerBackend.AddTrip" Async="true" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Minhas viagens</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblTitle" runat="server" Text="Título da Viagem:"></asp:Label>
            <asp:TextBox ID="txtTitle" runat="server" />
            <asp:Button ID="btnCreate" runat="server" Text="Criar" OnClick="BtnCreate_Click" />
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False" />

            <asp:Button ID="btnEdit" runat="server" Text="Editar" OnClick="BtnEdit_Click" Enabled="False" />
            <asp:Button ID="btnDelete" runat="server" Text="Excluir" OnClick="BtnDelete_Click" Enabled="False" />
        </div>

        <div>
            <asp:GridView ID="GridViewTrips" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanged="GridViewTrips_SelectedIndexChanged" OnRowCommand="GridViewTrips_RowCommand">
                <Columns>
                    <asp:BoundField DataField="TripId" HeaderText="ID" />
                    <asp:BoundField DataField="Title" HeaderText="Título" />
                    <asp:ButtonField Text="Selecionar" CommandName="Select" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkAddTasks" runat="server" CommandName="AddTasks" CommandArgument='<%# Eval("TripId") %>' Text="Tarefas" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- armazenar o TripId selecionado -->
        <asp:HiddenField ID="hfSelectedTripId" runat="server" />
    </form>
</body>
</html>
