using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;
using Common.UIDataTransferObject.NetworkModel;

namespace UIAdapter.NetworkModel.Measurement
{
    public class AnalogMeasurementDTOCreator : MeasurementDTOCreator<AnalogMeasurementDTO>
    {
        protected override void PopulateProperties(AnalogMeasurementDTO dto, ResourceDescription rd)
        {
            base.PopulateProperties(dto, rd);

            dto.MaxValue = rd.GetProperty(ModelCode.MEASUREMENTANALOG_MAXVALUE).AsFloat();
            dto.MinValue = rd.GetProperty(ModelCode.MEASUREMENTANALOG_MINVALUE).AsFloat();
        }

        protected override List<ModelCode> GetModelCodes()
        {
            var modelCodes = base.GetModelCodes();
            modelCodes.Add(ModelCode.MEASUREMENTANALOG_MAXVALUE);
            modelCodes.Add(ModelCode.MEASUREMENTANALOG_MINVALUE);

            return modelCodes;
        }

        protected override AnalogMeasurementDTO InstantiateEntity()
        {
            return new AnalogMeasurementDTO();
        }
    }
}
