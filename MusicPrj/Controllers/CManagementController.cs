using MusicPrj.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPrj.Controllers
{
    public class CManagementController : Controller
    {
        dbProjectMusicStoreEntities db = new dbProjectMusicStoreEntities();
        internal void typeNew(string typeName)
        {
            tAlbumType at = new tAlbumType();
            at.fTypeName = typeName;
            db.tAlbumTypes.Add(at);
            db.SaveChanges();
        }

        internal void typeDelete(int deleteId)
        {
            //刪除類別之前，把所有符合該類別的專輯id全部改為1(類別為不指定)，使這項目不會有NULL情況發生
            var al = db.tAlbums.Where(a => a.fType == deleteId);
            foreach (var a in al)
            {
                a.fType = 1;
            }
            db.SaveChanges();

            tAlbumType at = db.tAlbumTypes.Where(p => p.fTypeID == deleteId).FirstOrDefault();
            db.tAlbumTypes.Remove(at);
            db.SaveChanges();
        }

        internal void typeEdit(int typeOrigin, string typeChange)
        {
            tAlbumType at = db.tAlbumTypes.Where(p => p.fTypeID == typeOrigin).FirstOrDefault();
            at.fTypeName = typeChange;
            db.SaveChanges();
        }

        internal void kindNew(CKindEditObject kindObj, string severPath)
        {
            tAlbumKind kind = new tAlbumKind();
            kind.KindName = kindObj.kindName;
            kind.fColor = kindObj.kindColor;
            //如果有勾選上傳圖片才處理圖片內容
            //圖片檔名判斷已事先在前端限定jpg圖檔，因此此處就不多加條件判斷
            if (kindObj.uploadCheck == "true")
            {
                string filename = Guid.NewGuid().ToString() + ".jpg";
                string path = Path.Combine(severPath, filename);
                kindObj.kindImage.SaveAs(path);
                kind.fPhotoPath = filename;
            }
            db.tAlbumKinds.Add(kind);
            db.SaveChanges();
        }

        internal void kindEdit(CKindEditObject kindObj, string serverPath)
        {
            tAlbumKind kind = db.tAlbumKinds.Where(k => k.KindID == kindObj.kindId).FirstOrDefault();

            //修正曲風時假設有修正曲風名字，那麼原本擁有該曲風專輯的相關曲風會被剔除
            if (kind.KindName != kindObj.kindName)
            {
                var albums = db.tAlbums.Where(a => a.fKinds.Contains(kind.KindName));
                foreach (var a in albums)
                {
                    if (a.fKinds.Contains(kind.KindName + ","))
                    {
                        a.fKinds = a.fKinds.Replace(kind.KindName + ",", "");
                    }
                    else
                    {
                        a.fKinds = a.fKinds.Replace(kind.KindName, "");
                    }
                }
            }
            kind.fColor = kindObj.kindColor;
            kind.KindName = kindObj.kindName;
            if (kindObj.uploadCheck == "true")
            {
                string path = Path.Combine(serverPath, kind.fPhotoPath);
                kindObj.kindImage.SaveAs(path);
            }
            db.SaveChanges();
        }

        internal void kindDelete(int deleteId)
        {
            //刪除曲風之前先將相關專輯的相同曲風都刪除
            string kindName = db.tAlbumKinds.Where(k => k.KindID == deleteId).FirstOrDefault().KindName;
            var albums = db.tAlbums.Where(a => a.fKinds.Contains(kindName));
            foreach (var a in albums)
            {
                if (a.fKinds.Contains(kindName + ","))
                {
                    a.fKinds = a.fKinds.Replace(kindName + ",", "");
                }
                else
                {
                    a.fKinds = a.fKinds.Replace(kindName, "");
                }
            }
            db.SaveChanges();

            var kind = db.tAlbumKinds.Where(k => k.KindID == deleteId).FirstOrDefault();
            db.tAlbumKinds.Remove(kind);
            db.SaveChanges();
        }

        //音樂審核並寄送通知訊息給送審會員
        internal void pass(string account, int albumId)
        {
            var album = db.tAlbums.Where(a => a.fAlbumID == albumId).FirstOrDefault();
            album.fStatus = 2;
            album.fYear = DateTime.Now;

            tMessage msg = new tMessage();
            msg.fAccountFrom = "aaa";
            msg.fAccountTo = account;
            msg.fStatus = 1;
            msg.fTime = DateTime.Now;
            msg.fContent = $"您送審的專輯「{album.fAlbumName}」已通過審查並成功上架!";
            db.tMessages.Add(msg);
            db.SaveChanges();
        }
        internal void noPass(string account, int albumId)
        {
            var album = db.tAlbums.Where(a => a.fAlbumID == albumId).FirstOrDefault();
            album.fStatus = 0;

            tMessage msg = new tMessage();
            msg.fAccountFrom = "aaa";
            msg.fAccountTo = account;
            msg.fStatus = 1;
            msg.fTime = DateTime.Now;
            msg.fContent = $"您送審的專輯「{album.fAlbumName}」未通過審查!";
            db.tMessages.Add(msg);
            db.SaveChanges();
        }

        internal void multiplePass(string[] accounts, int[] albums)
        {
            foreach (var account in accounts)
            {
                foreach (var album in albums)
                {
                    var target = db.tAlbums.Where(a => a.fAlbumID == album).FirstOrDefault();
                    target.fStatus = 2;
                    target.fYear = DateTime.Now;

                    tMessage msg = new tMessage();
                    msg.fAccountFrom = "aaa";
                    msg.fAccountTo = account;
                    msg.fStatus = 1;
                    msg.fTime = DateTime.Now;
                    msg.fContent = $"您送審的專輯「{target.fAlbumName}」已通過審查並成功上架!";
                    db.tMessages.Add(msg);
                }
            }
            db.SaveChanges();
        }

        internal void multipleNoPass(string[] accounts, int[] albums)
        {
            foreach (var account in accounts)
            {
                foreach (var album in albums)
                {
                    var target = db.tAlbums.Where(a => a.fAlbumID == album).FirstOrDefault();
                    target.fStatus = 0;

                    tMessage msg = new tMessage();
                    msg.fAccountFrom = "aaa";
                    msg.fAccountTo = account;
                    msg.fStatus = 1;
                    msg.fTime = DateTime.Now;
                    msg.fContent = $"您送審的專輯「{target.fAlbumName}」未通過審查!";
                    db.tMessages.Add(msg);
                }
            }
            db.SaveChanges();
        }
        internal void recall(string account, int albumId)
        {
            var album = db.tAlbums.Where(a => a.fAlbumID == albumId).FirstOrDefault();
            album.fStatus = 0;

            tMessage msg = new tMessage();
            msg.fAccountFrom = "aaa";
            msg.fAccountTo = account;
            msg.fStatus = 1;
            msg.fTime = DateTime.Now;
            msg.fContent = $"您的專輯「{album.fAlbumName}」因有侵權之嫌因此強制下架，造成不便請多見諒";
            db.tMessages.Add(msg);
            db.SaveChanges();
        }
    }
}