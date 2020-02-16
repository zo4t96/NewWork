using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPrj.Models
{
    public class CMusic
    {
        dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        public tProduct getMusic(int musicId)
        {
            var result = db.tProducts.Where(m => m.fProductID == musicId).FirstOrDefault();
            return result;
        }
    }
}