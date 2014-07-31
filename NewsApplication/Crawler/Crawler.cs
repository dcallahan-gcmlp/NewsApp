using System;
using System.Collections.Generic;
using NewsApplication.Models;

namespace NewsApplication.Crawler
{
    public class Crawler
    {
        private readonly Dictionary<string, string> _crawledPages = new Dictionary<string, string>();
        private readonly Queue<PageStructure> _queue = new Queue<PageStructure>();
        private readonly Dictionary<WebPage, int> _pageRank = new Dictionary<WebPage, int>();
        private const int MaxDepth = 1;
        private readonly Dictionary<string, string> _childrenInQueue = new Dictionary<string, string>();

        public Dictionary<WebPage, int> PageRank { get { return _pageRank; } }

        //private static readonly List<string> InitialPages = new List<string>
        //{
        //    "http://www.hedgefundintelligence.com/",
        //    "http://www.institutionalinvestoralpha.com/",
        //    "http://www.pionline.com/",  BLOCKED FROM CRAWLING
        //    "http://www.finalternatives.com/",
        //    "http://www.evestment.com/",
        //    "http://www.seekingalpha.com/",
        //    "http://www.risk.net/",                          // !!!!!Need credentials for still
        //    "http://www.waterstechnology.com/buy-side-technology/",  //  !!!!!Need credentials for still  BLOCKED FROM CRAWLING
        //    "http://www.hfmweek.com/",           //  !!!!!Need credentials for still
        //    "http://www.iisearches.com/",          //    !!!!!Need credentials for still  BLOCKED FROM CRAWLING
        //    "http://www.institutionalinvestor.com/"
        //};

      //  readonly Dictionary<string, List<string>> _websiteAndHtmlForm = new Dictionary<string, List<string>>
      //  {
      //      {"http://www.hedgefundintelligence.com/", new List<string> {"class", "col_default article", "tag", "h1", "tag", "p"}},
      //      {"http://www.institutionalinvestoralpha.com/", new List<string> { "class", "article", "class", "article_header", "tag", "p" /*(second p)*/ }}, //GOOD
      // //     {"http://www.pionline.com/", new List<string> { "tag", "article", "itemprop", "headline", "tag", "p" }}, //Stil some errors
      //   //   {"http://www.finalternatives.com/", new List<string> { "class", "content story ", "class", "title", "tag", "p" }}, //GOOD
      ////      {"http://www.seekingalpha.com/", new List<string> { "id","article_body", "tag", "h1", "tag", "p"}}, //GOOD
      //  //    {"http://www.institutionalinvestor.com/", new List<string> { "class", "article-body", "class", "title", "tag", "p" }}, //GOOD
      //  //    {"http://risk.net/", new List<string> { "class", "kindle_title", "class", "article-block-section", "tag", "p" }}
      //      {"http://www.iisearches.com/", new List<string> {"id","ct100_cphMain_daContents", "class", "article-title", "tag", "p"} }
      //  }; 
        

