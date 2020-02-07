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
        //搜尋物件，內含各種搜尋功能，直接宣告在最外面
        CSearch search = new CSearch();

        // GET: BasicSearch
        //曲風選擇頁面
        public ActionResult Kinds(bool ajax = false)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
            }
            else
            {
                CWebInitailize ad = new CWebInitailize();
                ViewBag.InitialModel = (new CWebInitailize()).advancedInitial();
            }
            return View(search.takeAllKind());
        }

        public ActionResult KindResult(CKindObject kindObj)
        {
            return View(search.byKindPage(kindObj.kindID));
        }

        //進行搜尋時先跳轉到空頁面，並將搜尋資料傳遞，之後再讓該頁面實作搜尋的指令
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
            return View();
        }
        //基礎搜尋頁面，只有帶參數版本
        public ActionResult ResultView(CSearchObject keyObj)
        {
            return PartialView(search.byKeyword(keyObj.keyword));
        }

        //進階搜尋，實作方法與普通差不多
        public ActionResult AdvancedResult(CAdvancedSearchObject keyObj)
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
            return View();
        }
        public ActionResult AdvancedResultView(CAdvancedSearchObject keyObj)
        {
            return PartialView(search.byAdvanced(keyObj));
        }

        //以下皆為autocomplete的實作
        CAutoComplete cc = new CAutoComplete();
        public JsonResult AutoCompleteSong(string term)
        {
            return Json(cc.searchSong(term), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompleteSinger(string term)
        {
            return Json(cc.searchSiger(term), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompleteGroup(string term)
        {
            return Json(cc.searchGroup(term), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompleteComposer(string term)
        {
            return Json(cc.searchComposer(term), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompleteAlbum(string term)
        {
            return Json(cc.searchAlbum(term), JsonRequestBehavior.AllowGet);
        }
    }
}