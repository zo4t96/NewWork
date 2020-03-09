using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainWork.Models
{
    public class CStatistic
    {
        private dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();

        public dynamic userGettryNum(string s1)
        {
            var q = db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 1)
           .GroupBy(p => new { year = p.fTime.Value.Year, month = p.fTime.Value.Month })
           .Select(p => new { date = p.Key.year + "-" + p.Key.month, CNT = p.Count() }).OrderBy(p => p.date).ToList();
            //string a = "從:";
            //foreach (var c in q.ToList())
            //{
            //    a += ",dt:" + c.date + "cnt:" + c.CNT;
            //}
            //return Content(a);
            //string[] yearMonth = new string[5];
            //int[] pickCount = new int[5];
            //int m_Cnt = q.Count() > 5 ? 5 : q.Count();
            // int m_Cnt = 5;
            //for (int i = 0; i < m_Cnt; i++)
            //{
            //    if (q[i].date != null)
            //    {
            //        yearMonth[i] = q[i].date;
            //        pickCount[i] = q[i].CNT;
            //    }
            //    else
            //    {
            //        yearMonth[i] = "";
            //        pickCount[i] = 0;
            //    }
            //}
            return q.Take(6);
        }

        public dynamic adminGettryNum()
        {
            var q = db.tStatistics.Where(p => p.fType == 1)
           .GroupBy(p => new { year = p.fTime.Value.Year, month = p.fTime.Value.Month })
           .Select(p => new { date = p.Key.year + "-" + p.Key.month, CNT = p.Count() }).OrderBy(p => p.date).ToList();
            return q.Take(6);
        }

        public dynamic userGetpurchaseNum(string s1)
        {
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && (p.fType == 2 || p.fType == 3))
            .GroupBy(p => new { year = p.fTime.Value.Year, month = p.fTime.Value.Month })
            .Select(p => new { date = p.Key.year + "-" + p.Key.month, CNT = p.Count() })).OrderBy(p => p.date).ToList();
            return q.Take(6);
        }

        public dynamic adminGetpurchaseNum()
        {
            var q = (db.tStatistics.Where(p => (p.fType == 2 || p.fType == 3))
            .GroupBy(p => new { year = p.fTime.Value.Year, month = p.fTime.Value.Month })
            .Select(p => new { date = p.Key.year + "-" + p.Key.month, CNT = p.Count() })).OrderBy(p => p.date).ToList();
            return q.Take(6);
        }

        public dynamic userGetpurchaseTry(string s1)
        {
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1)
            .GroupBy(p => new { pickType = p.fType })
            .Select(p => new { useType = p.Key.pickType, CNT = p.Count() })).ToList();
            return q;
        }

        public dynamic adminGetpurchaseTry()
        {
            var q = (db.tStatistics.Where(p => p.fType == 1 || p.fType == 2 || p.fType == 3)
            .GroupBy(p => new { pickType = p.fType })
            .Select(p => new { useType = p.Key.pickType, CNT = p.Count() })).ToList();
            return q;
        }

        public dynamic userGettryWeek(string s1)
        {
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 1)
            .AsEnumerable()
            .GroupBy(p => new { week = p.fTime.Value.DayOfWeek })
            .Select(p => new { weekOf = p.Key.week, CNT = p.Count() })).OrderBy(p => p.weekOf).ToList();
            return q;
        }

        public dynamic adminGettryWeek()
        {
            var q = (db.tStatistics.Where(p => p.fType == 1)
            .AsEnumerable()
            .GroupBy(p => new { week = p.fTime.Value.DayOfWeek })
            .Select(p => new { weekOf = p.Key.week, CNT = p.Count() })).OrderBy(p => p.weekOf).ToList();
            return q;
        }

        public dynamic userGetmemberTop5Song(string s1)
        {
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 2)
            .GroupBy(p => new { songid = p.tProduct.fProductName })
            .Select(p => new { Song = p.Key.songid, Money = p.Sum(x => x.tProduct.fSIPrice), CNT = p.Count() })).OrderByDescending(p => p.Money).ToList();
            return q.Take(5);
        }

        public dynamic adminGetmemberTop5Song()
        {
            var q = (db.tStatistics.Where(p => p.fType == 2)
            .GroupBy(p => new { songid = p.tProduct.fProductName })
            .Select(p => new { Song = p.Key.songid, Money = p.Sum(x => x.tProduct.fSIPrice), CNT = p.Count() })).OrderByDescending(p => p.Money).ToList();
            return q.Take(5);
        }

        public dynamic userGetmemberTop5Album(string s1)
        {
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 3)
            .GroupBy(p => new { albumid = p.tProduct.tAlbum.fAlbumName })
            .Select(p => new { Album = p.Key.albumid, Money = p.Sum(x => (float)x.tProduct.tAlbum.fALPrice * x.tProduct.tAlbum.fDiscount), CNT = p.Count() })).OrderByDescending(p => p.Money).ToList();
            return q.Take(5);
        }

        public dynamic adminGetmemberTop5Album()
        {
            var q = (db.tStatistics.Where(p => p.fType == 3)
            .GroupBy(p => new { albumid = p.tProduct.tAlbum.fAlbumName })
            .Select(p => new { Album = p.Key.albumid, Money = p.Sum(x => (float)x.tProduct.tAlbum.fALPrice * x.tProduct.tAlbum.fDiscount), CNT = p.Count() })).OrderByDescending(p => p.Money).ToList();
            return q.Take(5);
        }

        public dynamic userGetmemberTryWeekTop5Song(string s1)
        {
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 1 && p.fTime > System.Data.Entity.DbFunctions.AddDays(DateTime.Now, -7))
            .GroupBy(p => new { songid = p.tProduct.fProductName, albumid = p.tProduct.tAlbum.fAlbumName })
            .Select(p => new { Song = p.Key.songid, Album = p.Key.albumid, CNT = p.Count() })).OrderByDescending(p => p.CNT).ToList();
            return q.Take(5);
        }

        public dynamic adminGetmemberTryWeekTop5Song()
        {
            var q = (db.tStatistics.Where(p => p.fType == 1 && p.fTime > System.Data.Entity.DbFunctions.AddDays(DateTime.Now, -7))
            .GroupBy(p => new { songid = p.tProduct.fProductName, albumid = p.tProduct.tAlbum.fAlbumName })
            .Select(p => new { Song = p.Key.songid, Album = p.Key.albumid, CNT = p.Count() })).OrderByDescending(p => p.CNT).ToList();
            return q.Take(5);
        }

        public dynamic userGetmemberTryMonthTop5Song(string s1)
        {
            var q = (db.tStatistics.Where(p => p.tProduct.tAlbum.fAccount == s1 && p.fType == 1 && p.fTime > System.Data.Entity.DbFunctions.AddDays(DateTime.Now, -30))
            .GroupBy(p => new { songid = p.tProduct.fProductName, albumid = p.tProduct.tAlbum.fAlbumName })
            .Select(p => new { Song = p.Key.songid, Album = p.Key.albumid, CNT = p.Count() })).OrderByDescending(p => p.CNT).ToList();
            return q.Take(5);
        }

        public dynamic adminGetmemberTryMonthTop5Song()
        {
            var q = (db.tStatistics.Where(p => p.fType == 1 && p.fTime > System.Data.Entity.DbFunctions.AddDays(DateTime.Now, -30))
            .GroupBy(p => new { songid = p.tProduct.fProductName, albumid = p.tProduct.tAlbum.fAlbumName })
            .Select(p => new { Song = p.Key.songid, Album = p.Key.albumid, CNT = p.Count() })).OrderByDescending(p => p.CNT).ToList();
            return q.Take(5);
        }

        public dynamic userGetmemberSongListTable(string s1)
        {
            var q = (db.tProducts.Where(p => p.tAlbum.fAccount == s1)
            .Select(p => new { ID = p.tAlbum.fAlbumID, Song = p.fProductName, Album = p.tAlbum.fAlbumName, Singer = p.fSinger, Composer = p.fComposer, tryStart = p.fPlayStart, tryEnd = p.fPlayEnd, Price = p.fSIPrice, SongPurchase = p.tStatistics.Count(x => x.fProductID == p.fProductID && x.fType == 2) })).OrderByDescending(p => p.Album).ToList();
            return q;
        }

        public dynamic userGetmyProductSearch(string s1, string product)
        {
            var q = (db.tProducts.Where(p => p.tAlbum.fAccount == s1 && p.fProductName.Contains(product))
            .Select(p => new { ID = p.tAlbum.fAlbumID, Song = p.fProductName, Album = p.tAlbum.fAlbumName, Singer = p.fSinger, Composer = p.fComposer, tryStart = p.fPlayStart, tryEnd = p.fPlayEnd, Price = p.fSIPrice, SongPurchase = p.tStatistics.Count(x => x.fProductID == p.fProductID && x.fType == 2) })).OrderByDescending(p => p.Album).ToList();
            return q;
        }

    }
}