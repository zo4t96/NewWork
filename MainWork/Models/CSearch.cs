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
        public IEnumerable<tProductKind> takeAllKind()
        {
            var result = db.tProductKinds.Select(p => p);
            return result;
        }
        public IEnumerable<tAlbumType> takeAllType()
        {
            var result = db.tAlbumTypes.Select(p => p).ToList();
            return result;
        }

        //基礎關鍵字搜尋，只會找歌名，但要透過歌名回傳相對應的專輯
        public IEnumerable<CSearchResult> byKeyword(string keyword)
        {
            var result = from p in db.tProducts
                         where p.fProductName.Contains(keyword)
                         select new CSearchResult { album = p.tAlbum, product = p };
            return result;
        }
    }
}