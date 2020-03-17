using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainWork.Models
{
    public class CMusic
    {
        //挑音樂給播放器用的
        dbProjectMusicStoreEntities1 db = new dbProjectMusicStoreEntities1();
        public tProduct getMusic(int musicId)
        {
            var result = db.tProducts.Where(m => m.fProductID == musicId).FirstOrDefault();
            return result;
        }
    }
}