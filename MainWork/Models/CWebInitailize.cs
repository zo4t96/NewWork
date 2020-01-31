using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainWork.Models
{
    public class CWebInitailize
    {
        public IEnumerable<tProductKind> kinds { get; set; }
        public IEnumerable<tAlbumType> types { get; set; }
        public CWebInitailize advancedInitial()
        {
            CSearch search = new CSearch();
            CWebInitailize ad = new CWebInitailize();
            ad.types = search.takeAllType();
            ad.kinds = search.takeAllKind();
            return ad;
        }
    }
}