using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace NewsApplication.Crawler
{
    public class WebPage
    {
        private List<string> _urls = new List<string>(); // list of urls the webpage links to
        private HtmlDocument document = new HtmlDocument();

        public WebPage(PageStructure pageStructure)
        {
            PageStructure = pageStructure;
            SetText(PageStructure); //sets the _text of the webpage given the page url as raw HTML
            SetArticle(Text); // sets the Boolean representing whether or not the page is an article page
            document.LoadHtml(Text);
            if (Article)
            {
                SetBlurb(); // sets the _blurb of the webpage given the text of the page
                SetTitle(); // sets the _title of the webpage given the text of the page
            }
            else
            {
                Title = "";
                Blurb = "";
            }
        }

        public string Text { get; private set; }
        public string Title { get; private set; }
        public bool Article { get; private set; }
        public string Blurb { get; private set; }

        public List<string> Urls
        {
            get
            {
                var defensiveCopy = _urls;
                return defensiveCopy;
            }
        }
        public PageStructure PageStructure { get; private set; }


        // Sets the _text field to the HTML content of the URL link
        private void SetText(PageStructure page)
        {
            try
            {
                // Create web connection to specific URL
                var req = (HttpWebRequest)WebRequest.Create(page.Url);

                
                // Get the response from the website, stream its content, and read it
                // Need to create some sort of handling for ill-formatted response/error response from the website
                var res = req.GetResponse();
                var resStream = res.GetResponseStream();
                if (resStream == null) return;
                var readStream = new StreamReader(resStream);

                // Turn stream into a usable string
                Text = WebUtility.HtmlDecode(readStream.ReadToEnd());
            }
            catch (WebException)
            {
                //var except = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                Text = string.Empty;
            }
        }

        // Sets the boolean representing whether or not the page is an article
        private void SetArticle(string webPageText)
        {
            if (!PageStructure.ArticleForm().Equals("tag"))
            {
                Article = (webPageText.Contains(PageStructure.ArticleForm() + "='" + PageStructure.ArticleName() + "'") || webPageText.Contains(PageStructure.ArticleForm() + "=\"" + PageStructure.ArticleName() + "\""));
            }
            else
            {
                Article = webPageText.Contains(PageStructure.ArticleName());
            }
        }

        // Gets the HTML content between the article tags
        public string ArticleContent()
        {
            if (!(PageStructure.ArticleForm().Equals("tag")))
            {
                return document.DocumentNode.SelectSingleNode("//*[@" + PageStructure.ArticleForm() + "='" + PageStructure.ArticleName() + "']").InnerHtml;
            }
            return document.DocumentNode.SelectSingleNode("//" + PageStructure.ArticleName()).InnerHtml;
        }

        // Sets _blurb to the first 50 characters in the first paragraph
        // If the page doesn't have an article sets _blurb to an empty string
        private void SetBlurb()
        {
            var articleText = ArticleContent();
            var articleDocument = new HtmlDocument();
            articleDocument.LoadHtml(articleText);
            var blurbString = "";
            if (! (PageStructure.ParagraphForm().Equals("tag")))
            {
                blurbString = articleDocument.DocumentNode.SelectSingleNode("//*[@" + PageStructure.ParagraphForm() + "='" + PageStructure.ParagraphName() + "']").InnerText;
            }
            else
            {
                var nodes = articleDocument.DocumentNode.SelectNodes("//" + PageStructure.ParagraphName());
                if (nodes != null)
                {
                    var content = "";
                    var count = 0;
                    while (content.Length < 50)
                    {
                        try
                        {
                            content = nodes[count].InnerText;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            content = nodes[0].InnerText;
                        }
                        count++;
                    }
                    blurbString = content;
                }
            }
            if (blurbString.Length > 149)
            {
                Blurb = blurbString.Substring(0, 149) + "...";
            }
            else
            {
                Blurb = blurbString + "...";
            }
        }

        // Sets _title of the article
        // If page doesn't have an article, sets _title to an empty string
        private void SetTitle()
        {
            if (!(PageStructure.TitleForm().Equals("tag")))
            {
                try
                {
                    Title = document.DocumentNode.SelectSingleNode("//*[@" + PageStructure.TitleForm() + "='" + PageStructure.TitleName() + "']").InnerText;
                }
                catch (NullReferenceException)
                {
                    Title = "";
                }   
            }
            else
            {
                try
                {
                    Title = document.DocumentNode.SelectSingleNode("//" + PageStructure.TitleName()).InnerText;
                }
                catch (NullReferenceException)
                {
                    Title = "";
                }
            }
        }

        // Sets the _urls of the webpage
        public List<string> SetUrls()
        {
            // probably need to check to see in the page contains an href first
            var htmlChecker = document.DocumentNode.SelectSingleNode("//a[@href]");
            if (htmlChecker != null)
            {
                var nodes = document.DocumentNode.SelectNodes("//a[@href]");
                foreach (var link in nodes)
                {
                    var relativePath = link.GetAttributeValue("href", null);
                    if (!BadUrl(relativePath)/*(!(relativePath.Contains("#"))) && (!(relativePath.Contains("javascript"))) && (!(relativePath.Contains("mailto")))*/)
                    {
                        // need to create error handling for ill-formatted URIs
                        _urls.Add(relativePath);
                    }
                }
            }
            var children = _urls;
            return children;

        }

        private static Boolean BadUrl(string url)
        {
            var badUrlContents = new List<string>
            {
                "#",
                "javascript",
                "mailto",
                "aboutus",
                "about-us",
                "contacts",
                "contactus",
                "rss",
                "cookies",
                "register",
                "subscribe",
                "careers",
                "jobs",
                "sitemap",
                "ebooks",
                "blog",
                "podcast",
                "newsletter",
                "blog",
                "freetrial",
                "login",
                "database-hfi", //specific to institutionalinvestorsalpha
                "archive",
                "socialmedia",
                "faq",
                "/symbol/", //seekingalpha
                "/data/",  //seekingalpha
                "customer-service",
                "reprints",
                "/video-center",  //institutionalinvestor
                "/webcasts",  //institutionalinvestor
                "/offices",  //institutionalinvestor
                "/orders" //institutionalinvestor
            };
            foreach (var word in badUrlContents)
            {
                if (url.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}