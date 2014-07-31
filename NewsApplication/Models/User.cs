using System;
using System.Configuration;
using System.Text.RegularExpressions;
using Simple.Data;

namespace NewsApplication.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordVerify { get; set; }

        public Boolean Login()
        {
            if (CheckLoginValidity())
            {
                try
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
                    var db = Database.OpenConnection(connectionString.ConnectionString);

                    var exists = db.ArticleContent.User.Find(db.ArticleContent.User.Email == Email &&
                                          db.ArticleContent.User.Password == Password);
                    Console.WriteLine("successfully logged in");
                    return exists != null;
                }
                catch (Exception)
                {
                    Console.WriteLine("login failed");
                    return false;
                }
            }
            return false;
        }

        public Boolean CreateUser()
        {
            if (CheckLoginValidity())
            {
                try
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
                    var db = Database.OpenConnection(connectionString.ConnectionString);
                    var exists = db.ArticleContent.User.Find(db.ArticleContent.User.Email == Email);
                    if (exists == null)
                    {
                        db.ArticleContent.User.Insert(Email: Email, Password: Password, JoinDate: DateTime.Now);
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        private Boolean CheckLoginValidity()
        {
            //var passwordMatch = Password.Equals(PasswordVerify);
            var isEmail = false;
            if (Email != null)
            {
                isEmail = Regex.IsMatch(Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            }
            return isEmail && /*passwordMatch &&*/ Password != null && Email.Contains("@gcmlp.com") && Password.Length > 0;
        }
    }
}