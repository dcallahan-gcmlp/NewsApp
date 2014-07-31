using System.Web.Mvc;
using NewsApplication.Models;

namespace NewsApplication.Controllers
{
    public class NewController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /New/Login

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user.Login())
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
    }
}
