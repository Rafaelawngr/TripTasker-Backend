using System;
using System.Web.UI;

namespace TripTaskerBackend
{
    public partial class AddUser : System.Web.UI.Page
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
                        var user = new User
                        {
                            Username = username,
                            Password = passwordHash
                        };

                        context.Users.Add(user);
                        context.SaveChanges();
                    }

                    Response.StatusCode = 200;
                    Response.Write("User created successfully");
                    Response.End();
                }
                else
                {
                    Response.StatusCode = 400;
                    Response.Write("Username and password are required.");
                    Response.End();
                }
            }
        }
    }
}