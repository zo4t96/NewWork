//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MainWork
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class tAlbum
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tAlbum()
        {
            this.tProducts = new HashSet<tProduct>();
        }
    
        public int fAlbumID { get; set; }
        public string fAlbumName { get; set; }
        public string fMaker { get; set; }
        public string fAccount { get; set; }
        public string fYear { get; set; }
        public Nullable<int> fType { get; set; }
        public Nullable<int> fStatus { get; set; }
        public Nullable<decimal> fALPrice { get; set; }
        public string fCoverPath { get; set; }
        public string fPublisher { get; set; }
        public string fKinds { get; set; }
        public Nullable<float> fDiscount { get; set; }
        public Nullable<int> fActivity { get; set; }
        public HttpPostedFileBase fCoverRealFile { get; set; }

        public virtual tAlbumType tAlbumType { get; set; }
        public virtual tMember tMember { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tProduct> tProducts { get; set; }
        public virtual tActivity tActivity { get; set; }
    }
}
