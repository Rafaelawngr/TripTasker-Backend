using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace TripTaskerBackend
{
    public partial class ApiLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Request.HttpMethod == "OPTIONS")
            {
                Response.StatusCode = 200;
                Response.End();
                return;
            }

            if (Request.HttpMethod == "POST")
            {
                string username = Request.Form["username"];
                string password = Request.Form["password"];

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    string passwordHash = PasswordHelper.HashPassword(password);

                    using (var context = new AppDbContext())
                    {
                        var user = context.Users
                            .FirstOrDefault(u => u.Username == username && u.Password == passwordHash);

                        if (user != null)
                        {
                            Response.StatusCode = 200;
                            Response.Write("Login successful");
                        }
                        else
                        {
                            Response.StatusCode = 401;
                            Response.Write("Invalid username or password");
                        }
                        Response.End();
                    }
                }
                else
                {
                    Response.StatusCode = 400;
                    Response.Write("Username and password are required.");
                    Response.End();
                }
            }
            else if (Request.HttpMethod == "GET")
            {
                using (var context = new AppDbContext())
                {
                    var users = context.Users
                        .Select(u => new { u.Id, u.Username })
                        .ToList(); 

                    var serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(users);

                    Response.ContentType = "application/json";
                    Response.StatusCode = 200;
                    Response.Write(json);
                    Response.End();
                }
            }
        }
    }
}
