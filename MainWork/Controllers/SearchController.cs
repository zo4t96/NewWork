﻿using MainWork.Models;
using MainWork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainWork.Controllers
{
    public class SearchController : Controller
    {
        // GET: BasicSearch
        public ActionResult Kinds()
        {
            CWebInitailize ad = new CWebInitailize();
            ViewBag.InitialModel = ad.advancedInitial();
            CSearch tk = new CSearch();
            return View(tk.takeAllKind());
        }

        [HttpPost]
        public ActionResult Kinds(bool ajax)
        {
            CSearch tk = new CSearch();
            if (ajax)
            {
                ViewBag.ajax = true;
            }
            return View(tk.takeAllKind());
        }

        public ActionResult Result()
        {
            //按下重新整理或直接貼網址會跑回首頁
            return RedirectToAction("Main","Homepage");
        }
        [HttpPost]
        public ActionResult Result(string keyword)
        {
            CSearch search = new CSearch();
            return View(search.byKeyword(keyword));
        }

        public ActionResult AdvancedResult()
        {
            return RedirectToAction("Main", "Homepage");
        }
        [HttpPost]
        public ActionResult AdvancedResult(CAdvancedSearchObject keyObj)
        {
            CSearch search = new CSearch();
            return View(search.byAdvanced(keyObj));
        }
    }
}