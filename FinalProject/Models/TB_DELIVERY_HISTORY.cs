//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinalProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TB_DELIVERY_HISTORY
    {
        public int ID { get; set; }
        public string DELIVERY_ID { get; set; }
        public Nullable<int> CANDIDATE_ID { get; set; }
        public string CANDIDATE_POSITION { get; set; }
        public string SOURCE { get; set; }
        public Nullable<int> CANDIDATE_STATE { get; set; }
        public Nullable<int> LAST_PIC { get; set; }
        public Nullable<System.DateTime> PROCESS_DATE { get; set; }
        public Nullable<int> CLIENT_ID { get; set; }
        public Nullable<System.DateTime> START_DATE { get; set; }
        public Nullable<System.DateTime> LAST_UPDATE { get; set; }
        public Nullable<System.DateTime> TOTA_DAY { get; set; }
        public string CLIENT_STATE { get; set; }
        public string NOTE { get; set; }
    }
}