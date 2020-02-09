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
            return View(search.takeAllKind());
        }
        
        //進行搜尋時先跳轉到空頁面，並將搜尋資料傳遞，之後再讓該頁面實作搜尋的指令
        public ActionResult KindResult(bool ajax = false)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
            }
            return View();
        }
        //沒有帶ajax參數代表僅提供讀取資料的頁面而無自己的網址
        public ActionResult KindResultView(int kindID)
        {
            return PartialView(search.byKindPage(kindID));
        }
        
        //基礎搜尋頁面
        public ActionResult Result(bool ajax = false)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
            }
            return View();
        }
        public ActionResult ResultView(string keyword)
        {
            return PartialView(search.byKeyword(keyword));
        }

        //進階搜尋，實作方法與普通差不多
        public ActionResult AdvancedResult(bool ajax = false)
        {
            if (ajax)
            {
                ViewBag.ajax = true;
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

        //進階搜尋內容的初始化
        public ActionResult SetTypes()
        {
            CSearch cs = new CSearch();
            var data = cs.takeAllType();
            //用json直接回傳entity物件時，會產生循環參考的錯誤
            //這邊選擇直接將資料轉為匿名物件回傳
            var result = data.Select(d => new { d.fTypeID, d.fTypeName });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetKinds()
        {
            CSearch cs = new CSearch();
            var data = cs.takeAllKind();
            var result = data.Select(d => new { d.KindName });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}