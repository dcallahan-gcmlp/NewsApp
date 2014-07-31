using System;
using System.Configuration;
using Simple.Data;

namespace NewsApplication.Models
{
    public class Comment
    {
        public string Body { get; set; }
        public int ArticleContentId { get; set; }
        public int ArticleCommentsId { get; set; }


        public int Save()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
            var db = Database.OpenConnection(connectionString.ConnectionString);
            var comment = db.ArticleComments.Insert(Body: Body, CommentTime: DateTime.Now, ArticleContentId: ArticleContentId, UserId: 1);
            return comment.ArticleCommentsId;
        }

        public void Delete()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
            var db = Database.OpenConnection(connectionString.ConnectionString);
            db.ArticleComments.Delete(ArticleCommentsId: ArticleCommentsId);
        }
    }
}