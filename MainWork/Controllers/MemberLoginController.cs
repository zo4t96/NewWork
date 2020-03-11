using MainWork;
using MainWork.Models;
using MainWork.ViewModel;
using MainWork.ViewModels;
using Newtonsoft.Json;
using prjSpotifyProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace prjSpotifyProject.Controllers
{
    public class MemberLoginController : Controller
    {
        dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();

        //string redirect_uri;
        string redirect_uri = "http://114.34.9.151/test/MemberLogin/AfterLineLogin";//網址需要再雲端後看雲端網址是什麼再改
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
            string redirect_uri_1 = "http://";
            string redirect_uri_2 = Request.Url.Authority;
            string redirect_uri_3 = Url.Content("~") + "MemberLogin/AfterLineLogin";
            //redirect_uri = redirect_uri_1+ redirect_uri_2 + redirect_uri_3;
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
                if (tm == null)
                {
                    return Content("此帳號未被綁定,請改用一般方式登入");
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
        public ActionResult Login(CLoginData data, bool ajax = false)
        {
            //if (string.IsNullOrEmpty(data.txtAccount))
            //    return View("Search", "Kind");
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
                    return Content("false");
                }
                //ViewBag.ErrorMessage = "帳號或密碼錯誤";
            }
            return Content("false");
        }

        // Creat
        public ActionResult New()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult New(tMember m)
        {
            //string fileName = "";
            //fileName = Guid.NewGuid() + ".jpg";
            //var path = Path.Combine(Server.MapPath("~/Images"), fileName);
            //m.SaveAs(path);

            m.fPicPath = "nobody.jpg";
            db.tMembers.Add(m);
            db.SaveChanges();
            return RedirectToAction("Main","Homepage");
            //tMemberFactory factory = new tMemberFactory();
            //factory.create(c);

            //return RedirectToAction("Main","Homepage");
        }

        public ActionResult Logout()
        {
            Session[CDictionary.SK_CURRENT_LOGINED_USER] = null;
            Session[CDictionary.SK_ACCOUNT] = null;
            return RedirectToAction("Main", "Homepage");
        }

        public ActionResult LoginCheck(string fAccount)
        {
            string account = fAccount;
            tMember loginUser = db.tMembers.FirstOrDefault(m => m.fAccount == account);

            string  message = "此帳號已被使用";

            if (loginUser == null)
            {
                message = "可以使用的帳號";
            }

            if (account == "")
            {
                message = "請輸入帳號";
            }
            return Content(message);
        }
    }
}