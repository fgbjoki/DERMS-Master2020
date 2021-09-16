using Common.AbstractModel;
using Common.ComponentStorage;
using Common.DataTransferObjects.CalculationEngine.EnergyBalanceForecast;
using Common.UIDataTransferObject.EnergyBalanceForecast;
using UIAdapter.Model.NetworkModel;

namespace UIAdapter.Forecast.EnergyBalanceForecast
{
    public class DataConverter
    {
        public DERStatesSuggestionDTO ConvertData(DERStateCommandingSequenceDTO request, DomainParametersDTO domainParameters, IStorage<NetworkModelItem> storage)
        {
            DERStatesSuggestionDTO convertedData = new DERStatesSuggestionDTO();

            convertedData.ImportedEnergy = request.ImportedEnergy;
            convertedData.CostOfEnergyUse.CostOfEnergyImport = convertedData.ImportedEnergy * domainParameters.CostOfImportedEnergyPerKWH;

            foreach (var derState in request.SuggestedDTOState)
            {
                Common.UIDataTransferObject.EnergyBalanceForecast.DERStateDTO newDtoState = new Common.UIDataTransferObject.EnergyBalanceForecast.DERStateDTO();

                DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(derState.GlobalId);
                if (dmsType == DMSType.ENERGYSTORAGE)
                {
                    ProcessEnergyStorage(derState, newDtoState, domainParameters, storage);
                    convertedData.CostOfEnergyUse.CostOfEnergyStorageUse += newDtoState.Cost;
                }
                else if (dmsType == DMSType.WINDGENERATOR || dmsType == DMSType.SOLARGENERATOR)
                {
                    ProcessGenerator(derState, newDtoState, domainParameters, storage);
                    convertedData.CostOfEnergyUse.CostOfGeneratorShutDown += newDtoState.Cost;
                }

                convertedData.DERStates.Add(newDtoState);
            }

            return convertedData;
        }

        private void ProcessGenerator(Common.DataTransferObjects.CalculationEngine.EnergyBalanceForecast.DERStateDTO derState, Common.UIDataTransferObject.EnergyBalanceForecast.DERStateDTO newDtoState, DomainParametersDTO domainParameters, IStorage<NetworkModelItem> storage)
        {
            ProcessEntity(derState, newDtoState, storage);

            newDtoState.EnergyUsed = derState.ActivePower / (60 / domainParameters.SimulationInterval);
            newDtoState.Cost = derState.ActivePower > 0 && derState.IsEnergized ? 0 : newDtoState.EnergyUsed * domainParameters.CostOfGeneratorShutdownPerKWH;
        }

        private void ProcessEnergyStorage(Common.DataTransferObjects.CalculationEngine.EnergyBalanceForecast.DERStateDTO derState, Common.UIDataTransferObject.EnergyBalanceForecast.DERStateDTO newDtoState, DomainParametersDTO domainParameters, IStorage<NetworkModelItem> storage)
        {
            ProcessEntity(derState, newDtoState, storage);

            newDtoState.EnergyUsed = derState.ActivePower / (60 / domainParameters.SimulationInterval);
            newDtoState.Cost = derState.ActivePower > 0 ? newDtoState.EnergyUsed * domainParameters.CostOfEnergyStorageUsePerKWH : 0; 
        }

        private void ProcessEntity(Common.DataTransferObjects.CalculationEngine.EnergyBalanceForecast.DERStateDTO derState, Common.UIDataTransferObject.EnergyBalanceForecast.DERStateDTO newDtoState, IStorage<NetworkModelItem> storage)
        {
            newDtoState.GlobalId = derState.GlobalId;
            newDtoState.IsEnergized = derState.IsEnergized;

            newDtoState.Name = storage.GetEntity(newDtoState.GlobalId).Name;
        }
    }
}
