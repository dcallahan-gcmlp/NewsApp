using System;
using System.Collections.Generic;

namespace NewsApplication.Crawler
{
    public class PageStructure
    {
        private readonly List<string> _articleStructure = new List<string>(6);
        private string _url;

        public PageStructure(string url, int depth, List<string> articleStructure, Uri baseUri)
        {
            BaseUri = baseUri;
            Url = url;
            Depth = depth;
            _articleStructure = articleStructure;
        }

        public string Url
        {
            get { return _url; }
            private set
            {
                var fixedPath = new Uri(BaseUri, value);
                _url = fixedPath.ToString();
            }
        }

        public int Depth { get; private set; }
        public Uri BaseUri { get; private set; }

        public List<string> ArticleStructure
        {
            get
            {
                var defensiveCopy = _articleStructure;
                return defensiveCopy;
            }
        }

        public string ArticleForm()
        {
            return _articleStructure[0];
        }

        public string ArticleName()
        {
            return _articleStructure[1];
        }

        public string TitleForm()
        {
            return _articleStructure[2];
        }

        public string TitleName()
        {
            return _articleStructure[3];
        }

        public string ParagraphForm()
        {
            return _articleStructure[4];
        }

        public string ParagraphName()
        {
            return _articleStructure[5];
        }
    }
}