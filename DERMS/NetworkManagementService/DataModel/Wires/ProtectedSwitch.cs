namespace NetworkManagementService.DataModel.Wires
{
    public class ProtectedSwitch : Switch
    {
        public ProtectedSwitch(long globalId) : base(globalId)
        {

        }

        protected ProtectedSwitch(ProtectedSwitch copyObject) : base(copyObject)
        {

        }
    }
}