        private readonly List<PageStructure> _pages = new List<PageStructure>
        {
            /*Good*/   new PageStructure("http://www.institutionalinvestoralpha.com/", 0, new List<string> { "class", "article", "class", "article_header", "tag", "p"}, new Uri("http://www.institutionalinvestoralpha.com/")),
            //Issue 7/30  new PageStructure("http://www.hedgefundintelligence.com/", 0, new List<string> { "class", "article normal", "tag", "h1", "tag", "p" }, new Uri("http://www.hedgefundintelligence.com/")),
            /*Good*/   new PageStructure("http://www.finalternatives.com/", 0, new List<string> { "class", "content story ", "class", "title", "tag", "p" }, new Uri("http://www.finalternatives.com/")),
            /*Good*/   new PageStructure("http://www.seekingalpha.com/", 0, new List<string> { "id","article_body", "tag", "h1", "tag", "p"}, new Uri("http://www.seekingalpha.com/")),
            /*Good*/   new PageStructure("http://www.institutionalinvestor.com/", 0, new List<string> { "class", "article-body", "class", "title", "tag", "p" }, new Uri("http://www.institutionalinvestor.com/")),
            //Blocks   new PageStructure("http://pionline.com/", 0, new List<string> { "id", "article-body", "tag", "h1", "tag", "p" }, new Uri("http://pionline.com/")),
            //Blocks   new PageStructure("http://risk.net/", 0, new List<string> { "class", "kindle_title", "class", "article-block-section", "tag", "p" }, new Uri("http://risk.net/")),
            //Blocks   new PageStructure("http://iisearches.com/", 0, new List<string> {"id","ct100_cphMain_daContents", "class", "article-title", "tag", "p"}, new Uri("http://www.iisearches.com/")),
            //Blocks   new PageStructure("http://www.evestment.com/news-events/industry-news/", 0, new List<string> {"class", "sfnewsContent", "class", "sfnewsTitle", "tag", "p"}, new Uri("http://www.evestment.com/news-events/industry-news/")),
            /*Good*/   new PageStructure("http://waterstechnology.com/buy-side-technology/", 0, new List<string> {"class", "article_descriptions new_test", "tag", "h1", "tag", "p"}, new Uri("http://www.waterstechnology.com/buy-side-technology/")),
            //Blocks   new PageStructure("https://www.hfmweek.com/", 0, new List<string> {"id", "col1", "tag", "h1", "tag", "p"}, new Uri("https://hfmweek.com/") ),
            /*Good*/   new PageStructure("http://www.bloomberg.com/", 0, new List<string> {"class", "article_body", "tag", "h1", "tag", "p" }, new Uri("http://www.bloomberg.com/"))
        };
 

        public void CrawlSite()
        {
            foreach (var page in _pages)
            {
                _queue.Enqueue(page);
            }
            while (_queue.Count != 0)
            {
                var currentWebPage = new WebPage(_queue.Dequeue());
                //Console.WriteLine("The page " + currentWebPage.PageStructure.Url + " has been expanded. " + currentWebPage.PageStructure.Depth + "\n");
                if (currentWebPage.Article)
                {
                    Console.WriteLine(currentWebPage.Title + "\n" + currentWebPage.Blurb);
                    if(currentWebPage.Blurb.Contains("Kushal Kumar"))
                    {
                        _pageRank.Add(currentWebPage,1);
                    }
                    else
                    {
                        _pageRank.Add(currentWebPage, 1);
                    }
                    
                }
                if (HasBeenCrawled(currentWebPage.PageStructure.Url) || TooDeep(currentWebPage)) continue;
                CrawlPage(currentWebPage);
            }
        }

        private void CrawlPage(WebPage webPage)
        {
            _crawledPages.Add(webPage.PageStructure.Url, "");
            // iterate over all the URL strings of the children, turn them into a webpage, and
            // put them on the queue if they havem't been crawled yet
            if (! TooDeep(webPage))
            {
                // all the "children" of the webpage...all links the webpage contains
                var children = webPage.SetUrls();

                foreach (var child in children)
                {
                    if ((!HasBeenCrawled(child)) && IsDescendent(child, webPage) && (!_childrenInQueue.ContainsKey(child)))
                    {
                        _childrenInQueue.Add(child, "");
                        var childPage = new PageStructure(child, webPage.PageStructure.Depth + 1, webPage.PageStructure.ArticleStructure, webPage.PageStructure.BaseUri);
                        _queue.Enqueue(childPage);
                    }
                }
            }
        }

        // tells whether or not the page has already been crawled
        private Boolean HasBeenCrawled(string url)
        {
            return _crawledPages.ContainsKey(url);
        }

        // tells whether or not the link is too deep in the search tree
        private Boolean TooDeep(WebPage webPage)
        {
            var depth = webPage.PageStructure.Depth;
            return depth >= MaxDepth;
        }

        // tells whether or not the link leads to a webpage that is not a descendent of the main page
        private Boolean IsDescendent(string url, WebPage webPage)
        {
            if (!url.Contains("http://") && !url.Contains("https://")) return true;
            var uri = webPage.PageStructure.BaseUri.ToString();
            return url.Contains(uri);
        }
    }
}