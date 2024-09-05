using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TripTaskerBackend
{
    public partial class AddTrip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTrips();
            }
        }

        private void LoadTrips()
        {
            using (var context = new AppDbContext())
            {
                GridViewTrips.DataSource = context.Trips.ToList();
                GridViewTrips.DataBind();
            }
        }

        protected void GridViewTrips_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            GridViewRow selectedRow = GridViewTrips.SelectedRow;
            string tripId = selectedRow.Cells[0].Text;

          
            hfSelectedTripId.Value = tripId;

            btnEdit.Enabled = true;
            btnDelete.Enabled = true;

        }

        protected void GridViewTrips_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddTasks")
            {
                int tripId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"AddTasks.aspx?TripId={tripId}");
            }
        }

        protected async void BtnEdit_Click(object sender, EventArgs e)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("Action", "edit"),
        new KeyValuePair<string, string>("Title", txtTitle.Text),
        new KeyValuePair<string, string>("TripId", hfSelectedTripId.Value)
    });

            try
            {
                var response = await client.PostAsync("http://localhost:53626/ApiTrip.aspx", content);

                if (response.IsSuccessStatusCode)
                {
                    LoadTrips(); 
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    lblError.Text = $"Erro ao editar a viagem. Status: {response.StatusCode}, Detalhes: {responseBody}";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Erro de comunicação: {ex.Message}";
                lblError.Visible = true;
            }
        }


        protected async void BtnCreate_Click(object sender, EventArgs e)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("Action", "create"),
        new KeyValuePair<string, string>("Title", txtTitle.Text)
    });

            try
            {
                var response = await client.PostAsync("http://localhost:53626/ApiTrip.aspx", content);

                if (response.IsSuccessStatusCode)
                {
                    LoadTrips(); 
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    lblError.Text = $"Erro ao criar a viagem. Status: {response.StatusCode}, Detalhes: {responseBody}";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Erro de comunicação: {ex.Message}";
                lblError.Visible = true;
            }
        }


        protected async void BtnDelete_Click(object sender, EventArgs e)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("Action", "delete"),
        new KeyValuePair<string, string>("TripId", hfSelectedTripId.Value)
    });

            try
            {
                var response = await client.PostAsync("http://localhost:53626/ApiTrip.aspx", content);

                if (response.IsSuccessStatusCode)
                {
                    LoadTrips(); 
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    lblError.Text = $"Erro ao excluir a viagem. Status: {response.StatusCode}, Detalhes: {responseBody}";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Erro de comunicação: {ex.Message}";
                lblError.Visible = true;
            }
        }
    }
}
