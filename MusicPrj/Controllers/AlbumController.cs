using MainWork.Models;
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
using MainWork.ViewModels;
using prjSpotifyProject.ViewModels;

namespace MainWork.Controllers
{
    public class testO
    {
        public int num { get; set; }
    }

    public class AlbumController : Controller
    {
        private dbProjectMusicStoreEntities1 db = new dbProjectMusicStoreEntities1();
        CPlaylist playlist = new CPlaylist();
        CAlbum album = new CAlbum();
        // GET: Album

        //測試用網頁
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

        public ActionResult Ordertest()
        {
            List<testO> ls = new List<testO> { };
            ls.Add(new testO { num = 1 });
            ls.Add(new testO { num = 2 });
            ls.Add(new testO { num = 3 });
            ls.Add(new testO { num = 4 });
            ls.Add(new testO { num = 5 });
            ls.Add(new testO { num = 6 });
            var lss = ls.Where(p => p.num >= 3).ToList();
            lss.AddRange(ls.Where(p => p.num < 3).ToList());
            string a = "從:";
            foreach (var c in lss)
            {
                a += "," + c.num;
            }
            return Content(a); ;
        }

        public ActionResult Index(bool ajax = false)
        {
            //      Session[CDictionary.SK_ACCOUNT] = "aaa";
            var album = from a in db.tAlbums
                        where a.fStatus == 1
                        select a;
            //     Session[CDictionary.SK_sideboxList1Id] = "1";
            CSearch cs = new CSearch();
            CEvent ce = new CEvent();
            ViewBag.events = ce.eventQuery();
            return View(album.ToList());
        }

        //商品頁面
        public ActionResult AlbumDetail(int amid, bool ajax = false)
        {
            ViewBag.AlbInfo = db.tAlbums.FirstOrDefault(p => p.fAlbumID == amid);
            var list = db.tProducts.Where(p => p.fAlbumID == amid);
            //         Session["account"] = "aaa";

            if (Session[CDictionary.SK_ACCOUNT] != null)
            {
                string nowAccount = Session[CDictionary.SK_ACCOUNT].ToString();
                ViewBag.privilege = db.tMembers.FirstOrDefault(m => m.fAccount == nowAccount).fPrivilege;
            }

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

        //播放器相關
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

        public ActionResult _PlayLists()
        {
            if (Session[CDictionary.SK_ACCOUNT] == null || string.IsNullOrWhiteSpace(Session[CDictionary.SK_ACCOUNT].ToString()))
            {
                return Content("");
            }
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            tMember tM = db.tMembers.FirstOrDefault(p => p.fAccount == s1);
            List<tProduct> tp = null;
            if (DateTime.Now < tM.fSubscriptEndDate)
            {
                ViewBag.memberUseType = "包月會員";
                ViewBag.memberSubscribeEnd = tM.fSubscriptEndDate.Value.ToShortDateString();
                tp = playlist.getUserPlaylistSubscribe(s1);
            }
            else
            {
                ViewBag.memberUseType = "一般會員";
                ViewBag.memberSubscribeEnd = "";
                tp = playlist.getUserPlaylistNormal(s1);
            }
            if (tp.Count != 0)
            {
                return PartialView("_PlayLists", tp);
            }
            else
            {
                //    return View();
                return PartialView("_PlayLists", tp);
            }
        }

        public ActionResult addPlayLists(int amid)
        {
            string s1 = "";
            try
            {
                s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            }
            catch
            {
                s1 = "";
            }
            string msg = playlist.userAddPlayLists(s1, amid);

            //只有成功時不跳alert
            if(msg == "成功")
            {
                return Content("成功");
            }
            else
            {
                return JavaScript("alert('" + msg + "');");
            }
        }

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
                    if (playmode != 4)
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
                        tM.fLastPlaySong = db.tPurchaseItems.FirstOrDefault(p => p.fCustomer == s1 && p.fProductID == tprodSec.fProductID).fProductID;
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

        [HttpPost]
        public ActionResult deletePlayLists(int deleteid)
        {
            string s1 = "";
            try
            {
                s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            }
            catch
            {
                s1 = "";
            }
            string Msg = playlist.userDeletePlayLists(s1, deleteid);
            if (Msg == "")
            {
                return JavaScript("");
            }
            else
            {
                return JavaScript("alert('" + Msg + "');");
            }
        }

        //創作者相關
        public ActionResult MyAlbumList(string account, bool ajax = false)
        {
            //Session[CDictionary.SK_ACCOUNT] = "aaa";
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var list = db.tAlbums.Where(p => p.fAccount == s1);
            return PartialView(list);
        }

        //創作者所有專輯
        public ActionResult _MyAlbumList()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var list = db.tAlbums.Where(p => p.fAccount == s1);
            return View(list);
        }

