﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainWork.ViewModels
{
    public class CEventObject
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string eventName { get; set; }
        public int[] eventAlbums { get; set; }
        public HttpPostedFileBase eventImage { get; set; }
        public double discount { get; set; }
    }
}