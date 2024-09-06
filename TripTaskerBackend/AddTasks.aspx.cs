using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TripTaskerBackend
{
    public partial class AddTasks : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tripId;
                if (int.TryParse(Request.QueryString["TripId"], out tripId))
                {
                    hfSelectedTripId.Value = tripId.ToString();
                    await LoadTasks(tripId);
                }
                else
                {
                    Response.Write("ID da viagem inválido.");
                }
            }
        }

        protected async void btnCreateTask_Click(object sender, EventArgs e)
        {
            string title = txtTaskTitle.Value;
            string description = txtTaskDescription.Value;
            string dueDateStr = txtDueDate.Value;
            string statusStr = ddlStatus.SelectedValue;

            int tripId;
            DateTime dueDate;
            int status;

            if (int.TryParse(hfSelectedTripId.Value, out tripId) &&
                !string.IsNullOrEmpty(title) &&
                !string.IsNullOrEmpty(description) &&
                DateTime.TryParse(dueDateStr, out dueDate) &&
                int.TryParse(statusStr, out status))
            {
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
            {
                { "Title", title },
                { "Description", description },
                { "DueDate", dueDate.ToString("yyyy-MM-dd") },
                { "Status", status.ToString() },
                { "TripId", tripId.ToString() }
            };

                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync("http://localhost:53626/ApiTask.aspx", content);

                    if (response.IsSuccessStatusCode)
                    {
                        Response.Write("Tarefa criada com sucesso.");
                        await LoadTasks(tripId);
                    }
                    else
                    {
                        Response.Write("Falha ao criar tarefa.");
                    }
                }
            }
            else
            {
                Response.Write("Erro: Dados inválidos para criação da tarefa.");
            }
        }


        private async Task LoadTasks(int tripId)
        {
            using (var context = new AppDbContext())
            {
               
                var tasks = await context.Tasks
                    .Where(t => t.TripId == tripId)
                    .ToListAsync(); 

                
                var taskViewModels = tasks.Select(t => new
                {
                    t.TaskId,
                    t.Title,
                    t.Description,
                    Status = TaskHelpers.GetTaskStatusString(t.Status), 
                    DueDate = t.DueDate.ToString("dd-MM-yyyy") 
                }).ToList();

                gvTasks.DataSource = taskViewModels;
                gvTasks.DataBind();
            }
        }

        protected void GridViewTrips_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddTasks")
            {
                int tripId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"AddTasks.aspx?TripId={tripId}");
            }
        }

        protected void gvTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectTask")
            {
                // Verifica se o índice da linha foi obtido corretamente
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                if (gvTasks.DataKeys != null && gvTasks.DataKeys.Count > rowIndex)
                {
                    // Obtém o ID da tarefa a partir da fonte de dados
                    int taskId = (int)gvTasks.DataKeys[rowIndex].Value;

                    // Armazena o ID da tarefa no HiddenField
                    hfSelectedTaskId.Value = taskId.ToString();

                    // Exibe os botões de editar e excluir
                    btnEditTask.Visible = true;
                    btnDeleteTask.Visible = true;
                }
                else
                {
                    Response.Write("Erro: Não foi possível selecionar a tarefa.");
                }
            }
        }

        protected async void btnEditTask_Click(object sender, EventArgs e)
        {
            int taskId = int.Parse(hfSelectedTaskId.Value); 
            string title = txtTaskTitle.Value;              
            string description = txtTaskDescription.Value;  
            string dueDateStr = txtDueDate.Value;           
            string statusStr = ddlStatus.SelectedValue;     

            DateTime dueDate;
            int status;

            
            if (!string.IsNullOrEmpty(title) &&
                !string.IsNullOrEmpty(description) &&
                DateTime.TryParse(dueDateStr, out dueDate) &&
                int.TryParse(statusStr, out status))
            {
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
            {
                { "Action", "edit" },
                { "TaskId", taskId.ToString() },
                { "Title", title },
                { "Description", description },
                { "DueDate", dueDate.ToString("dd-MM-yyyy") },
                { "Status", status.ToString() },
            };

                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync("http://localhost:53626/ApiTask.aspx", content);

                    if (response.IsSuccessStatusCode)
                    {
                        Response.Write("Tarefa editada com sucesso.");
                        await LoadTasks(int.Parse(hfSelectedTripId.Value)); // Recarrega as tarefas
                    }
                    else
                    {
                        Response.Write("Falha ao editar a tarefa.");
                    }
                }
            }
            else
            {
                Response.Write("Erro: Dados inválidos para edição da tarefa.");
            }
        }

        protected async void btnDeleteTask_Click(object sender, EventArgs e)
{
    int taskId = int.Parse(hfSelectedTaskId.Value); // Recupera o ID da tarefa selecionada

    using (var client = new HttpClient())
    {
        var values = new Dictionary<string, string>
        {
            { "Action", "delete" },
            { "TaskId", taskId.ToString() }
        };

        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync("http://localhost:53626/ApiTask.aspx", content);

        if (response.IsSuccessStatusCode)
        {
            Response.Write("Tarefa excluída com sucesso.");
            await LoadTasks(int.Parse(hfSelectedTripId.Value)); // Recarrega as tarefas
        }
        else
        {
            Response.Write("Falha ao excluir a tarefa.");
        }
    }
}
    }
}