        public ActionResult _MyAlbumAddAlbum()
        {
            return PartialView();
        }

        public ActionResult _MyAlbumAnalysis()
        {
            return PartialView();
        }

        public ActionResult _MyAlbumProductList()
        {
            return PartialView();
        }

        public ActionResult AlbumInfo(int amid, bool ajax = false)
        {
            string s1 ="";
            if (Session[CDictionary.SK_ACCOUNT] == null)
            {
                return RedirectToAction("Main", "Homepage");
                //Response.Redirect("~/Homepage/Main");
            }
            else
            {
                s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            }
            var list = db.tProducts.Where(p => p.fAlbumID == amid);
            //非創作者或管理員
            if (db.tAlbums.FirstOrDefault(p=>p.fAlbumID == amid).fAccount != s1)
            {
                return RedirectToAction("Main", "Homepage");
                //Response.Redirect("~/Homepage/Main");
            }
            Session["albumid"] = amid;
            return View(list);
            
        }

        public ActionResult _LinePlayer()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            tMember tM = db.tMembers.FirstOrDefault(p => p.fAccount == s1);
            List<tProduct> tp = null;
            if (DateTime.Now < tM.fSubscriptEndDate)
            {
                ViewBag.memberUseType = "包月會員";
                ViewBag.memberSubscribeEnd = tM.fSubscriptEndDate.Value.ToShortDateString();
                tp = playlist.getUserPlaylistSubscribe(s1);
            }
            else
            {
                ViewBag.memberUseType = "一般會員";
                ViewBag.memberSubscribeEnd = "";
                tp = playlist.getUserPlaylistNormal(s1);
            }
            if (tp.Count != 0)
            {
                return PartialView("_LinePlayer", tp);
            }
            else
            {
                return PartialView("_PlayLists", tp);
            }
        }

        public ActionResult _LineLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _LineLogin(CLoginData data)
        {
            if (string.IsNullOrEmpty(data.txtAccount))
                return View("Search", "Kind");
            if (data != null)
            {
                string account = data.txtAccount;
                tMember cust = (new dbProjectMusicStoreEntities1()).tMembers.FirstOrDefault(p => p.fAccount == account);
                if (cust != null)
                {
                    if (cust.fPassword.Equals(data.txtPassword))
                    {
                        Session[CDictionary.SK_ACCOUNT] = account;
                        Session[CDictionary.SK_CURRENT_LOGINED_USER] = cust;
                        return RedirectToAction("_LinePlayer", "Album");
                    }
                }
                ViewBag.ErrorMessage = "帳號與密碼不符";
            }
            return View();
        }

        public ActionResult _LineAccountBinding()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            tMember tM = db.tMembers.FirstOrDefault(p => p.fAccount == s1);
            if (tM.fLineStatus > 0)
            {
                return View(tM);
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult LineAccountBindingSubmit()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            ViewBag.Msg = album.userBindLineAccount(s1);
            return JavaScript("alert('" + ViewBag.Msg + "');");
        }

        //已購買音樂
        public ActionResult MyMusic()
        {
            if (Session[CDictionary.SK_ACCOUNT] == null || string.IsNullOrWhiteSpace(Session[CDictionary.SK_ACCOUNT].ToString()))
            {
                return Content("你還沒登入喔");
            }
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            tMember tM = db.tMembers.FirstOrDefault(p => p.fAccount == s1);
            List<tProduct> tp = null;
            tp = playlist.getUserPlaylistNormalformusic(s1);
            if (tp != null)
            {
                return View(tp);
            }
            else
            {
                return View();
            }
        }

