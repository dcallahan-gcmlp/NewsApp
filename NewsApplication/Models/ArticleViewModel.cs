using System;
using System.Collections.Generic;
using System.Configuration;
using Simple.Data;

namespace NewsApplication.Models
{
    public class ArticleViewModel
    {

        public List<Article> Articles() {
            var today = DateTime.Now;
            var connectionString = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
            var db = Database.OpenConnection(connectionString.ConnectionString);
            List<Article> articles = db.ArticleContent.FindAll(db.ArticleContent.DateAdded > today.AddDays(-2))
                .Select(
                    db.ArticleContent.Url,
                    db.ArticleContent.Title,
                    db.ArticleContent.Blurb,
                    db.ArticleContent.ArticleContentId,
                    db.ArticleContent.Section
                )
                .Where(db.ArticleContent.Score >= 10)
                .OrderByDescending(db.ArticleContent.Score)
                .ThenByDescending(db.ArticleContent.DateAdded);
            foreach (var article in articles)
            {
                article.ArticleComments = db.ArticleComments.All()
                    .Select(
                        db.ArticleComments.Body,
                        db.ArticleComments.ArticleCommentsId
                    )
                    // database reference needs to come first...before c# object reference
                    .Where(db.ArticleComments.ArticleContentId == article.ArticleContentId);
                var url = db.Publication.Find(db.Publication.ArticleContentId == article.ArticleContentId && db.Publication.Date > DateTime.Today && db.Publication.Date < DateTime.Today.AddDays(1));
                    
                if (url == null)
                {
                    db.Publication.Insert(Date: DateTime.Now, ArticleContentId: article.ArticleContentId);
                }
            }
            return articles;
        }

        public List<Article> Articles(DateTime date)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
            var db = Database.OpenConnection(connectionString.ConnectionString);
            List<Article> articles = db.ArticleContent.All()
                .Select(
                    db.ArticleContent.Url,
                    db.ArticleContent.Title,
                    db.ArticleContent.Blurb,
                    db.ArticleContent.ArticleContentId,
                    db.ArticleContent.Section
                )
                .Join(db.Publication)
                .On(db.ArticleContent.ArticleContentId == db.Publication.ArticleContentId)
                .Where(db.Publication.Date > date && db.Publication.Date < date.AddDays(1) && db.ArticleContent.Score >= 10);  // Dates are never going to be exactly equal...need to dates within one day period
            foreach (var article in articles)
            {
                article.ArticleComments = db.ArticleComments.All()
                    .Select(
                        db.ArticleComments.Body,
                        db.ArticleComments.ArticleCommentsId
                    )
                    .Where(db.ArticleComments.ArticleContentId == article.ArticleContentId);
            }
            return articles;
        }

        public List<Sections> DistinctSections()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
            var db = Database.OpenConnection(connectionString.ConnectionString);
            List<Sections> sections = db.Keywords.All()
                .Select(
                    db.Keywords.Section.Distinct()
                );
            return sections;
        }
    }

    public class Article
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Blurb { get; set; }
        public int ArticleContentId { get; set; }
        public List<Comment> ArticleComments { get; set; }
        public string Section { get; set; }
    }

    public class ViewModel
    {
        public List<Article> Articles { get; set; }
        public List<Sections> Sections { get; set; }
    }

    public class Sections
    {
        public string Section { get; set; }
    }
}