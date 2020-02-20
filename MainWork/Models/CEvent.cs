using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainWork.Models
{
    public class CEvent
    {
        dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        //活動查詢
        public IEnumerable<tActivity> eventQuery()
        {
            var result = db.tActivities.Select(a => a);
            return result;
        }
    }
}