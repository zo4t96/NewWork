using MusicPrj.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Web.Script.Services;
using System.Web.Script.Serialization;


namespace MusicPrj.Controllers
{
    public class AlbumController : Controller
    {
        private dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        // GET: Album

        public ActionResult Index(bool ajax =false)
        {
            var album = from a in db.tAlbums
                        select a;
          Session[CDictionary.SK_sideboxList1Id] = "test";
            return View(album);
        }

        //[HttpPost]
        //public ActionResult Index(bool ajax)
        //{
        //    var album = from a in db.tAlbums
        //                select a;
        //    CWebInitailize ad = new CWebInitailize();
        //    ViewBag.InitialModel = ad.advancedInitial();
        //    //List <tPlayList> tPL = db.tPlayLists.Where(p => p.fAccount == Session[CDictionary.SK_ACCOUNT].ToString()).ToList();
        //    //if (tPL != null)
        //    //{
        //    //    Session[CDictionary.SK_PLAYERLIST] = tPL;
        //    //}
        //    return View(album);
        //}

        CPlaylist playlist = new CPlaylist();
        CAlbum album = new CAlbum();
        public ActionResult PlayLists()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            List<tProduct> tp = playlist.getUserPlaylist(s1);
            if (tp.Count != 0)
            {
                return View(tp);
            }
            else
            {
                //    return View();
                return Content("<span>撥放清單是空的喔<span>");
            }

        }

        public ActionResult _PlayLists()
        {
            if(Session[CDictionary.SK_ACCOUNT] == null || string.IsNullOrWhiteSpace(Session[CDictionary.SK_ACCOUNT].ToString()))
            {
                return Content("<span>你還沒登入喔<span>");
            }
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            List<tProduct> tp = playlist.getUserPlaylist(s1);
            if (tp.Count != 0)
            {
            return PartialView("_PlayLists",tp);
            }
            else
            {
                //    return View();
                return Content("<span>撥放清單是空的喔<span>");
            }
        }

