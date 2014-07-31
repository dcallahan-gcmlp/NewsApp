using System;
using System.Web.Mvc;
using NewsApplication.Models;

namespace NewsApplication.Controllers
{
    public class CommentController : Controller
    {

        //
        // POST: /Comment/Create

        [HttpPost]
        public JsonResult Create(Comment comment)
        {
            var commentId = 0;
            try
            {
                if(comment.Body != null)
                {
                    commentId = comment.Save();
                }
            }
            catch (Exception)
            {
                return Json(new {error = true, errormsg = "messed up in comment.Save()"});
            }
            
            //return Json(new { error = true, errormsg = "Please add a comment."});
            return Json(new { error = false, errormsg = string.Empty, id = commentId });
        }

        //
        // POST: /Comment/Delete

        [HttpPost]
        public JsonResult Delete(Comment comment)
        {
            try
            {
                comment.Delete();
            }
            catch (Exception)
            {
                return Json(new {error = true, errormsg = "messed up in comment.Delete()"});
            }
            return Json(new { error = false, errormsg = string.Empty });
        }
    }
}
