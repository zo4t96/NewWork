using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPrj.ViewModels
{
    public class CKindEditObject
    {
        public int kindId { get; set; }
        public string kindName { get; set; }
        public string kindColor { get; set; }
        public string uploadCheck { get; set; }
        public HttpPostedFileBase kindImage { get; set; }
    }
}