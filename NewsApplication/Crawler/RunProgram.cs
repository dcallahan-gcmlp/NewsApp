using System.Linq;

namespace NewsApplication.Crawler
{
    public class RunProgram
    {
        public static void Main(string args)
        {

            // Get all the necessary data
            var pull = new DataPull();

            // Create a crawler object
            var crawler = new Crawler();

            // Crawl all requisite sites
            crawler.CrawlSite();

            // Grab all the pages that are crawled AND that are articles
            var articles = crawler.PageRank.Keys.ToList();

            // Create a scorer object and give it all the articles and relevant data
            var scorer = new Scorer(articles, pull);

            // Score the articles
            scorer.ScorePages();
        }
    }
}