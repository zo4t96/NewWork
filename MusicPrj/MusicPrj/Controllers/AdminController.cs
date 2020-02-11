using MusicPrj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPrj.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
                return View();
        }
        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult Index3()
        {
            return View();
        }
        public ActionResult Index4()
        {
            return View();
        }
        public ActionResult chart(bool ajax = false)
        {
            return View();
        }
        public ActionResult table(bool ajax = false)
        {
            return View();
        }
        public ActionResult form(bool ajax = false)
        {
            return View();
        }
        public ActionResult calendar(bool ajax = false)
        {
            return View();
        }
        public ActionResult map(bool ajax = false)
        {
            return View();
        }

        public ActionResult login(bool ajax = false)
        {
            return View();
        }

        public ActionResult register(bool ajax = false)
        {
            return View();
        }

    }
}