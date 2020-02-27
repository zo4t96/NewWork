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
using Newtonsoft.Json;

namespace MusicPrj.Controllers
{
    public class testO
    {
        public int num { get; set; }
    }

    public class AlbumController : Controller
    {
        private dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        // GET: Album

        public ActionResult Index(bool ajax = false)
        {
                    Session[CDictionary.SK_ACCOUNT] = "aaa";
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
        //    //List <tPlayList> tPL = db.tPlayLists.Where(p => p.fAccount == Session[CDictionary.SK_ACCOUNT].ToString()).ToList();
        //    //if (tPL != null)
        //    //{
        //    //    Session[CDictionary.SK_PLAYERLIST] = tPL;
        //    //}
        //    Session[CDictionary.SK_sideboxList1Id] = "test";
        //    return View(album);
        //}

        CPlaylist playlist = new CPlaylist();
        CAlbum album = new CAlbum();
        public ActionResult PlayLists()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            List<tProduct> tp = playlist.getUserPlaylistSubscribe(s1);
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

        public ActionResult Ordertest()
        {
            List<testO> ls = new List<testO> { };
            ls.Add(new testO { num = 1 });
            ls.Add(new testO { num = 2 });
            ls.Add(new testO { num = 3 });
            ls.Add(new testO { num = 4 });
            ls.Add(new testO { num = 5 });
            ls.Add(new testO { num = 6 });
            var lss = ls.Where(p=>p.num>=3).ToList();
            lss.AddRange(ls.Where(p => p.num < 3).ToList());
            string a="從:";
            foreach (var c in lss)
            {
                a += ","+c.num;
            }
            return Content(a); ;
        }

        public ActionResult _PlayLists()
        {
            if (Session[CDictionary.SK_ACCOUNT] == null || string.IsNullOrWhiteSpace(Session[CDictionary.SK_ACCOUNT].ToString()))
            {
                return Content("<span>你還沒登入喔<span>");
            }
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            tMember tM = db.tMembers.FirstOrDefault(p => p.fAccount == s1);
            List<tProduct> tp = null;
            if (DateTime.Now < tM.fSubscriptEndDate)
            {
                ViewBag.memberUseType = "包月會員";
                tp = playlist.getUserPlaylistSubscribe(s1);
            }
            else
            {
                ViewBag.memberUseType = "一般會員";
                tp = playlist.getUserPlaylistNormal(s1);
            }
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
            if (Session[CDictionary.SK_ACCOUNT] != null || !string.IsNullOrWhiteSpace(Session[CDictionary.SK_ACCOUNT].ToString()))
            {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            return Json(playlist.userAddPlayLists(s1,amid));
            }
            else
            {
                return Json(null);
            }
        }

        //未完成,先不包成模組
        public ActionResult nextPlayLists(int amid, int playmode)
        {

            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            string s2 = "";
            string s3 = "";
            tMember tM = db.tMembers.FirstOrDefault(p => p.fAccount == s1);
            tProduct tprodSec = null;
            if (DateTime.Now < tM.fSubscriptEndDate)
            {
                s3 = "包月會員";
                if (playmode != 2)
                {
                    if(playmode != 4)
                    {
                    tprodSec = playlist.getUserPlaylistSubscribeNext(s1, amid).Skip(1).Take(1).FirstOrDefault();
                    }
                    else
                    {
                    tprodSec = playlist.getUserPlaylistSubscribeNext(s1, amid).LastOrDefault();
                    }

                }
                else
                {
                    while (tprodSec == null)
                    {
                        Random rand = new Random();
                        int rInt = rand.Next(2, 10);
                        tprodSec = playlist.getUserPlaylistSubscribeNext(s1, amid).Skip(rInt).Take(1).FirstOrDefault();
                    }
                }
            }
            else
            {
                s3 = "一般會員";

                if (playmode != 2)
                {
                    if (playmode != 4)
                    {
                        tprodSec = playlist.getUserPlaylistNormalNext(s1, amid).Skip(1).Take(1).FirstOrDefault();
                    }
                    else
                    {
                        tprodSec = playlist.getUserPlaylistNormalNext(s1, amid).LastOrDefault();
                    }
                }
                else
                {
                    while (tprodSec == null)
                    {
                        Random rand = new Random();
                        int rInt = rand.Next(2, 10);
                        tprodSec = playlist.getUserPlaylistNormalNext(s1, amid).Skip(rInt).Take(1).FirstOrDefault();
                    }
                }
            }
            if (tprodSec != null)
            {
                int m_temp_nextalbum = (int)tprodSec.fAlbumID;
                string talbSec = (db.tAlbums.FirstOrDefault(p => p.fAlbumID == m_temp_nextalbum)).fCoverPath;
                s2 = "成功";
                //    tProduct tprodSec = db.tProducts.FirstOrDefault(p => p.fProductID == 3);
                try
                {
                    if (s3 == "包月會員")
                    {
                        tM.fLastPlaySong = db.tPlayLists.FirstOrDefault(p => p.fAccount == s1 && p.fProductID == tprodSec.fProductID).fPlayId;
                    }
                    else
                    {
                        tM.fLastPlaySong = db.tPurchaseItems.FirstOrDefault(p => p.fCustomer == s1 && p.fProductID == tprodSec.fProductID).fPurchaseItemID;
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    s2 = ex.ToString();
                    return Json(s2, JsonRequestBehavior.AllowGet);
                }
                var tprodSecResult = new
                {
                    fProductID = tprodSec.fProductID,
                    fProductName = tprodSec.fProductName,
                    fFilePath = tprodSec.fFilePath,
                    fComposer = tprodSec.fComposer,
                    fCoversource = (db.tAlbums.FirstOrDefault(p => p.fAlbumID == m_temp_nextalbum)).fCoverPath,
                    fAlbumName = (db.tAlbums.FirstOrDefault(p => p.fAlbumID == m_temp_nextalbum)).fAlbumName,
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

        public ActionResult MyAlbumList(string account, bool ajax = false)
        {
            //   string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            //string s1 = "bbb";
            //var q = db.tPurchaseItems.Where(p => p.fCustomer == s1)
            //    .GroupBy(p => new { year = p.fDate.Value.Year, month = p.fDate.Value.Month })
            //    .Select(p => new SelectListItem {Value = string.Format("{0}/{1}", p.Key.year, p.Key.month), Text = p.Count().ToString() }).ToList();
            //var q = from a in db.tPurchaseItems
            //                        where a.tMember.fAccount == s1
            //                        group a by new { year = a.fDate.Value.Year,month = a.fDate.Value.Month } into g
            //                        select new { dt = string.Format("{0}/{1}", g.Key.year, g.Key.month), CNT = g.Count()};
         //   q.Reverse();

            //string[] s2 = new string[5];
            //int m_Cnt = q.Count() > 5 ? 5 : q.Count()-1;
            //    for (int i = 0; i < m_Cnt; i++)
            //    {
            //        if (q[i] != null)
            //        {
            //        s2[i] = q[i].Value;
            //        }
            //        else
            //        {
            //            s2[i] = "99";
            //        }
            //    }

            //    ViewBag.monthSale1 = JsonConvert.SerializeObject(s2.Reverse());


            return View();
        }

        public ActionResult _MyAlbumList()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var list = db.tAlbums.Where(p => p.fAccount == s1);
            return View(list);
        }

        public ActionResult MyMusic()
        {
            if (Session[CDictionary.SK_ACCOUNT] == null || string.IsNullOrWhiteSpace(Session[CDictionary.SK_ACCOUNT].ToString()))
            {
                return Content("<span>你還沒登入喔<span>");
            }
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            tMember tM = db.tMembers.FirstOrDefault(p => p.fAccount == s1);
            List<tProduct> tp = null;
            tp = playlist.getUserPlaylistNormal(s1);
            if (tp.Count != 0)
            {
                return View(tp);
            }
            else
            {
                return View();
            }
        }

        public ActionResult AlbumInfo(int amid , bool ajax = false)
        {
    //        Session[CDictionary.SK_ACCOUNT] = "aaa";
            //     ViewBag.AlbInfo = db.tAlbums.FirstOrDefault(p => p.fAlbumID == amid);
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
        public ActionResult DelteSong(int amid)
        {
            string s1 = "AlbumInfo?amid=" + Session["albumid"].ToString();
            string s2 = Session[CDictionary.SK_ACCOUNT].ToString();
            ViewBag.Msg = album.userDelteSong(amid, s2);
            TempData["message"] = ViewBag.Msg;
            //return Content(ViewBag.Msg);//除錯用
            return Redirect(s1);
        }

        //刪除專輯
        public ActionResult DeleteAlbum(int amid)
        {
            string s1 = "MyAlbumList?account=" + Session[CDictionary.SK_ACCOUNT].ToString();
            string s2 = Session[CDictionary.SK_ACCOUNT].ToString();
            ViewBag.Msg = album.userDeleteAlbum(amid,s2);
            TempData["message"] = ViewBag.Msg;
            //return Content(ViewBag.Msg); //除錯用
            return Redirect(s1);
        }

        //更新單曲
        [HttpPost]
        public ActionResult updateSongTryLimit(int amid, float start, float end)
        {
            string s1 = "AlbumInfo?amid=" + Session["albumid"].ToString();
            ViewBag.Msg = album.userUpdateSongTryLimit(amid, start, end);
            TempData["message"] = ViewBag.Msg;
            return Json(ViewBag.Msg);
        }

        //更新專輯
        [HttpPost]
        public JsonResult updateAlbumInfoFile(FormCollection formCollection, HttpPostedFileBase fCoverPathUpload)
        {
            string s1 = Session["albumid"].ToString();
            if(fCoverPathUpload != null)
            {
            ViewBag.Msg = album.userUpdateAlbumFile(s1, fCoverPathUpload);
            }
            ViewBag.Msg = album.userUpdateAlbumInfo(s1, formCollection);
            TempData["message"] = ViewBag.Msg;
            return Json(ViewBag.Msg);
        }

        //更新專輯
        public ActionResult _updateSongInterface(string soid)
        {
            int intSoid = Int32.Parse(soid);
            tProduct tP = (new dbProjectMusicStoreEntities()).tProducts.FirstOrDefault(p => p.fProductID == intSoid);
            if (tP != null)
            {
                return PartialView("_updateSongInterface", tP);
            }
            else
            {
                return PartialView("_updateSongInterface");
            }
        }

        //重新上傳單曲
        public ActionResult updateSong(FormCollection formCollection, HttpPostedFileBase fRealFile)
        {
            string fProductID = formCollection["fProductID"];
         //   string fProductID = "5";
            if (fRealFile != null)
            {
                ViewBag.Msg = album.userUpdateSongFile(fProductID, fRealFile);
            }
            ViewBag.Msg = album.userUpdateSongInfo(fProductID, formCollection);
            TempData["message"] = ViewBag.Msg;
            return Json(ViewBag.Msg);
        }

        //更新單曲
        [HttpPost]
        public JsonResult updateSongInfoAjax(string soid,FormCollection formCollection)
        {
            ViewBag.Msg = album.userUpdateSongInfoDetail(soid, formCollection);
            TempData["message"] = ViewBag.Msg;
            return Json(ViewBag.Msg);
        }

        //更新專輯Kind畫面開啟
        public ActionResult _kindReviseInterface()
        {
            return PartialView("_kindReviseInterface");
        }

        //更新專輯Kind處理
        public ActionResult kindRevise(string tAfKinds)
        {
             string s1 = Session["albumid"].ToString();
            ViewBag.Msg = album.userUpdateAlbumKind(s1, tAfKinds);
            TempData["message"] = ViewBag.Msg;
            return RedirectToAction("AlbumInfo", "Album", new { amid = s1 });
        }
    }
}