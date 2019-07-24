using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;
using FinalProject.Utils;

namespace FinalProject.DTO
{
    public class DetailCandidateDTO
    {
        public CandidateDTO DataCandidate { get; set; }
        public List<CandidateJobExperienceDTO> DataJobExp { get; set; }
        public List<CandidateSelectionHistoryDTO> DataSelectionHistory { get; set; }
        public List<DeliveryHistoryDTO> DataDeliveryHistory { get; set; }
    }

    //----------------------------------------------------- manage data -----------------------------------------------

    public class Manage_DetailCandidate { 
    
        public static DetailCandidateDTO GetData(int id)
        {
            return new DetailCandidateDTO
            {
                DataCandidate = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == id),
                DataJobExp = Manage_CandidateJobExperienceDTO.GetData().Where(d => d.CANDIDATE_ID == id).ToList(),
                DataDeliveryHistory = Manage_DeliveryHistoryDTO.GetData().Where(d => d.CANDIDATE_ID == id).ToList(),
                DataSelectionHistory = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d => d.CANDIDATE_ID == id).ToList()
            };
        }
    }
}