using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainWork.ViewModel
{
    public class CLoginViewModel
    {
        public string emailoraccount { get; set; }
        public string password { get; set; }
    }
    public class LineLoginToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
    }

    public class LineUserProfile
    {
        public string userId { get; set; }
        public string displayName { get; set; }
        public string pictureUrl { get; set; }
        public string statusMessage { get; set; }
        public string email { get; set; }
    }
}