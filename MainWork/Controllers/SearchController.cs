using MainWork.Models;
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
        //曲風選擇頁面
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

        //基礎搜尋頁面，只有帶參數版本
        public ActionResult Result(CSearchObject keyObj)
        {
            if (keyObj.ajax)
            {
                ViewBag.ajax = true;
            }
            else
            {
                CWebInitailize ad = new CWebInitailize();
                ViewBag.InitialModel = ad.advancedInitial();
            }
            CSearch search = new CSearch();
            return View(search.byKeyword(keyObj.keyword));
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

        //以下皆為autocomplete的實作
        public JsonResult AutoCompleteSong(string term)
        {
            CAutoComplete cc = new CAutoComplete();
            return Json(cc.searchSong(term), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompleteSinger(string term)
        {
            CAutoComplete cc = new CAutoComplete();
            return Json(cc.searchSiger(term), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompleteGroup(string term)
        {
            CAutoComplete cc = new CAutoComplete();
            return Json(cc.searchGroup(term), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompleteComposer(string term)
        {
            CAutoComplete cc = new CAutoComplete();
            return Json(cc.searchComposer(term), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompleteAlbum(string term)
        {
            CAutoComplete cc = new CAutoComplete();
            return Json(cc.searchAlbum(term), JsonRequestBehavior.AllowGet);
        }
    }
}