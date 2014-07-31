using System;
using System.Collections.Generic;
using System.Configuration;
using Simple.Data;

namespace NewsApplication.Crawler
{
    public class Scorer
    {

        private readonly List<WebPage> _pages;
        private readonly DataPull _pull;

        public Scorer(List<WebPage> pages, DataPull pull)
        {
            _pages = pages;
            _pull = pull;
        }

        public Dictionary<WebPage, int> ScorePages()
        {
            var webPageScores = new Dictionary<WebPage, int>();
            var connectionString = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
            var db = Database.OpenConnection(connectionString.ConnectionString);
            var data = _pull.GetData();
            foreach (var webPage in _pages)
            {
                var articleContentAndPage = webPage.ArticleContent() + " " + webPage.Title;
                var score = 1;
                var section = "";
                foreach (var keyword in data)
                {
                    if (!articleContentAndPage.Contains(keyword.Keyword)) continue;
                    score = score * keyword.Score;
                    section = keyword.Section;
                }
                var url = db.ArticleContent.Find(db.ArticleContent.Url == webPage.PageStructure.Url);
                var title = db.ArticleContent.Find(db.ArticleContent.Title == webPage.Title);
                if (url == null && title == null)
                {
                    db.ArticleContent.Insert(Url: webPage.PageStructure.Url, Title: webPage.Title, Blurb: webPage.Blurb, Score: score, DateAdded: DateTime.Now.ToString("G"), Section: section);
                }
                webPageScores.Add(webPage, score);
            }
            return webPageScores;
        }
    }
}