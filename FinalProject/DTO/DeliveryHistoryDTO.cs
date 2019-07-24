using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class DeliveryHistoryDTO
    {
        public int? ID { get; set; }
        public string DELIVERY_ID { get; set; }
        public int? CANDIDATE_ID { get; set; }
        public string CANDIDATE_POSITION { get; set; }
        public string SOURCE { get; set; }
        public int? CANDIDATE_STATE { get; set; }
        public int? LAST_PIC { get; set; }
        public System.DateTime? PROCESS_DATE { get; set; }
        public int? CLIENT_ID { get; set; }
        public System.DateTime? START_DATE { get; set; }
        public System.DateTime? LAST_UPDATE { get; set; }
        public System.DateTime? TOTA_DAY { get; set; }
        public string CLIENT_STATE { get; set; }
        public string NOTE { get; set; }
    }

    public class Manage_DeliveryHistoryDTO
    {
       public static List<DeliveryHistoryDTO> GetData()
        {
            using(DBEntities db = new DBEntities())
            {
                return db.TB_DELIVERY_HISTORY.Select(d => new DeliveryHistoryDTO {
                    ID = d.ID,
                    DELIVERY_ID = d.DELIVERY_ID,
                    CANDIDATE_ID = d.CANDIDATE_ID,
                    CANDIDATE_POSITION = d.CANDIDATE_POSITION,
                    SOURCE = d.SOURCE,
                    CANDIDATE_STATE = d.CANDIDATE_STATE,
                    LAST_PIC = d.LAST_PIC,
                    PROCESS_DATE = d.PROCESS_DATE,
                    CLIENT_ID = d.CLIENT_ID,
                    START_DATE = d.START_DATE,
                    LAST_UPDATE = d.LAST_UPDATE,
                    TOTA_DAY = d.TOTA_DAY,
                    CLIENT_STATE = d.CLIENT_STATE,
                    NOTE = d.NOTE
                }).ToList();
            }
       }
    }
}