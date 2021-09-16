using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.UIDataTransferObject.EnergyBalanceForecast
{
    [DataContract]
    public class DERStatesSuggestionDTO
    {
        public DERStatesSuggestionDTO()
        {
            DERStates = new List<DERStateDTO>();
            CostOfEnergyUse = new CostOfEnergyUseDTO();
        }

        [DataMember]
        public List<DERStateDTO> DERStates { get; set; }

        [DataMember]
        public CostOfEnergyUseDTO CostOfEnergyUse { get; set; }

        [DataMember]
        public float ImportedEnergy { get; set; }

        [DataMember]
        public bool Error { get; set; }
    }
}
