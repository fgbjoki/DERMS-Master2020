using CalculationEngine.Model.EnergyCalculations;
using CalculationEngine.TopologyAnalysis;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.Logger;
using System.Collections.Generic;

namespace CalculationEngine.EnergyCalculators
{
    public abstract class BaseTopologyCalculatingUnit<T> : ITopologyCalculatingUnit
        where T : CalculationObject
    {
        private IStorage<T> storage;

        private List<DMSType> dmsTypesToConsider;

        private ITopologyReader topologyReader;

        public BaseTopologyCalculatingUnit(List<DMSType> dmsTypes, IStorage<T> storage)
        {
            dmsTypesToConsider = dmsTypes;

            this.storage = storage;
        }

        public float Calculate(EnergyBalanceCalculation energyBalanceCalculation, IEnumerable<long> connectedNodesGids)
        {
            float totalSum = 0;

            foreach (var connectedNodeGid in connectedNodesGids)
            {
                DMSType nodeDMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(connectedNodeGid);

                if (!dmsTypesToConsider.Contains(nodeDMSType))
                {
                    continue;
                }

                T entity = storage.GetEntity(connectedNodeGid);

                if (entity == null)
                {
                    Logger.Instance.Log($"[{GetType().Name}] Error during calculating. Missing entity from storage with gid: {connectedNodeGid:X16}.");
                    continue;
                }

                AdditionalProcessing(energyBalanceCalculation, entity);

                float extractedValue = ExtractValueFromEntity(entity);

                totalSum += extractedValue;
            }

            return totalSum;
        }

        protected virtual void AdditionalProcessing(EnergyBalanceCalculation energyBalanceCalculation, T entity)
        {

        }

        protected virtual float ExtractValueFromEntity(T entity)
        {
            return entity.GetCalculation(CalculationType.ActivePower).Value;
        }

        public abstract void Recalculate(EnergyBalanceCalculation energyBalance, long conductingEquipmentGid, float delta);
    }
}