        public ActionResult MusicDownload(int songid)
        {
            //網址給其他人時的防護
            if (Session[CDictionary.SK_ACCOUNT] == null || string.IsNullOrWhiteSpace(Session[CDictionary.SK_ACCOUNT].ToString()))
            {
                return Content("你還沒登入喔");
            }
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            string mes = album.checkDownloadPrivilege(s1, songid);
            if (mes == "")
            {
                string filepath = db.tProducts.FirstOrDefault(p => p.fProductID == songid).fFilePath;
                string path = Server.MapPath(@"~\MusicFiles\" + filepath);
                //取得檔案名稱
                string filename = System.IO.Path.GetFileName(path);
                //讀成串流
                Stream iStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                return File(iStream, "application/unknown", filename);
            }
            else
            {
                return Content(mes);
            }
        }

        //##################################新增刪除修改子程式###################################################
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
        public ActionResult AlbumInfoUpload(FormCollection formCollection, HttpPostedFileBase tP_fRealFile)
        {
            if (formCollection == null)
            {
                return View();
            }
            int amid = Int32.Parse(Session["albumid"].ToString());
            ViewBag.Msg = album.userAddsong(formCollection, amid, tP_fRealFile);
            // string s1 = "AlbumInfo?amid=" + Session["albumid"].ToString();
            return JavaScript("alert('" + ViewBag.Msg + "');");
            //    return Redirect(s1);
        }

        //刪除單曲
        public ActionResult DelteSong(int amid)
        {
            string s1 = "AlbumInfo?amid=" + Session["albumid"].ToString();
            string s2 = Session[CDictionary.SK_ACCOUNT].ToString();
            ViewBag.Msg = album.userDelteSong(amid, s2);
            return JavaScript("alert('" + ViewBag.Msg + "');");
            //return Content(ViewBag.Msg);//除錯用
        }

        //刪除專輯
        public ActionResult DeleteAlbum(int amid)
        {
            string s1 = "MyAlbumList?account=" + Session[CDictionary.SK_ACCOUNT].ToString();
            string s2 = Session[CDictionary.SK_ACCOUNT].ToString();
            ViewBag.Msg = album.userDeleteAlbum(amid, s2);
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
            return Json(ViewBag.Msg);
        }

        //更新專輯
        [HttpPost]
        public ActionResult updateAlbumInfoFile(FormCollection formCollection, HttpPostedFileBase fCoverPathUpload)
        {
            string s1 = Session["albumid"].ToString();
            string s2 = Session[CDictionary.SK_ACCOUNT].ToString();
            if (fCoverPathUpload != null)
            {
                ViewBag.Msg = album.userUpdateAlbumFile(s1, fCoverPathUpload, s2);
            }
            ViewBag.Msg = album.userUpdateAlbumInfo(s1, formCollection, s2);
            // TempData["message"] = ViewBag.Msg;
            return JavaScript("alert('" + ViewBag.Msg + "');");
        }

        //專輯下架
        [HttpPost]
        public ActionResult updateAlbumInfoClose()
        {
            string s1 = Session["albumid"].ToString();
            if (s1 != null)
            {
                ViewBag.Msg = album.userUpdateAlbumCloseStatus(s1);
            }
            ViewBag.Msg = album.userUpdateAlbumCloseStatus(s1);
            // TempData["message"] = ViewBag.Msg;
            return JavaScript("alert('" + ViewBag.Msg + "');");
        }

        //更新專輯
        public ActionResult _updateSongInterface(string soid)
        {
            int intSoid = Int32.Parse(soid);
            tProduct tP = (new dbProjectMusicStoreEntities1()).tProducts.FirstOrDefault(p => p.fProductID == intSoid);
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
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            string fProductID = formCollection["fProductID"];
            //   string fProductID = "5";
            if (fRealFile != null)
            {
                ViewBag.Msg = album.userUpdateSongFile(fProductID, fRealFile, s1);
            }
            ViewBag.Msg = album.userUpdateSongInfo(fProductID, formCollection, s1);
            //  TempData["message"] = ViewBag.Msg;
            return JavaScript("alert('" + ViewBag.Msg + "');");
        }

        //更新單曲
        [HttpPost]
        public ActionResult updateSongInfoAjax(FormCollection formCollection)
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            ViewBag.Msg = album.userUpdateSongInfoDetail(formCollection, s1);
            //   TempData["message"] = ViewBag.Msg;
            return JavaScript("alert('" + ViewBag.Msg + "');");
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
            //TempData["message"] = ViewBag.Msg;
            return JavaScript("alert('" + ViewBag.Msg + "');");
        }

        //更新專輯Kind畫面開啟
        public ActionResult _kindAddInterface()
        {
            return PartialView("_kindAddInterface");
        }

   
    //03/08/2020新增使用自訂活動頁面(先放棄實作)
    public ActionResult _userEvent()
        {
            return PartialView();
        }
        public ActionResult userEventRenew(string account)
        {
            CEvent ce = new CEvent();
            var result = ce.eventQuery(account).Select(e => new { e.fTitle, e.fStartTime, e.fEndTime });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}