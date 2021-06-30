using System.Runtime.Serialization;

namespace NetworkManagementService.DataModel.DER_Specific
{
    [DataContract]
    public class SolarGenerator : Generator
    {
        public SolarGenerator(long globalId) : base(globalId)
        {

        }

        protected SolarGenerator(SolarGenerator copyObject) : base(copyObject) 
        {

        }
    }
}
