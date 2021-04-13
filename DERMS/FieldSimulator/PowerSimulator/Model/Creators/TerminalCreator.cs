using FieldSimulator.PowerSimulator.Model.Connectivity;
using Common.AbstractModel;
using FTN.ESI.SIMES.CIM.CIMAdapter.Importer;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public class TerminalCreator : BaseCreator<DERMS.Terminal, Terminal>
    {
        public TerminalCreator(ImportHelper importHelper) : base(DMSType.TERMINAL, importHelper)
        {
        }

        protected override Terminal InstantiateNewEntity(long globalId)
        {
            return new Terminal(globalId);
        }
    }
}