        public ActionResult addPlayLists(int amid)
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(playlist.userAddPlayLists(s1,amid));
        }

        //未完成,先不包成模組
        public ActionResult nextPlayLists(int amid)
        {

            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            string s2 = "";
            string s3 = "";
            tMember tM = db.tMembers.FirstOrDefault(p => p.fAccount == s1);

            
            if(DateTime.Now > tM.fSubscriptEndDate)
            {
                s3 = "已過期";
            }
            else if (DateTime.Now < tM.fSubscriptEndDate)
            {
                s3 = "未過期";
            }else
            {
                s3 = "一般會員";
            }
            int m_temp_nextPlayID = 0;
            List<tPlayList> tnowPL = db.tPlayLists.Where(p => p.fAccount == s1).ToList();
            tnowPL.Reverse();
            //if (tnowPL.FirstOrDefault(p => p.fProductID == amid) == null)
            //{
            //    s2 = "撥放清單找不到該首";
            //    m_temp_nextPlayID = tnowPL.First().fProductID;
            //    tProduct tprod = db.tProducts.FirstOrDefault(p => p.fProductID == m_temp_nextPlayID);
            //    return Json(tprod, JsonRequestBehavior.AllowGet);
            //}
            tPlayList tPlayl = tnowPL.FirstOrDefault(p => p.fProductID == amid && p.fAccount == s1);
            if (tPlayl == null)
            {
                s2 = "查無此";
                return Json(s2, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.tPlayLists.Remove(tPlayl);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    s2 = ex.ToString();
                    return Json(s2, JsonRequestBehavior.AllowGet);
                }
            }
            //刪除後重新撈一次數據,避免撈相同的
            tnowPL = db.tPlayLists.Where(p => p.fAccount == s1).ToList();
            tnowPL.Reverse();
            m_temp_nextPlayID = tnowPL.First().fProductID;
            tProduct tprodSec = db.tProducts.FirstOrDefault(p => p.fProductID == m_temp_nextPlayID);
            if (tprodSec != null)
            {
                int m_temp_nextalbum = (int)tprodSec.fAlbumID;
                string talbSec = (db.tAlbums.FirstOrDefault(p => p.fAlbumID == m_temp_nextalbum)).fCoverPath;
                s2 = "成功";
                //    tProduct tprodSec = db.tProducts.FirstOrDefault(p => p.fProductID == 3);
                var tprodSecResult = new
                {
                    fProductID = tprodSec.fProductID,
                    fProductName = tprodSec.fProductName,
                    fPlayStart = tprodSec.fPlayStart,
                    fPlayEnd = tprodSec.fPlayEnd,
                    fFilePath = tprodSec.fFilePath,
                    fComposer = tprodSec.fComposer,
                    fCoversource = (db.tAlbums.FirstOrDefault(p => p.fAlbumID == m_temp_nextalbum)).fCoverPath,
                    fSbuscribedate = s3
                };
                return Json(tprodSecResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                s2 = "查無此2";
                return Json(s2, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Find(string term = "")
        {
            var getresult = db.tProducts.Where(p=>p.fProductName.StartsWith(term)).Select(p=>p.fProductName);
            return Json(getresult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult test()
        {
            int amid = 5;
            //test 測試框內框外對連結影響0129
            ViewBag.AlbInfo = db.tAlbums.FirstOrDefault(p => p.fAlbumID == amid);
            var list = db.tProducts.Where(p => p.fAlbumID == amid);
            //         Session["account"] = "aaa";
            if (list != null)
            {
                Session["albumid"] = amid;
                return View(list);
            }
            else
            {
                return View();
            }
        }

        public ActionResult MyAlbumList(string account)
        {
            var list = db.tAlbums.Where(p => p.fAccount == account);
            ViewBag.account = account;
            //Session[CDictionary.SK_sideboxList1Name] = "增加專輯";
            //Session[CDictionary.SK_sideboxList1Method] = "AddAlbum";
            //Session[CDictionary.SK_sideboxList1Id] = Session[CDictionary.SK_ACCOUNT].ToString();
            return View(list);
        }

        [HttpPost]
        public ActionResult MyAlbumList(string account, bool ajax)
        {
            var list = db.tAlbums.Where(p => p.fAccount == account);
            ViewBag.account = account;
            return View(list);
        }

        public ActionResult AlbumInfo(int amid, bool ajax = false)
        {
            ViewBag.AlbInfo = db.tAlbums.FirstOrDefault(p => p.fAlbumID == amid);
            var list = db.tProducts.Where(p => p.fAlbumID == amid);
            if (list != null)
            {
                Session["albumid"] = amid;
                return View(list);
            }
            else
            {
                return View();
            }
        }

        public ActionResult AlbumDetail(int amid, bool ajax = false)
        {
            ViewBag.AlbInfo = db.tAlbums.FirstOrDefault(p => p.fAlbumID == amid);
            var list = db.tProducts.Where(p => p.fAlbumID == amid);
            //         Session["account"] = "aaa";
            if (list != null)
            {
                Session["albumid"] = amid;
                return View(list);
            }
            else
            {
                return View();
            }
        }

        //[HttpPost]
        //public ActionResult AlbumDetail(int amid, bool ajax)
        //{
        //    CWebInitailize ad = new CWebInitailize();
        //    ViewBag.InitialModel = ad.advancedInitial();
        //    ViewBag.AlbInfo = db.tAlbums.FirstOrDefault(p => p.fAlbumID == amid);
        //    var list = db.tProducts.Where(p => p.fAlbumID == amid);
        //    //         Session["account"] = "aaa";
        //    if (list != null)
        //    {
        //        Session["albumid"] = amid;
        //        return View(list);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}


        public ActionResult AddAlbum(string account)
        {
            tAlbum tA = new tAlbum();
            tA.fAccount = account;
            return View(tA);
        }

        //上傳專輯
        [HttpPost]
        public ActionResult AddAlbum(tAlbum tA)
        {
            string user = Session[CDictionary.SK_ACCOUNT].ToString();
            ViewBag.error = album.userAddAlbum(tA, user);
            string s1 = "MyAlbumList?account=" + Session[CDictionary.SK_ACCOUNT].ToString();
            return Redirect(s1);
        }

        //上傳單曲
        [HttpPost]
        public ActionResult AlbumInfo(tProduct tP)
        {
            if(tP == null)
            {
                return View();
            }
            int amid =Int32.Parse(Session["albumid"].ToString());
            ViewBag.error = album.userAddsong(tP, amid);
            string s1 = "AlbumInfo?amid=" + Session["albumid"].ToString();
            return Redirect(s1);
        }

        //編輯單曲
        public ActionResult _EditSong(int amid)
        {
            tProduct tprodEdit = db.tProducts.FirstOrDefault(p => p.fProductID == amid);
            return PartialView("_EditSong",tprodEdit);
        }

        //刪除單曲
        public ActionResult DelteSong(int amid, bool ajax)
        {
            string s1 = "AlbumInfo?amid=" + Session["albumid"].ToString();
            tProduct tP = db.tProducts.FirstOrDefault(p=>p.fProductID == amid);
            if (tP == null)
            {
                return Redirect(s1);
            }
            ViewBag.Msg = album.userDelteSong(tP);
            //return View(); //除錯用
            return Redirect(s1);
        }

        //刪除專輯
        public ActionResult DeleteAlbum(int amid, bool ajax)
        {
            string s1 = "MyAlbumList?amid=" + Session[CDictionary.SK_ACCOUNT].ToString();
            tAlbum tA = db.tAlbums.FirstOrDefault(p => p.fAlbumID == amid);
            if (tA == null)
            {
                return Redirect(s1);
            }
            List<tProduct> tP = db.tProducts.Where(p => p.fAlbumID == amid).ToList();
            if (tP == null)
            {
                return Redirect(s1);
            }
            ViewBag.Msg = album.userDeleteAlbum(tA,tP);
            //return View(); //除錯用
            return Redirect(s1);
        }

        //更新單曲
        public ActionResult updateSongTryLimit(int amid, float start, float end)
        {
            string s1 = "AlbumInfo?amid=" + Session["albumid"].ToString();
                return Json(album.userUpdateSongTryLimit(amid, start, end));
        }

    }
}