using System;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace TripTaskerBackend
{
    public partial class ApiTasks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Request.HttpMethod)
            {
                case "POST":
                    HandlePostRequest();
                    break;
                case "PUT":
                    HandlePutRequest();
                    break;
                case "DELETE":
                    HandleDeleteRequest();
                    break;
                case "GET":
                    HandleGetRequest();
                    break;
                default:
                    Response.StatusCode = 405; // Method Not Allowed
                    break;
            }
        }

        private void HandlePostRequest()
        {
            var action = Request.Form["Action"];
            switch (action.ToLower())
            {
                case "create":
                    CreateTask();
                    break;
                default:
                    Response.StatusCode = 400; // Bad Request
                    Response.Write("Ação não reconhecida");
                    break;
            }
        }

        private void HandlePutRequest()
        {
            var action = Request.Form["Action"];
            if (action == "update")
            {
                UpdateTask();
            }
            else
            {
                Response.StatusCode = 400; // Bad Request
                Response.Write("Ação não reconhecida");
            }
        }

        private void HandleDeleteRequest()
        {
            var action = Request.Form["Action"];
            if (action == "delete")
            {
                DeleteTask();
            }
            else
            {
                Response.StatusCode = 400; // Bad Request
                Response.Write("Ação não reconhecida");
            }
        }

        private void HandleGetRequest()
        {
            int tripId;
            if (int.TryParse(Request.QueryString["tripId"], out tripId))
            {
                GetTasks(tripId);
            }
            else
            {
                Response.StatusCode = 400; // Bad Request
                Response.Write("ID da viagem inválido");
            }
        }

        private void CreateTask()
        {
            var title = Request.Form["title"];
            var description = Request.Form["description"];
            var tripId = int.Parse(Request.Form["tripId"]);
            var status = (Tasks.TaskProgress)Enum.Parse(typeof(Tasks.TaskProgress), Request.Form["status"]);
            var dueDate = DateTime.Parse(Request.Form["dueDate"]);

            using (var context = new AppDbContext())
            {
                var task = new TaskItem
                {
                    Title = title,
                    Description = description,
                    TripId = tripId,
                    Status = status,
                    DueDate = dueDate
                };
                context.Tasks.Add(task);
                context.SaveChanges();
                Response.StatusCode = 201; // Created
                Response.Write("Task created successfully.");
            }
        }

        private void UpdateTask()
        {
            var taskId = int.Parse(Request.Form["taskId"]);
            using (var context = new AppDbContext())
            {
                var task = context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
                if (task != null)
                {
                    task.Title = Request.Form["title"];
                    task.Description = Request.Form["description"];
                    task.Status = (Tasks.TaskProgress)Enum.Parse(typeof(Tasks.TaskProgress), Request.Form["status"]);
                    task.DueDate = DateTime.Parse(Request.Form["dueDate"]);
                    context.SaveChanges();
                    Response.StatusCode = 200; // OK
                    Response.Write("Task updated successfully.");
                }
                else
                {
                    Response.StatusCode = 404; // Not Found
                    Response.Write("Task not found.");
                }
            }
        }

        private void DeleteTask()
        {
            var taskId = int.Parse(Request.Form["taskId"]);
            using (var context = new AppDbContext())
            {
                var task = context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
                if (task != null)
                {
                    context.Tasks.Remove(task);
                    context.SaveChanges();
                    Response.StatusCode = 200; // OK
                    Response.Write("Task deleted successfully.");
                }
                else
                {
                    Response.StatusCode = 404; // Not Found
                    Response.Write("Task not found.");
                }
            }
        }

        private void GetTasks(int tripId)
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
                                   }).ToList();

                if (tasks.Any())
                {
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serializer.Serialize(tasks);
                    Response.ContentType = "application/json";
                    Response.StatusCode = 200; // OK
                    Response.Write(json);
                }
                else
                {
                    Response.StatusCode = 404; // Not Found
                    Response.Write("No tasks found for the given trip.");
                }
            }
        }
    }
}
