using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"http://localhost:53626/ApiTask.aspx?TripId={tripId}");
                    string responseBody = await response.Content.ReadAsStringAsync();


                    if (response.IsSuccessStatusCode)
                    {

                        if (response.Content.Headers.ContentType.MediaType == "application/json")
                        {

                            var tasks = JsonConvert.DeserializeObject<List<TaskItem>>(responseBody);

                            gvTasks.DataSource = tasks;
                            gvTasks.DataBind();
                        }
                        else
                        {
                            Response.Write($"Erro: Conteúdo retornado não é JSON.");
                        }
                    }
                    else
                    {
                        Response.Write($"Erro ao carregar tarefas. Status: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write($"Erro ao carregar tarefas: {ex.Message}");
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
            if (e.CommandName == "EditTask")
            {
                // Lógica para editar a tarefa
                int taskId = Convert.ToInt32(e.CommandArgument);
                // Exemplo: Carregar a tarefa e permitir edição
            }
            else if (e.CommandName == "DeleteTask")
            {
                // Lógica para excluir a tarefa
                int taskId = Convert.ToInt32(e.CommandArgument);
                // Exemplo: Excluir a tarefa e atualizar a lista
            }
        }
    }
    }
