using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPrj.Models
{
    public class CWebInitailize
    {
        public IEnumerable<tAlbumKind> kinds { get; set; }
        public IEnumerable<tAlbumType> types { get; set; }
        public CWebInitailize advancedInitial()
        {
            CTakeType tt = new CTakeType();
            CTakeKind tk = new CTakeKind();
            CWebInitailize ad = new CWebInitailize();
            ad.types = tt.takeAllType();
            ad.kinds = tk.takeAllKind();
            return ad;
        }
    }
}