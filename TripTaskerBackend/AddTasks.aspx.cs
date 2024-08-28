using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web.UI.WebControls;

namespace TripTaskerBackend
{
    public partial class AddTasks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tripId;
                if (int.TryParse(Request.QueryString["TripId"], out tripId))
                {
                    LoadTasks(tripId);
                }
                else
                {
                    lblError.Text = "ID da viagem inválido.";
                    lblError.Visible = true;
                }
            }
        }

        private void LoadTasks(int tripId)
        {
            using (var context = new AppDbContext())
            {
                var tasks = context.Tasks
                                   .Where(t => t.TripId == tripId)
                                   .ToList();

                GridViewTasks.DataSource = tasks;
                GridViewTasks.DataBind();

                var trip = context.Trips.FirstOrDefault(t => t.TripId == tripId);
                if (trip != null)
                {
                    lblTripTitle.Text = $"Tarefas para a Viagem: {trip.Title}";
                }
            }
        }

        protected void btnCreateTask_Click(object sender, EventArgs e)
        {
            int tripId;
            if (int.TryParse(Request.QueryString["TripId"], out tripId))
            {
                var client = new HttpClient();
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("title", txtTitle.Text),
                    new KeyValuePair<string, string>("description", txtDescription.Text),
                    new KeyValuePair<string, string>("tripId", tripId.ToString()),
                    new KeyValuePair<string, string>("status", ddlStatus.SelectedValue),
                    new KeyValuePair<string, string>("dueDate", txtDueDate.Text)
                });

                var response = client.PostAsync("http://localhost:53626/ApiTasks.aspx", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    LoadTasks(tripId);
                    ClearFields();
                }
                else
                {
                    lblError.Text = $"Erro ao criar tarefa. Status: {response.StatusCode}, Detalhes: {response.Content.ReadAsStringAsync().Result}";
                    lblError.Visible = true;
                }
            }
        }

        private void ClearFields()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtDueDate.Text = "";
            ddlStatus.SelectedIndex = 0;
        }
    }
}
