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

        //基礎關鍵字搜尋，只會找符合條件的歌名，但要透過歌名回傳相對應的專輯
        public IEnumerable<CSearchResult> byKeyword(string keyword)
        {
            var result = from p in db.tProducts
                         where p.fProductName.Contains(keyword)
                         select new CSearchResult { album = p.tAlbum, product = p };
            return result;
        }

        //進階搜索，找的是符合條件的專輯
        public IEnumerable<tAlbum> byAdvanced(CAdvancedSearchObject keyObj)
        {
            //因為此處作法是先挑音樂再過濾出專輯，所以沒有加入音樂的專輯也不會被找到
            var data = from p in db.tProducts
                       select new CSearchResult { album = p.tAlbum, product = p };

            //比對專輯裡的音樂名稱是否符合
            if (!string.IsNullOrEmpty(keyObj.adSong))
            {
                data = data.Where(p => p.product.fProductName.Contains(keyObj.adSong));
            }
            if (!string.IsNullOrEmpty(keyObj.adSinger))
            {
                data = data.Where(p => p.product.fArtist.Contains(keyObj.adSinger));
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
            if (keyObj.adType != 0)
            {
                data = data.Where(a => a.album.fType == keyObj.adType);
            }
            if(keyObj.adKinds != null)
            {
                for (int i = 0; i < keyObj.adKinds.Length; i++)
                {
                    //lambda似乎不能直接接受陣列[i]的內容，必須先轉換成一個變數
                    string kind = keyObj.adKinds[i];
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
    }
}