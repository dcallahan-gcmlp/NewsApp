using System.Web.Mvc;
using NewsApplication.Models;

namespace NewsApplication.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /Register/

        public ActionResult Index()
        {
            return View();
        }


        //
        // POST: /New/RegisterAccount

        [HttpPost]
        public ActionResult RegisterAccount(User user)
        {
            if (user.CreateUser())
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
    }
}
