using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Newtonsoft.Json;
using static TripTaskerBackend.Tasks;

namespace TripTaskerBackend
{
    public partial class ApiTask : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Request.HttpMethod)
            {
                case "GET":
                    HandleGetRequest();
                    break;
                case "POST":
                    HandlePostRequest();
                    break;
                default:
                    Response.StatusCode = 405;
                    Response.Write("Método HTTP não suportado");
                    break;
            }
        }

        private void HandleGetRequest()
        {
            int tripId;
            if (int.TryParse(Request.QueryString["TripId"], out tripId))
            {
                using (var context = new AppDbContext())
                {
                    var tasks = context.Tasks
                        .Where(t => t.TripId == tripId)
                        .Select(t => new
                        {
                            t.TaskId,
                            t.Title,
                            t.Description,
                            t.Status,
                            t.DueDate
                        })
                        .ToList();

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

        private void HandlePostRequest()
        {
            string action = Request.Form["Action"]; 

            if (string.IsNullOrEmpty(action))
            {
               
                string title = Request.Form["Title"];
                string description = Request.Form["Description"];
                string dueDateStr = Request.Form["DueDate"];
                var statusStr = Request.Form["Status"];
                int tripId;
                DateTime dueDate;
                int status;

                try
                {
                    if (int.TryParse(Request.Form["TripId"], out tripId) &&
                        DateTime.TryParse(dueDateStr, out dueDate) &&
                        int.TryParse(statusStr, out status) &&
                        !string.IsNullOrEmpty(title))
                    {
                        using (var context = new AppDbContext())
                        {
                            var task = new TaskItem
                            {
                                Title = title,
                                Description = description,
                                TripId = tripId,
                                Status = (TaskProgress)status,
                                DueDate = dueDate
                            };
                            context.Tasks.Add(task);
                            context.SaveChanges();
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
                catch (Exception ex)
                {
                
                    Response.StatusCode = 500;
                    Response.Write($"Erro interno do servidor: {ex.Message}");
                }
            }
            else if (action == "edit")
            {
                HandleEditTask(); 
            }
            else if (action == "delete")
            {
                HandleDeleteTask(); 
            }
            else
            {
                Response.StatusCode = 400;
                Response.Write("Ação não especificada.");
            }
        }


        private void HandleCreateTask()
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
                    context.SaveChanges();
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

        private void HandleEditTask()
        {
            int taskId;
            string title = Request.Form["Title"];
            string description = Request.Form["Description"];
            string dueDateStr = Request.Form["DueDate"];
            string statusStr = Request.Form["Status"];

            DateTime dueDate;
            int status;

           
            if (!int.TryParse(Request.Form["TaskId"], out taskId) ||
                string.IsNullOrEmpty(title) ||
                !DateTime.TryParse(dueDateStr, out dueDate) ||
                !int.TryParse(statusStr, out status)) 
            {
                Response.StatusCode = 400;
                Response.Write("Dados inválidos para edição.");
                return;
            }

            using (var context = new AppDbContext())
            {
                var task = context.Tasks.FirstOrDefault(t => t.TaskId == taskId);

                if (task == null)
                {
                    Response.StatusCode = 404;
                    Response.Write("Tarefa não encontrada.");
                    return;
                }

                task.Title = title;
                task.Description = description;
                task.DueDate = dueDate;
                task.Status = (TaskProgress)status; 

                context.SaveChanges();
                Response.StatusCode = 200;
                Response.Write("Tarefa editada com sucesso.");
            }
        }


        private void HandleDeleteTask()
        {
            int taskId;

            if (!int.TryParse(Request.Form["TaskId"], out taskId))
            {
                Response.StatusCode = 400;
                Response.Write("ID da tarefa inválido.");
                return;
            }

            using (var context = new AppDbContext())
            {
                var task = context.Tasks.FirstOrDefault(t => t.TaskId == taskId);

                if (task == null)
                {
                    Response.StatusCode = 404;
                    Response.Write("Tarefa não encontrada.");
                    return;
                }

                context.Tasks.Remove(task);
                context.SaveChanges();
                Response.StatusCode = 200;
                Response.Write("Tarefa excluída com sucesso.");
            }
        }
    }
}
