using MusicPrj.Models;
using MusicPrj.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MusicPrj.Controllers
{
    public class MemberLoginController : Controller
    {

        string redirect_uri = "http://localhost/test/MemberLogin/AfterLineLogin";
        string client_id = "1653927630";
        string client_secret = "a92834ca3cb1d19554db273b160a659f";

        public ActionResult GetLineLoginUrl()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("");
            }

            //state使用隨機字串比較安全
            //每次Ajax Request都產生不同的state字串，避免駭客拿固定的state字串將網址掛載自己的釣魚網站獲取用戶的Line個資授權(CSRF攻擊)
            string state = Guid.NewGuid().ToString();
            TempData["state"] = state;//利用TempData被取出資料後即消失的特性，來防禦CSRF攻擊
            //如果是ASP.net Form，就改成放入Session或Cookie，之後取出資料時再把Session或Cookie設為null刪除資料
            string LineLoginUrl =
             $@"https://access.line.me/oauth2/v2.1/authorize?response_type=code&client_id={client_id}&redirect_uri={redirect_uri}&state={state}&scope={HttpUtility.UrlEncode("openid profile email")}";
            //scope給openid是程式為了抓id_token用，設email則為了id_token的Payload裡才會有用戶的email資訊
            return Content(LineLoginUrl);

        }

        /// <summary>
        /// 使用者在Line網頁登入後的處理，接收Line傳遞過來的參數
        /// </summary>
        /// <param name="state"></param>
        /// <param name="code"></param>
        /// <param name="error"></param>
        /// <param name="error_description"></param>
        /// <returns></returns>
        public ActionResult AfterLineLogin(string state, string code, string error, string error_description)
        {
            if (!string.IsNullOrEmpty(error))
            {//用戶沒授權你的LineApp
                ViewBag.error = error;
                ViewBag.error_description = error_description;
                return View();
            }

            if (TempData["state"] == null)
            {//可能使用者停留Line登入頁面太久

                return Content("頁面逾期");

            }

            if (Convert.ToString(TempData["state"]) != state)
            {//使用者原先Request QueryString的TempData["state"]和Line導頁回來夾帶的state Querystring不一樣，可能是parameter tampering或CSRF攻擊

                return Content("state驗證失敗");

            }

            if (Convert.ToString(TempData["state"]) == state)
            {//state字串驗證通過

                //取得id_token和access_token:https://developers.line.biz/en/docs/line-login/web/integrate-line-login/#spy-getting-an-access-token
                string issue_token_url = "https://api.line.me/oauth2/v2.1/token";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(issue_token_url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                //必須透過ParseQueryString()來建立NameValueCollection物件，之後.ToString()才能轉換成queryString
                NameValueCollection postParams = HttpUtility.ParseQueryString(string.Empty);
                postParams.Add("grant_type", "authorization_code");
                postParams.Add("code", code);
                postParams.Add("redirect_uri", this.redirect_uri);
                postParams.Add("client_id", this.client_id);
                postParams.Add("client_secret", this.client_secret);

                //要發送的字串轉為byte[] 
                byte[] byteArray = Encoding.UTF8.GetBytes(postParams.ToString());
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);
                }//end using

                //API回傳的字串
                string responseStr = "";
                //發出Request
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseStr = sr.ReadToEnd();
                    }//end using  
                }


                LineLoginToken tokenObj = JsonConvert.DeserializeObject<LineLoginToken>(responseStr);
                string id_token = tokenObj.id_token;

                //方案總管>參考>右鍵>管理Nuget套件 搜尋 System.IdentityModel.Tokens.Jwt 來安裝
                var jst = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(id_token);
                LineUserProfile user = new LineUserProfile();
                //↓自行決定要從id_token的Payload中抓什麼user資料
                user.userId = jst.Payload.Sub;
                user.displayName = jst.Payload["name"].ToString();
                //       user.pictureUrl = jst.Payload["picture"].ToString();
                //        if (jst.Payload.ContainsKey("email") && !string.IsNullOrEmpty(Convert.ToString(jst.Payload["email"])))
                //          {//有包含email，使用者有授權email個資存取，並且用戶的email有值
                //              user.email = jst.Payload["email"].ToString();
                //        }
                tMember tm = (new dbProjectMusicStoreEntities()).tMembers.FirstOrDefault(p => p.fLineId == user.userId);
                if(tm == null)
                {
                    return Content("此帳號未被綁定");
                }
                else
                {
                    Session[CDictionary.SK_ACCOUNT] = tm.fAccount;
                    Session[CDictionary.SK_CURRENT_LOGINED_USER] = tm;
                }

                string access_token = tokenObj.access_token;
                ViewBag.access_token = access_token;

                ViewBag.user = JsonConvert.SerializeObject(user, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                });


            }//end if 
          
            return View();
        }


        /// <summary>
        /// 徹銷Line Login，目前感覺不出差別在哪= =a，等待API改版
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RevokeLineLoginUrl(string access_token)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://api.line.me/oauth2/v2.1/revoke");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            //必須透過ParseQueryString()來建立NameValueCollection物件，之後.ToString()才能轉換成queryString
            NameValueCollection postParams = HttpUtility.ParseQueryString(string.Empty);
            postParams.Add("access_token", access_token);
            postParams.Add("client_id", this.client_id);
            postParams.Add("client_secret", this.client_secret);


            //要發送的字串轉為byte[] 
            byte[] byteArray = Encoding.UTF8.GetBytes(postParams.ToString());
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
            }//end using

            //API回傳的字串
            string responseStr = "";
            //發出Request
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseStr = sr.ReadToEnd();
                }//end using  
            }


            return Content(responseStr);
        }

        // 登入連結畫面
        public ActionResult Index()
        {
            if (Session[CDictionary.SK_CURRENT_LOGINED_USER] == null)
                return RedirectToAction("Login");
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        // 登入
        [HttpPost]
        public ActionResult Login(CLoginData data)
        {
            if (string.IsNullOrEmpty(data.txtAccount))
                return View("Search", "Kind");
            if (data != null)
            {
                string account = data.txtAccount;
                tMember cust = (new dbProjectMusicStoreEntities()).tMembers.FirstOrDefault(p => p.fAccount == account);
                if (cust != null)
                {
                    if (cust.fPassword.Equals(data.txtPassword))
                    {
                        Session[CDictionary.SK_ACCOUNT] = account;
                        Session[CDictionary.SK_CURRENT_LOGINED_USER] = cust;
                        return RedirectToAction("Main", "Homepage");
                    }
                }
                ViewBag.ErrorMessage = "帳號與密碼不符";
            }
            return View();
        }

        // Creat
        public ActionResult New()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult New(tMember c)
        {
            tMemberFactory factory = new tMemberFactory();
            factory.create(c);

            return RedirectToAction("Main", "Homepage");
        }

        public ActionResult Logout()
        {
            Session[CDictionary.SK_CURRENT_LOGINED_USER] = null;
            Session[CDictionary.SK_ACCOUNT] = null;
            return RedirectToAction("Main", "Homepage");
        }


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