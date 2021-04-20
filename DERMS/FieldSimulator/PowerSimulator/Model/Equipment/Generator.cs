namespace FieldSimulator.PowerSimulator.Model.Equipment
{
    public class Generator : ConductingEquipment
    {
        public Generator(long globalId) : base(globalId)
        {
        }

        public float NominalPower { get; set; }

        public override void Update(DERMS.IdentifiedObject cimObject)
        {
            base.Update(cimObject);

            DERMS.Generator cimGenerator = cimObject as DERMS.Generator;

            if (cimGenerator == null)
            {
                return;
            }

            NominalPower = cimGenerator.NominalPower;
        }
    }
}
