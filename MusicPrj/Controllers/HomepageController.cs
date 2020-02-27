using MusicPrj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPrj.Controllers
{
    public class HomepageController : Controller
    {
        //第一次進入網站會讀這個(會讀取母框架)
        public ActionResult Main(bool ajax = false)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
            }
            CSearch cs = new CSearch();
            return View(cs.allAlbum().ToList());
        }

        public ActionResult _PlayLists()
        {
            return Content("<span>你還沒登入喔<span>");
        }
    }
}