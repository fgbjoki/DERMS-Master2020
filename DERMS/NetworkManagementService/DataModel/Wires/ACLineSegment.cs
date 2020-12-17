namespace NetworkManagementService.DataModel.Wires
{
    public class ACLineSegment : Conductor
    {
        public ACLineSegment(long globalId) : base(globalId)
        {

        }

        protected ACLineSegment(ACLineSegment copyObject) : base(copyObject)
        {

        }
    }
}
