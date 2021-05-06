using ClientUI.Models.Schema.NodeCreators;
using Common.AbstractModel;
using Common.Logger;
using Common.UIDataTransferObject.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.Models.Schema
{
    public class SchemaCreator
    {
        private Dictionary<DMSType, SchemaNodeCreator> creators;
        private InterConnectedBreakerSchemaNodeCreator interConnectedBreakerNodeCreator;

        private NonClicableSchemaNodeCreator nonClicable;

        public SchemaCreator()
        {
            InitializeCreators();

            interConnectedBreakerNodeCreator = new InterConnectedBreakerSchemaNodeCreator();

            nonClicable = new NonClicableSchemaNodeCreator();
        }

        public SchemaGraphWrapper CreateSchema(SubSchemaDTO graphDto)
        {
            Dictionary<long, SchemaNode> nodes = new Dictionary<long, SchemaNode>();

            Queue<long> nodesToProcess = new Queue<long>();

            PreProcessRoot(graphDto.EnergySourceGid, graphDto, nodes, nodesToProcess);

            while (nodesToProcess.Count > 0)
            {
                long currentNodeGid = nodesToProcess.Dequeue();

                ProcessNode(currentNodeGid, graphDto, nodes, nodesToProcess);
            }

            SchemaGraphWrapper graphWrapper = new SchemaGraphWrapper(nodes[graphDto.EnergySourceGid], nodes, graphDto.InterConnectedBreaker);

            return graphWrapper;
        }

        private void ProcessNode(long currentNodeGid, SubSchemaDTO graphDto, Dictionary<long, SchemaNode> createdNodes, Queue<long> nodesToProcess)
        {
            SchemaNode currentNode = createdNodes[currentNodeGid];

            foreach (var childNodeGid in graphDto.ParentChildRelation[currentNodeGid])
            {
                SchemaNode childNode = CreateNewNode(childNodeGid, graphDto);

                currentNode.Children.Add(childNode);
                nodesToProcess.Enqueue(childNodeGid);
                createdNodes.Add(childNodeGid, childNode);
            }
        }

        private SchemaNode CreateNewNode(long currentNodeGid, SubSchemaDTO graphDto)
        {
            DMSType nodeDMSType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(currentNodeGid);
            SchemaNodeCreator schemaNodeCreator;

            if (IsInterConnectedBreaker(currentNodeGid, graphDto.InterConnectedBreaker))
            {
                schemaNodeCreator = interConnectedBreakerNodeCreator;
            }
            else if (!creators.TryGetValue(nodeDMSType, out schemaNodeCreator))
            {
                // TODO SHOW ON SCREEN AS WELL
                Logger.Instance.Log($"[{GetType()}] Cannot find schema node creator for DMSType: \'{nodeDMSType.ToString()}\'.");
            }

            return schemaNodeCreator.CreateNode(graphDto.ConductingEquipmentStates.Nodes[currentNodeGid]);
        }

        private bool IsInterConnectedBreaker(long interConnectedBreakerGid, long currentNodeGid)
        {
            return interConnectedBreakerGid == currentNodeGid;
        }

        private void PreProcessRoot(long rootGlobalId, SubSchemaDTO graphDto, Dictionary<long, SchemaNode> createdNodes, Queue<long> nodesToProcess)
        {
            SchemaNode rootNode = CreateNewNode(rootGlobalId, graphDto);

            nodesToProcess.Enqueue(rootGlobalId);
            createdNodes.Add(rootGlobalId, rootNode);
        }

        private void InitializeCreators()
        {
            creators = new Dictionary<DMSType, SchemaNodeCreator>()
            {
                { DMSType.BREAKER, new BreakerSchemaNodeCreator() },
                { DMSType.CONNECTIVITYNODE, new ConnectivityNodeSchemaNodeCreator() },
                { DMSType.ENERGYCONSUMER, /* TODO CHANGE THIS */ new NonClicableSchemaNodeCreator() },
                { DMSType.ENERGYSOURCE, /* TODO CHANGE THIS*/ new NonClicableSchemaNodeCreator() },
                { DMSType.ENERGYSTORAGE, new EnergyStorageSchemaNodeCreator() },
                { DMSType.WINDGENERATOR, new WindGeneratorSchemaNodeCreator() },
                { DMSType.SOLARGENERATOR, new SolarPanelSchemaNodeCreator() },
            };
        }
    }
}
