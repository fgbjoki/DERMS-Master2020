using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.DataTransferObjects.CalculationEngine.DEROptimalCommanding
{
    [DataContract]
    public class DEROptimalCommandingFeedbackDTO
    {
        public DEROptimalCommandingFeedbackDTO()
        {
            Result = new List<DERUnitFeedbackDTO>();
        }

        /// <summary>
        /// Item1 = Storage gid, Item2 = ActivePower
        /// </summary>
        [DataMember]
        public List<DERUnitFeedbackDTO> Result { get; set; }

        [DataMember]
        public bool ValidCommanding { get; set; }
    }
}
