using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Data.Entity;
using static TripTaskerBackend.Tasks;
using static TripTaskerBackend.TaskHelpers;
using Newtonsoft.Json;

namespace TripTaskerBackend
{
    public partial class ApiTask : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                switch (Request.HttpMethod)
                {
                    case "GET":
                        await HandleGetRequestAsync();
                        break;
                    case "POST":
                        await HandlePostRequestAsync();
                        break;
                    default:
                        Response.StatusCode = 405;
                        break;
                }
            }
            
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.Write($"Erro interno: {ex.Message}");
            }
        }


        private async Task HandleGetRequestAsync()
        {
            int tripId;
            if (int.TryParse(Request.QueryString["TripId"], out tripId))
            {
                using (var context = new AppDbContext())
                {
                    var tasks = await context.Tasks
                                             .Where(t => t.TripId == tripId)
                                             .Select(t => new {
                                                 t.Title,
                                                 t.Description,
                                                 t.Status,
                                                 t.DueDate
                                             })
                                             .ToListAsync(); 

                    var json = new JavaScriptSerializer().Serialize(tasks);
                    Response.ContentType = "application/json";
                    Response.Clear(); 
                    Response.Write(json);
                    Response.End(); 
                }
            }
            else
            {
                Response.StatusCode = 400;
                Response.Clear(); // Limpa qualquer conteúdo anterior
                Response.Write("Id da viagem inválido");
                Response.End(); // Encerra a resposta
            }
        }

        private async Task HandlePostRequestAsync()
        {
            string title = Request.Form["Title"];
            int tripId;

     
            if (int.TryParse(Request.Form["TripId"], out tripId) && !string.IsNullOrEmpty(title))
            {
                using (var context = new AppDbContext())
                {
                    var task = new TaskItem
                    {
                        Title = title,
                        TripId = tripId,
                        Status = TaskProgress.ToDo,
                        DueDate = DateTime.Now
                    };
                    context.Tasks.Add(task);
                    await context.SaveChangesAsync();
                    Response.StatusCode = 201;
                    Response.Write("Tarefa criada com sucesso.");
                }
            }
            else
            {
                Response.StatusCode = 400;
                Response.Write("Dados inválidos para criação da tarefa.");
            }
        }
    }
}
