using MainWork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainWork.Models
{
    public class CSearch
    {
        dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        //前兩個方法是用來初始化進階搜尋內容的方法
        public IEnumerable<tAlbumKind> takeAllKind()
        {
            var result = db.tAlbumKinds.Select(p => p);
            return result;
        }
        
        public IEnumerable<tAlbumType> takeAllType()
        {
            var result = db.tAlbumTypes.Select(p => p).ToList();
            return result;
        }

        //首頁商品項目，選取所有商品
        public IEnumerable<tAlbum> allAlbum()
        {
            var all = db.tAlbums.Where(a => a.fStatus == 2).OrderByDescending(a => a.fYear);
            return all;
        }

        //曲風頁面點選查詢
        public IEnumerable<tAlbum> byKindPage(int kindID)
        {
            string kind = db.tAlbumKinds.Where(s => s.KindID == kindID).First().KindName;
            var result = db.tAlbums.Where(a => a.fKinds.Contains(kind) && a.fStatus ==2).OrderByDescending(a => a.fYear);
            return result;
        }

        //基礎關鍵字搜尋，只會找符合條件的歌名，但要透過歌名回傳相對應的專輯
        public IEnumerable<CSearchResult> byKeyword(string keyword)
        {
            var result = from p in db.tProducts
                         where p.fProductName.Contains(keyword) && p.tAlbum.fStatus == 2
                         orderby p.tAlbum.fYear descending
                         select new CSearchResult { album = p.tAlbum, product = p };
            return result;
        }

        //進階搜索，找的是符合條件的專輯
        public IEnumerable<tAlbum> byAdvanced(CAdvancedSearchObject keyObj)
        {
            //因為此處作法是先挑音樂再過濾出專輯，所以沒有加入音樂的專輯也不會被找到
            var data = from p in db.tProducts
                       where p.tAlbum.fStatus == 2
                       orderby p.tAlbum.fYear descending
                       select new CSearchResult { album = p.tAlbum, product = p };

            //比對專輯裡的音樂名稱是否符合
            if (!string.IsNullOrEmpty(keyObj.adSong))
            {
                data = data.Where(p => p.product.fProductName.Contains(keyObj.adSong));
            }
            if (!string.IsNullOrEmpty(keyObj.adSinger))
            {
                data = data.Where(p => p.product.fSinger.Contains(keyObj.adSinger));
            }
            if (!string.IsNullOrEmpty(keyObj.adComposer))
            {
                data = data.Where(p => p.product.fArranger.Contains(keyObj.adComposer));
            }
            if (!string.IsNullOrEmpty(keyObj.adAlbum))
            {
                data = data.Where(a => a.album.fAlbumName.Contains(keyObj.adAlbum));
            }
            if (!string.IsNullOrEmpty(keyObj.adGroup))
            {
                data = data.Where(a => a.album.fMaker.Contains(keyObj.adGroup));
            }
            if (keyObj.adType != 1)//不指定的id為1，資料庫若有更改得做修正
            {
                data = data.Where(a => a.album.fType == keyObj.adType);
            }
            if (keyObj.adKinds != null)
            {
                string[] adkinds = keyObj.adKinds.Split(',');
                for (int i = 0; i < adkinds.Length; i++)
                {
                    //lambda似乎不能直接接受陣列[i]的內容，必須先轉換成一個變數
                    string kind = adkinds[i];
                    data = data.Where(a => a.album.fKinds.Contains(kind));
                }
            }

            //因為前面所挑出來的專輯可能會重複，因此要再過濾一次重複的專輯
            List<tAlbum> result = new List<tAlbum>();
            foreach(var a in data)
            {
                if (!result.Contains(a.album))
                {
                    result.Add(a.album);
                }
            }
            return result;
        }

        //以下後臺用
        //尋找屬於特定活動的專輯
        public IEnumerable<tAlbum> byEvent(int eventId)
        {
            var result = db.tAlbums.Where(a => a.fActivityID == eventId && a.fStatus == 2).OrderByDescending(a => a.fYear); ;
            return result;
        }

        internal object MusicManage(string keyword, string method)
        {
            var result = new List<object>();
            var albums = new List<tAlbum>();
            if (method == "account")
            {
                albums = db.tAlbums.Where(a => a.fAccount.Contains(keyword) && a.fStatus == 2).OrderByDescending(a => a.fYear).ToList();
            }
            else if (method == "group")
            {
                albums = db.tAlbums.Where(a => a.fMaker.Contains(keyword) && a.fStatus == 2).OrderByDescending(a => a.fYear).ToList();
            }
            else if (method == "albumName")
            {
                albums = db.tAlbums.Where(a => a.fAlbumName.Contains(keyword) && a.fStatus == 2).OrderByDescending(a => a.fYear).ToList();
            }
            albums.OrderBy(a => a.fYear);

            foreach (var a in albums)
            {
                List<tProduct> musics = new List<tProduct>();
                foreach (var p in a.tProducts)
                {
                    var pro = new tProduct()
                    {
                        fProductName = p.fProductName,
                        fSinger = p.fSinger,
                        fComposer = p.fComposer,
                        fSIPrice = p.fSIPrice,
                        fFilePath = p.fFilePath
                    };
                    musics.Add(pro);
                }
                tAlbum album = new tAlbum()
                {
                    fAlbumID = a.fAlbumID,
                    fYear = a.fYear,
                    fCoverPath = a.fCoverPath,
                    fAccount = a.fAccount,
                    fMaker = a.fMaker,
                    fAlbumName = a.fAlbumName,
                    fALPrice = a.fALPrice
                };
                result.Add(new { album, musics });
            }
            return result;
        }

        internal object accountManage(string keyword)
        {
            List<object> result = new List<object>();
            var members = db.tMembers.Where(m => m.fAccount.Contains(keyword)).ToList();
            foreach(var m in members)
            {
                var member = new
                {
                    m.fAccount,
                    m.fNickName,
                    m.fEmail,
                    m.fPrivilege,
                    m.fSubscriptStartDate,
                    m.fSubscriptEndDate,
                    m.fMoney
                };
                result.Add(member);
            }
            return result;
        }
    }
}