using Common.AbstractModel;
using Common.GDA;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology
{
    public class TopologyOrderProcessingEnumerable : IEnumerable<KeyValuePair<DMSType, List<ResourceDescription>>>
    {
        private Dictionary<DMSType, List<ResourceDescription>> dictionary;
        private ModelResourcesDesc modelResourceDesc;
        private List<DMSType> dmsTypeOrder;

        public TopologyOrderProcessingEnumerable(ModelResourcesDesc modelResourceDesc)
        {
            this.modelResourceDesc = modelResourceDesc;

            InitializeDMSTypeOrder(modelResourceDesc);
        }

        public void PrepareEnumerator(Dictionary<DMSType,List<ResourceDescription>> dictionary)
        {
            this.dictionary = dictionary;
        }

        public IEnumerator<KeyValuePair<DMSType, List<ResourceDescription>>> GetEnumerator()
        {
            return new TopologyOrderProcessingEnumerator(dmsTypeOrder, dictionary);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TopologyOrderProcessingEnumerator(dmsTypeOrder, dictionary);
        }

        private void InitializeDMSTypeOrder(ModelResourcesDesc modelResourceDesc)
        {
            List<DMSType> generatorDMSTypes = modelResourceDesc.GetLeaves(ModelCode.GENERATOR);

            dmsTypeOrder = new List<DMSType>()
            {
                DMSType.CONNECTIVITYNODE,
                DMSType.TERMINAL,
            };

            dmsTypeOrder.AddRange(modelResourceDesc.GetLeaves(ModelCode.CONDUCTINGEQ).Except(generatorDMSTypes));

            dmsTypeOrder.AddRange(generatorDMSTypes);
        }
    }

    internal class TopologyOrderProcessingEnumerator : IEnumerator<KeyValuePair<DMSType, List<ResourceDescription>>>
    {
        private List<KeyValuePair<DMSType, List<ResourceDescription>>> order;
        private int currentIndex;

        public TopologyOrderProcessingEnumerator(List<DMSType> dmsTypeOrder, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            if (!affectedEntities.ContainsKey(DMSType.TERMINAL) || !affectedEntities.ContainsKey(DMSType.CONNECTIVITYNODE))
            {
                return;
            }

            InitializeOrderProcessing(dmsTypeOrder, affectedEntities);

            currentIndex = -1;
        }

        private void InitializeOrderProcessing(List<DMSType> dmsTypeOrder, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            order = new List<KeyValuePair<DMSType, List<ResourceDescription>>>();

            List<ResourceDescription> rds;
            foreach (DMSType dmsType in dmsTypeOrder)
            {
                if (affectedEntities.TryGetValue(dmsType, out rds))
                {
                    order.Add(new KeyValuePair<DMSType, List<ResourceDescription>>(dmsType, rds));
                }
            }
        }

        public KeyValuePair<DMSType, List<ResourceDescription>> Current
        {
            get
            {
                return order[currentIndex];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return order[currentIndex];
            }
        }

        public void Dispose()
        {
            order.Clear();
            order = null;
        }

        public bool MoveNext()
        {
            if (order == null)
            {
                return false;
            }

            if (order.Count - 1 == currentIndex)
            {
                return false;
            }

            ++currentIndex;
            return true;
        }

        public void Reset()
        {
            currentIndex = -1;
        }
    }
}
