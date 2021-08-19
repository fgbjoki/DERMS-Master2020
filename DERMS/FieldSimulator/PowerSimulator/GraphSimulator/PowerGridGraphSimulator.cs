using FieldSimulator.PowerSimulator.Model.Graph;
using FieldSimulator.PowerSimulator.Model.Graph.GraphTraverser;
using FieldSimulator.PowerSimulator.Model.Graph.TopologyGraphCreator;
using FieldSimulator.PowerSimulator.Storage;
using FieldSimulator.PowerSimulator.Storage.Weather;
using System.Timers;

namespace FieldSimulator.PowerSimulator.GraphSimulator
{
    public class PowerGridGraphSimulator
    {
        private TopologyGraph topologyGraph;
        private GraphCreatorWrapper graphCreator;
        private PowerGridSimulatorStorage storage;

        private TopologyGraphNodeCalculationInjector calculationInjector;

        private Timer simulationTimer;

        private StorageLock locker;

        private ITopologyNodeCalculationSimulator nodeSimulator;
        private double simulationInterval;

        public PowerGridGraphSimulator(PowerGridSimulatorStorage storage, double simulationInterval)
        {
            this.storage = storage;
            this.simulationInterval = simulationInterval;

            graphCreator = new GraphCreatorWrapper();
            nodeSimulator = new TopologyNodeCalculationSimulator();
            calculationInjector = new TopologyGraphNodeCalculationInjector();

            simulationTimer = new Timer();
            simulationTimer.Elapsed += Simulate;
            simulationTimer.AutoReset = true;
            simulationTimer.Enabled = false;
            simulationTimer.Interval = simulationInterval * 1000;
        }

        public void CreateGraphs(EntityStorage entityStorage)
        {
            topologyGraph = graphCreator.CreateGraph(entityStorage);
            locker = topologyGraph.StorageReaderLock;

            foreach (var node in topologyGraph.GetAllNodes())
            {
                calculationInjector.InjectCalculation(entityStorage, node);
            }
        }

        public void LoadBreakerBranches(BreakerTopologyManipulation breakerManipulation)
        {
            breakerManipulation.LoadTopologyGraph(topologyGraph);
        }

        public void Start()
        {
            simulationTimer.Enabled = true;
        }

        public void Stop()
        {
            simulationTimer.Enabled = false;
        }

        private void Simulate(object sender, ElapsedEventArgs e)
        {
            simulationTimer.Enabled = false;
            try
            {
                foreach (var root in topologyGraph.GetRoots())
                {
                    nodeSimulator.Simulate(root, storage, simulationInterval);
                }
            }
            finally
            {
                simulationTimer.Enabled = true;
            }
        }

    }
}
