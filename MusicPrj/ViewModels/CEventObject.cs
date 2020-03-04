using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPrj.ViewModels
{
    public class CEventObject
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string eventName { get; set; }
        public int[] eventAlbums { get; set; }
        public HttpPostedFileBase eventImage { get; set; }
        public float discount { get; set; }

        //活動ID，修正時才會用到的屬性
        public int eventId { get; set; }
    }
}