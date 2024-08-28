using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace TripTaskerBackend
{
    public partial class ApiTrip : System.Web.UI.Page
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
                case "PUT":
                    HandlePutRequest();
                    break;
                case "DELETE":
                    HandleDeleteRequest();
                    break;
                default:
                    Response.StatusCode = 405; // Method Not Allowed
                    break;
            }
        }

        private void HandleGetRequest()
        {
            using (var context = new AppDbContext())
            {
                var trips = context.Trips.ToList();
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(trips);

                Response.ContentType = "application/json";
                Response.StatusCode = 200;
                Response.Write(json);
                Response.End();
            }
        }

        private void HandlePostRequest()
        {
            var action = Request.Form["Action"];
            var title = Request.Form["Title"];
            var tripId = Request.Form["TripId"];

            if (string.IsNullOrEmpty(action))
            {
                Response.StatusCode = 400; // Bad Request
                Response.Write("Ação obrigatória não especificada");
                return;
            }

            using (var context = new AppDbContext())
            {
                switch (action.ToLower())
                {
                    case "create":
                        if (string.IsNullOrEmpty(title))
                        {
                            Response.StatusCode = 400; // Bad Request
                            Response.Write("Campo de título obrigatório");
                            return;
                        }

                        var trip = new Trip { Title = title };
                        context.Trips.Add(trip);
                        context.SaveChanges();
                        Response.StatusCode = 201; // Created
                        Response.Write("Viagem criada com sucesso");
                        break;

                    case "edit":
                        if (string.IsNullOrEmpty(tripId) || !int.TryParse(tripId, out int id))
                        {
                            Response.StatusCode = 400; // Bad Request
                            Response.Write("ID da viagem inválido");
                            return;
                        }

                        var tripToEdit = context.Trips.FirstOrDefault(t => t.TripId == id);
                        if (tripToEdit != null)
                        {
                            tripToEdit.Title = title;
                            context.SaveChanges();
                            Response.StatusCode = 200; // OK
                            Response.Write("Viagem editada com sucesso");
                        }
                        else
                        {
                            Response.StatusCode = 404; // Not Found
                            Response.Write("Viagem não encontrada");
                        }
                        break;

                    case "delete":
                        if (string.IsNullOrEmpty(tripId) || !int.TryParse(tripId, out int deleteId))
                        {
                            Response.StatusCode = 400; // Bad Request
                            Response.Write("ID da viagem inválido");
                            return;
                        }

                        var tripToDelete = context.Trips.FirstOrDefault(t => t.TripId == deleteId);
                        if (tripToDelete != null)
                        {
                            context.Trips.Remove(tripToDelete);
                            context.SaveChanges();
                            Response.StatusCode = 200; // OK
                            Response.Write("Viagem excluída com sucesso");
                        }
                        else
                        {
                            Response.StatusCode = 404; // Not Found
                            Response.Write("Viagem não encontrada");
                        }
                        break;

                    default:
                        Response.StatusCode = 400; // Bad Request
                        Response.Write("Ação não reconhecida");
                        break;
                }
            }
        }

        private void HandlePutRequest()
        {
            // Você pode adicionar lógica para PUT, se necessário
        }

        private void HandleDeleteRequest()
        {
            // A lógica para DELETE pode ser implementada aqui se necessário
        }
    }
}
