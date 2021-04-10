using CalculationEngine.Model.EnergyCalculations;
using CalculationEngine.TopologyAnalysis;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.Logger;
using System.Collections.Generic;
using System;

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

        public float Calculate(long sourceGid, IEnumerable<long> connectedNodesGids)
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

                float extractedValue = ExtractValueFromEntity(entity);

                totalSum += extractedValue;
            }

            return totalSum;
        }

        private float ExtractValueFromEntity(T entity)
        {
            return entity.GetCalculation(CalculationType.ActivePower).Value;
        }

        public float Recalculate(float calculatedValue, long modifiedEntityGid, float newEntityValue)
        {
            T entity = storage.GetEntity(modifiedEntityGid);

            if (entity == null)
            {
                Logger.Instance.Log($"[{GetType().Name}] Error during calculating. Missing entity from storage with gid: {modifiedEntityGid:X16}.");
                return 0;
            }

            float deltaValue = 0;

            CalculationWrapper calculation = entity.GetCalculation(CalculationType.ActivePower);
            deltaValue = newEntityValue - calculation.Value;

            calculation.Value = newEntityValue;

            return calculatedValue + deltaValue;
        }
    }
}
