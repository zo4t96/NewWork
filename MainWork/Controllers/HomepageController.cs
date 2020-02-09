using MainWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainWork.Controllers
{
    public class HomepageController : Controller
    {
        // GET: Homepage

        //第一次進入網站會讀這個(會讀取母框架)
        public ActionResult Main(bool ajax = false)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
            }
            CSearch cs = new CSearch();
            return View(cs.allAlbum());
        }
    }
}