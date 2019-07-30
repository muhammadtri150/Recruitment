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
        public int? TOTA_DAY { get; set; }
        public string CLIENT_STATE { get; set; }
        public string NOTE { get; set; }
        public int SELECTION_ID { get; set; }
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
                    NOTE = d.NOTE,
                }).ToList();
                
            }
       }

       public static ClientDTO GetDataClient(int id)
       {
            return Manage_ClientDTO.GetData().FirstOrDefault(d => d.ID == id);
       }

        public static UserDTO GetDataPic(int id)
        {
            return Manage_UserDTO.GetDataUser().FirstOrDefault(d => d.USER_ID == id);
        }

        public static int AddData(DeliveryHistoryDTO Data)
        {
            using(DBEntities db = new DBEntities())
            {
                

                //get total day
                var total = DateTime.Now.Subtract(db.TB_CANDIDATE_SELECTION_HISTORY.FirstOrDefault(d => d.CANDIDATE_ID == Data.CANDIDATE_ID && d.CANDIDATE_STATE == 1).PROCESS_DATE.Value);
                int TotalDay = Convert.ToInt16(DateTime.Now.Subtract(db.TB_CANDIDATE_SELECTION_HISTORY.FirstOrDefault(d => d.CANDIDATE_ID == Data.CANDIDATE_ID && d.CANDIDATE_STATE == 1).PROCESS_DATE.Value).ToString("dddd"));

                //get process date
                var process_Date = db.TB_CANDIDATE_SELECTION_HISTORY.FirstOrDefault(d => d.CANDIDATE_ID == Data.CANDIDATE_ID && d.CANDIDATE_STATE == 1).PROCESS_DATE;

                //data candidate
                var DataCandidate = db.TB_CANDIDATE.FirstOrDefault(c => c.ID == Data.CANDIDATE_ID);

                db.TB_DELIVERY_HISTORY.Add(new TB_DELIVERY_HISTORY
                {
                    DELIVERY_ID = Data.DELIVERY_ID,
                    CANDIDATE_ID = Data.CANDIDATE_ID,
                    CANDIDATE_POSITION = Data.CANDIDATE_POSITION,
                    SOURCE = DataCandidate.SOURCE,
                    CANDIDATE_STATE = Data.CANDIDATE_STATE,
                    LAST_PIC = Data.LAST_PIC,
                    PROCESS_DATE = process_Date,
                    CLIENT_ID = Data.CLIENT_ID,
                    START_DATE = DateTime.Now,
                    LAST_UPDATE = DateTime.Now,
                    TOTA_DAY = TotalDay,
                    CLIENT_STATE = Data.CLIENT_STATE,
                    NOTE = Data.NOTE
                });

                return db.SaveChanges();
            }
        }

        public static int EditData(DeliveryHistoryDTO Data)
        {
            using(DBEntities db = new DBEntities())
            {

                TB_DELIVERY_HISTORY DeliveryHistory = db.TB_DELIVERY_HISTORY.FirstOrDefault(d => d.ID == Data.ID);

                //data PIC
                UserDTO DataPic = (UserDTO)HttpContext.Current.Session["UserLogin"];

                DeliveryHistory.CANDIDATE_POSITION = Data.CANDIDATE_POSITION;
                DeliveryHistory.SOURCE = Data.SOURCE;
                DeliveryHistory.CANDIDATE_STATE = Data.CANDIDATE_STATE;
                DeliveryHistory.LAST_PIC = DataPic.USER_ID;
                DeliveryHistory.PROCESS_DATE = db.TB_CANDIDATE_SELECTION_HISTORY.FirstOrDefault(d => d.CANDIDATE_ID == Data.CANDIDATE_ID && d.CANDIDATE_STATE == 1).PROCESS_DATE;
                DeliveryHistory.CLIENT_ID = Data.CLIENT_ID;
                DeliveryHistory.LAST_UPDATE = DateTime.Now;
                DeliveryHistory.CLIENT_STATE = Data.CLIENT_STATE;
                DeliveryHistory.NOTE = Data.NOTE;

                return db.SaveChanges();
            }
        }
    }
}