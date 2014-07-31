using System;
using System.Web.Mvc;
using NewsApplication.Models;

namespace NewsApplication.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(DateTime? date=null)
        {
            var viewModel = new ViewModel();
            var articleViewModel = new ArticleViewModel();
            viewModel.Sections = articleViewModel.DistinctSections();
            if (date == null)
            {
                viewModel.Articles = articleViewModel.Articles();
            }
            else
            {
                viewModel.Articles = articleViewModel.Articles((DateTime)date);
            }
            return View(viewModel);
        }
    }
}
