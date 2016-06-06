//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FrameworkLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class Settings
    {
        public long ID { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateLastModified { get; set; }
        public string GlobalCodeInHead { get; set; }
        public string GlobalCodeInBody { get; set; }
        public decimal ShoppingCartTax { get; set; }
        public int MaxRequestLength { get; set; }
        public int MaxUploadFileSizePerFile { get; set; }
        public System.DateTime SiteOnlineAtDateTime { get; set; }
        public Nullable<System.DateTime> SiteOfflineAtDateTime { get; set; }
        public string SiteOfflineUrl { get; set; }
        public long OutputCacheDurationInSeconds { get; set; }
        public long DefaultLanguageID { get; set; }
        public long DefaultMasterPageID { get; set; }
        public string PageNotFoundUrl { get; set; }
        public bool EnableGlossaryTerms { get; set; }
    
        public virtual Language DefaultLanguage { get; set; }
        public virtual MasterPage DefaultMasterPage { get; set; }
    }
}
