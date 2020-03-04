using MusicPrj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MusicPrj.Controllers
{
    public class StatisticController : Controller
    {

        private dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();

        //該會員近6個月試聽總數
        public ActionResult getStatistic_tryNum()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 1)
                .GroupBy(p => new { year = p.fTime.Value.Year, month = p.fTime.Value.Month })
                .Select(p => new { date = p.Key.year + "-" + p.Key.month, CNT = p.Count() })).OrderBy(p => p.date).ToList();
            //string a = "從:";
            //foreach (var c in q.ToList())
            //{
            //    a += ",dt:" + c.date + "cnt:" + c.CNT;
            //}
            //return Content(a);
            string[] yearMonth = new string[5];
            int[] pickCount = new int[5];
            int m_Cnt = q.Count() > 5 ? 5 : q.Count();
            // int m_Cnt = 5;
            for (int i = 0; i < m_Cnt; i++)
            {
                if (q[i].date != null)
                {
                    yearMonth[i] = q[i].date;
                    pickCount[i] = q[i].CNT;
                }
                else
                {
                    yearMonth[i] = "";
                    pickCount[i] = 0;
                }
            }
            return Json(q.Take(6), JsonRequestBehavior.AllowGet);
        }

        //該會員近6個月專輯/單曲被購買總數
        public ActionResult getStatistic_purchaseNum()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && (p.fType == 2 || p.fType==3))
                .GroupBy(p => new { year = p.fTime.Value.Year, month = p.fTime.Value.Month })
                .Select(p => new { date = p.Key.year + "-" + p.Key.month, CNT = p.Count() })).OrderBy(p => p.date).ToList();
            return Json(q.Take(6), JsonRequestBehavior.AllowGet);
        }

        //該會員試聽購買占比
        public ActionResult getStatistic_purchaseTry()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1)
                .GroupBy(p => new { pickType = p.fType })
                .Select(p => new { useType = p.Key.pickType, CNT = p.Count()})).ToList();
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        //該會員被試聽週間變化
        public ActionResult getStatistic_tryWeek()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 1)
                .AsEnumerable()
                .GroupBy(p => new { week = p.fTime.Value.DayOfWeek })
                .Select(p => new { weekOf = p.Key.week, CNT = p.Count() })).OrderBy(p => p.weekOf).ToList();
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        //該會員賣最好5首單曲
        public ActionResult getStatistic_memberTop5Song()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 2)
                .GroupBy(p => new { songid = p.tProduct.fProductName })
                .Select(p => new { Song = p.Key.songid, Money = p.Sum(x=>x.tProduct.fSIPrice),CNT =p.Count() })).OrderByDescending(p => p.Money).ToList();
            return Json(q.Take(5), JsonRequestBehavior.AllowGet);
        }

        //該會員賣最好5首專輯
        public ActionResult getStatistic_memberTop5Album()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 3)
                .GroupBy(p => new { albumid = p.tProduct.tAlbum.fAlbumName })
                .Select(p => new { Album = p.Key.albumid, Money = p.Sum(x => (float)x.tProduct.tAlbum.fALPrice*x.tProduct.tAlbum.fDiscount), CNT= p.Count() })).OrderByDescending(p => p.Money).ToList();
            return Json(q.Take(5), JsonRequestBehavior.AllowGet);
        }

        //該會員本周最多試聽5首單曲
        public ActionResult getStatistic_memberTryWeekTop5Song()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 1 && p.fTime> System.Data.Entity.DbFunctions.AddDays(DateTime.Now,-7) )
                .GroupBy(p => new { songid = p.tProduct.fProductName,albumid =p.tProduct.tAlbum.fAlbumName })
                .Select(p => new { Song = p.Key.songid,Album= p.Key.albumid, CNT = p.Count() })).OrderByDescending(p => p.CNT).ToList();
            return Json(q.Take(5), JsonRequestBehavior.AllowGet);
        }

        //該會員本月最多試聽5首單曲
        public ActionResult getStatistic_memberTryMonthTop5Song()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 1 && p.fTime > System.Data.Entity.DbFunctions.AddDays(DateTime.Now, -30))
                .GroupBy(p => new { songid = p.tProduct.fProductName, albumid = p.tProduct.tAlbum.fAlbumName })
                .Select(p => new { Song = p.Key.songid, Album = p.Key.albumid, CNT = p.Count() })).OrderByDescending(p => p.CNT).ToList();
            return Json(q.Take(5), JsonRequestBehavior.AllowGet);
        }

        //該會員本月單曲
        public ActionResult getStatistic_memberSongListTable()
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var q = (db.tProducts.Where(p => p.tAlbum.fAccount == s1)
                .Select(p => new {ID=p.tAlbum.fAlbumID, Song = p.fProductName, Album = p.tAlbum.fAlbumName, Singer = p.fSinger,Composer=p.fComposer,  tryStart = p.fPlayStart, tryEnd = p.fPlayEnd, Price =p.fSIPrice, SongPurchase = p.tStatistics.Count(x => x.fProductID == p.fProductID && x.fType == 2) })).OrderByDescending(p => p.Album).ToList();
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        public ActionResult myProductSearch(string product)
        {
            string s1 = Session[CDictionary.SK_ACCOUNT].ToString();
            var q = (db.tProducts.Where(p => p.tAlbum.fAccount == s1 && p.fProductName.Contains(product))
                .Select(p => new { ID = p.tAlbum.fAlbumID, Song = p.fProductName, Album = p.tAlbum.fAlbumName, Singer = p.fSinger, Composer = p.fComposer, tryStart = p.fPlayStart, tryEnd = p.fPlayEnd, Price = p.fSIPrice, SongPurchase = p.tStatistics.Count(x => x.fProductID == p.fProductID && x.fType == 2) })).OrderByDescending(p => p.Album).ToList();
            return Json(q, JsonRequestBehavior.AllowGet);
        }


        public ActionResult findProd(string term = "")
        {
            var getresult = db.tProducts.Where(p => p.fProductName.StartsWith(term)).Select(p => p.fProductName);
            return Json(getresult, JsonRequestBehavior.AllowGet);
        }
    }
}