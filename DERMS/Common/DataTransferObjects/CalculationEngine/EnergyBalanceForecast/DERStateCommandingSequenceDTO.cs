using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.DataTransferObjects.CalculationEngine.EnergyBalanceForecast
{
    [DataContract]
    public class DERStateCommandingSequenceDTO
    {
        public DERStateCommandingSequenceDTO()
        {
            SuggestedDTOState = new List<DERStateDTO>();
        }

        [DataMember]
        public List<DERStateDTO> SuggestedDTOState { get; set; }

        [DataMember]
        public float ImportedEnergy { get; set; }

        [DataMember]
        public bool Error { get; set; }
    }
}
