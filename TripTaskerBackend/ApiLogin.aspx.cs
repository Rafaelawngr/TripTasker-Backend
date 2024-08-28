using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TripTaskerBackend
{
    public partial class ApiLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                var username = Request.Form["username"];
                var password = Request.Form["password"];

                var pwd = PasswordHelper.HashPassword(password);

                using (var context = new AppDbContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == pwd);
                    if (user != null)
                    {
                        Response.StatusCode = 200;
                        Response.Write("Login successful");
                        Page.Visible = false;
                    }
                    else
                    {
                        Response.StatusCode = 401;
                        Response.Write("Invalid username or password");
                        Page.Visible = false;
                    }
                }
            }
            else
            {
                if (Request.HttpMethod == "GET")
                {
                    using (var context = new AppDbContext())
                    {
                        var users = context.Users.Select(u => new { u.Id, u.Username, u.Password }).ToList();
                        var serializer = new JavaScriptSerializer();
                        var json = serializer.Serialize(users);

                        Response.ContentType = "application/json";
                        Response.StatusCode = 200;
                        Response.Write(json);
                        Page.Visible = false;
                    }
                }
            }
        }
    }
}