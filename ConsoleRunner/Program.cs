using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NewsApplication.Crawler;
using NewsApplication.Models;
using Simple.Data;

namespace ConsoleRunner
{
    class Program
    {
        public static void Main(string[] args)
        {
            //WebPage page = new WebPage(new PageStructure("http://www.institutionalinvestorsalpha.com/Article/3354277/Will-Hedge-Fund-Fees-Ever-Come-Down.html", 0, new List<string>() { "class", "article", "id", "ctl00_ContentPlaceHolder1_lblTitle", "tag", "p" /*(second p)*/ }, new Uri("http://www.institutionalinvestorsalpha.com/")));
            //Console.WriteLine(page.Blurb);
            //Console.WriteLine(page.Title);
            //Console.WriteLine(page.Article);



            ////WebPage page2 = new WebPage(new PageStructure("http://www.hedgefundintelligence.com/Article/3353537/AsiaHedge-News/SPARX-targets-growth-with-new-Japan-value-investing-strategy.html", 0, new List<string> { "class", "col_default article", "tag", "h1", "tag", "p" }, new Uri("http://www.hedgefundintelligence.com/")));
            ////Console.WriteLine(page2.Blurb);
            ////Console.WriteLine(page2.Title);
            ////Console.WriteLine(page2.Article);


            //WebPage page3 = new WebPage(new PageStructure("http://www.finalternatives.com/node/27412", 0, new List<string> { "class", "content story ", "class", "title", "tag", "p" }, new Uri("http://www.finalternatives.com/")));
            //Console.WriteLine(page3.Blurb);
            //Console.WriteLine(page3.Title);
            //Console.WriteLine(page3.Article);

            //WebPage page4 = new WebPage(new PageStructure("http://seekingalpha.com/article/2281813-cutting-my-dividends-by-35-percent-improving-my-dividend-growth-portfolio", 0, new List<string> { "id", "article_body", "tag", "h1", "tag", "p" }, new Uri("http://www.seekingalpha.com/")));
            //Console.WriteLine(page4.Blurb);
            //Console.WriteLine(page4.Title);
            //Console.WriteLine(page4.Article);




            WebPage page5 = new WebPage(new PageStructure("http://www.institutionalinvestor.com/Article/3231909/Search/The-Best-Tweets-From-Delivering-Alpha.html?Keywords=grosvenor+capital+management&OrderType=1&PeriodType=4&StartDay=0&StartMonth=1&StartYear=2000&EndDay=0&EndMonth=7&EndYear=2014&ScopeIndex=0#.U7RrVfldX30", 0, new List<string> { "class", "article-body", "class", "title", "tag", "p" }, new Uri("http://institutionalinvestor.com/")));
            Console.WriteLine(page5.Blurb);
            Console.WriteLine(page5.Title);
            Console.WriteLine(page5.Article);




            //WebPage page6 = new WebPage(new PageStructure("http://www.risk.net/risk-magazine/feature/2350848/bloomberg-sef-success-leads-to-fee-criticism", 0, new List<string> { "class", "article-description", "class", "kindle_title", "tag", "p" }, new Uri("http://risk.net/")));
            //Console.WriteLine(page6.Blurb);
            //Console.WriteLine(page6.Title);
            //Console.WriteLine(page6.Article);


            //WebPage page6 = new WebPage("http://www.institutionalinvestorsalpha.com/FAQ.html", 0,
            //    new List<string>()
            //    {
            //        "class",
            //        "article",
            //        "id",
            //        "ctl00_ContentPlaceHolder1_lblTitle",
            //        "tag",
            //        "p" /*(second p)*/
            //    }, new Uri("http://www.institutionalinvestorsalpha.com/"));
            //Console.WriteLine(page6.Blurb);
            //Console.WriteLine(page6.Title);
            //Console.WriteLine(page6.Article);
            //foreach (var i in page6.Urls)
            //{
            //    Console.WriteLine(i);
            //}


            //WebPage page6 = new WebPage(new PageStructure("http://www.waterstechnology.com/waters/feature/2352343/in-spain-technology-charts-bad-bank-path", 0, new List<string> { "class", "article_descriptions new_test", "tag", "h1", "tag", "p" }, new Uri("http://www.waterstechnology.com/buy-side-technology/")));
            //Console.WriteLine(page6.Blurb);
            //Console.WriteLine(page6.Title);
            //Console.WriteLine(page6.Article);


            var articleScore = new Dictionary<WebPage, int>();
            var pull = new DataPull();
            var crawler = new Crawler();
            crawler.CrawlSite();
            Console.WriteLine("Done Crawling");
            var articles = crawler.PageRank.Keys.ToList();
            var scorer = new Scorer(articles, pull);
            var scores = scorer.ScorePages();

            foreach (var article in scores.Keys.AsEnumerable())
            {
                Console.WriteLine(article.Title + " " + article.Blurb + "\n " + scores[article]);
            }
            
            //var highScore = new Dictionary<string, int>();
            //foreach (var article in articles)
            //{
            //    var scorer = new Scorer(article, pull);
            //    var score = scorer.ScorePage();
            //    articleScore.Add(article, score);
            //    Console.WriteLine(article.Title + " " + article.Blurb + "\n " + score);
            //    if (score > 1)
            //    {
            //        if (!highScore.Keys.ToArray().Contains(article.Title + " " + article.Blurb))
            //        {
            //            highScore.Add(article.Title + " " + article.Blurb, score);
            //        }
            //    }
            //}
            Console.WriteLine("\n\n\n\n DONE SCORING!!!!\n\n\n\n\n\n");

            //foreach (var a in highScore.Keys)
            //{
            //    Console.WriteLine(a + "\n" + highScore[a]);
            //}

            //var pull = new AthenaPull();
            //var list = pull.Emms.Keys;
            //foreach (var word in list)
            //{
            //    Console.WriteLine(word);
            //}

            //var articleViewsModels = new ArticleViewModel();
            //var f = articleViewsModels.Articles;
            //foreach (var q in f)
            //{
            //    Console.WriteLine(q.Url);
            //    Console.WriteLine(q.Title);
            //    Console.WriteLine(q.Blurb);
            //}

            //var today = DateTime.Now;
            //var yesterday = new DateTime(2014, 7, 8);
            //var substraction = new TimeSpan(5);
            //var x = today.AddDays(-5);
            //Console.WriteLine(x);
            //Console.WriteLine(yesterday > x);

            //var _keywords = new Dictionary<string, int>
            //{
            //    {"long-biased credit",2},
            //    {"structured credit",2},
            //    {"long/short credit",2},
            //    {"equity hedge",2},
            //    {"energy materials",2},
            //    {"equity market neutral",2},
            //    {"fundamental growth",2},
            //    {"fundamental value",2},
            //    {"multi-strategy",2},
            //    {"quantitative directional",2},
            //    {"short bias",2},
            //    {"activist",2},
            //    {"credit arbitrage",2},
            //    {"restructuring",2},
            //    {"distressed",2},
            //    {"merger arbitrage",2},
            //    {"Regulation D",2},
            //    {"special situation",2},
            //    {"active trading",2},
            //    {"commodity",2},
            //    {"systematic diversified",2},
            //    {"fixed income",2},
            //    {"volatility",2},
            //    {"yield alternatives",2},
            //    {"sovereign",2},
            //    {"corporate",2},
            //    {"convertible arbitrage", 2},
            //    {"asset backed",2},
            //    {"discretionary thematic",2},
            //    {"regulation",2},
            //    {"institutional",2},
            //    {"fund of hedge funds", 10},
            //    {"fund of funds", 10},
            //    {"alternative investment", 5},
            //    {"insider trading", 25},
            //    {"insider-trading", 25},
            //    {"Volcker rule", 5},
            //};

            //var _competitors = new Dictionary<string, int>
            //{
            //    {"Blackstone Alternative Asset Management", 5}, //Blackstone fohf
            //    {"EnTrust Capital", 5},
            //    {"Pacific Alternative Asset Management", 5},
            //    {"Permal",5},
            //    {"Prisma",5}, //KKR fohf
            //    {"Mesirow Advanced Strategies",5}, // Mesirow fohf
            //    {"AIMS Hedge Fund Strategies",5}, //Goldman fohf
            //    {"Alternative Investment & Manager Selection", 5}, // Goldman fohf
            //    {"Aurora", 5},
            //    {"BlackRock Alternative Advisors",5}, //BlackRock fohf
            //    {"GAM", 5},
            //    {"Aetos",5},
            //    {"Cube Capital", 5},
            //    {"Rock Creek",5},
            //    {"K2",5},
            //    {"Crestline",5},
            //    {"JP Morgan Alternative Asset Management",5}, //JP fohf
            //    {"J.P. Morgan Alternative Asset Management",5}, //JP fohf
            //    {"PFS Horizon",5}, //Credit Suisse fohf
            //    {"HSBC Private Bank",5}, //HSBS fohf
            //    {"Lyxor",5},
            //    {"A&Q Hedge Fund Solutions",5} //UBS fohf
            //};

            //var connectionString = ConfigurationManager.ConnectionStrings["NewsApplicationDb"];
            //var db = Database.OpenConnection(connectionString.ConnectionString);

            //foreach (var competitor in _competitors.Keys)
            //{
            //    db.Keywords.Insert(Keyword: competitor, Score: _competitors[competitor], Section: "Competitors");
            //}

            //foreach (var keyword in _keywords.Keys)
            //{
            //    db.Keywords.Insert(Keyword: keyword, Score: _keywords[keyword], Section: "Industry");
            //}

            Console.ReadKey();
        }
    }
}
