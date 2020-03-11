//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MusicPrj
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tAlbum
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tAlbum()
        {
            this.tProducts = new HashSet<tProduct>();
        }

        public int fAlbumID { get; set; }
        [Display(Name = "專輯名稱")]
        [Required(ErrorMessage = "專輯名稱必填")]
        public string fAlbumName { get; set; }
        [Display(Name = "發行團體")]
        [Required(ErrorMessage = "發行團體必填")]
        public string fMaker { get; set; }
        public string fAccount { get; set; }
        [Display(Name = "最後修改時間")]
        public Nullable<System.DateTime> fYear { get; set; }
        [Display(Name = "風格/類別")]
        public Nullable<int> fType { get; set; }
        public Nullable<int> fStatus { get; set; }
        [Display(Name = "專輯售價")]
        [Required(ErrorMessage = "專輯售價必填")]
        public Nullable<decimal> fALPrice { get; set; }
        [Display(Name = "封面")]
        public string fCoverPath { get; set; }
        [Display(Name = "專輯類型")]
        public string fKinds { get; set; }
        public Nullable<float> fDiscount { get; set; }
        public Nullable<int> fActivityID { get; set; }

        public virtual tActivity tActivity { get; set; }
        public virtual tAlbumType tAlbumType { get; set; }
        public virtual tMember tMember { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tProduct> tProducts { get; set; }
    }
}
