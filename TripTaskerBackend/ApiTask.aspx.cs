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
                    case "PUT":
                        await HandlePutRequestAsync();
                        break;
                    case "DELETE":
                        await HandleDeleteRequestAsync();
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
                Response.Clear();
                Response.Write("Id da viagem inválido");
                Response.End();
            }
        }

        private async Task HandlePostRequestAsync()
        {
            string title = Request.Form["Title"];
            string description = Request.Form["Description"];
            string dueDateStr = Request.Form["DueDate"];
            int tripId;
            DateTime dueDate;

            if (int.TryParse(Request.Form["TripId"], out tripId) &&
                DateTime.TryParse(dueDateStr, out dueDate) &&
                !string.IsNullOrEmpty(title))
            {
                using (var context = new AppDbContext())
                {
                    var task = new TaskItem
                    {
                        Title = title,
                        Description = description,
                        TripId = tripId,
                        Status = TaskProgress.ToDo,
                        DueDate = dueDate
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

        protected async Task HandleDeleteRequestAsync()
        {
            int taskId;
            if (int.TryParse(Request.QueryString["taskId"], out taskId))
            {
                using (var context = new AppDbContext())
                {
                    var task = await context.Tasks.FindAsync(taskId);
                    if (task != null)
                    {
                        context.Tasks.Remove(task);
                        await context.SaveChangesAsync();
                        Response.StatusCode = 200;
                        Response.Write("Tarefa excluída com sucesso.");
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        Response.Write("Tarefa não encontrada.");
                    }
                }
            }
            else
            {
                Response.StatusCode = 400;
                Response.Write("ID de tarefa inválido.");
            }
        }

        protected async Task HandlePutRequestAsync()
        {
            int taskId;
            if (int.TryParse(Request.Form["TaskId"], out taskId))
            {
                using (var context = new AppDbContext())
                {
                    var task = await context.Tasks.FindAsync(taskId);
                    if (task != null)
                    {
                        task.Title = Request.Form["Title"];
                        task.Description = Request.Form["Description"];
                        task.Status = (TaskProgress)Enum.Parse(typeof(TaskProgress), Request.Form["Status"]);
                        task.DueDate = DateTime.Parse(Request.Form["DueDate"]);

                        await context.SaveChangesAsync();
                        Response.StatusCode = 200;
                        Response.Write("Tarefa atualizada com sucesso.");
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        Response.Write("Tarefa não encontrada.");
                    }
                }
            }
            else
            {
                Response.StatusCode = 400;
                Response.Write("ID de tarefa inválido.");
            }
        }

    }
}