using MainWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainWork.Controllers
{
    public class PlayerController : Controller
    {
        // GET: TwoPlayer

        public ActionResult TwoPlayer(bool ajax = false)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
            }
            return View();
        }

        //商品內容頁的試聽曲專用小播放器
        public ActionResult DemoPlayer(int musicId)
        {
            CMusic cm = new CMusic();
            return PartialView(cm.getMusic(musicId));
        }
    }
}