using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;

namespace UIAdapter.NetworkModel.ConductingEquipment
{
    public class WindGeneratorDTOCreator : DistirbutedEnergyResouceDTOCreator<WindGeneratorDTO>
    {
        protected override WindGeneratorDTO InstantiateEntity()
        {
            return new WindGeneratorDTO();
        }

        protected override List<ModelCode> GetModelCodes()
        {
            var modelCodes = base.GetModelCodes();
            modelCodes.Add(ModelCode.WINDGENERATOR_STARTUPSPEED);
            modelCodes.Add(ModelCode.WINDGENERATOR_NOMINALSPEED);
            modelCodes.Add(ModelCode.WINDGENERATOR_CUTOUTSPEED);

            return modelCodes;
        }

        protected override void PopulateProperties(WindGeneratorDTO dto, ResourceDescription rd)
        {
            base.PopulateProperties(dto, rd);

            dto.CutOutSpeed = rd.GetProperty(ModelCode.WINDGENERATOR_CUTOUTSPEED).AsFloat();
            dto.NominalSpeed = rd.GetProperty(ModelCode.WINDGENERATOR_NOMINALSPEED).AsFloat();
            dto.StartUpSpeed = rd.GetProperty(ModelCode.WINDGENERATOR_STARTUPSPEED).AsFloat();
        }
    }
}
