using Common.AbstractModel;
using Common.DataTransferObjects.CalculationEngine;
using System.Collections.Generic;
using UIAdapter.Schema.Graph;

namespace UIAdapter.Schema
{
    public class DynamicSchemaToGraphSchemaConverter
    {
        public SchemaGraph Convert(SchemaGraphChanged changedGraph)
        {
            SchemaGraph newGraph = new SchemaGraph();

            Queue<long> nodesToProcess = new Queue<long>();

            PreprocessRoot(changedGraph.EnergySourceGid, nodesToProcess, newGraph);

            while (nodesToProcess.Count > 0)
            {
                long currentNode = nodesToProcess.Dequeue();
                SchemaGraphNode currentGraphNode = newGraph.GetNode(currentNode);

                ProcessChildren(currentGraphNode, newGraph, changedGraph.ParentChildBranches, nodesToProcess);
            }

            newGraph.MarkRoot(changedGraph.EnergySourceGid);
            newGraph.MarkInterConnectedBreaker(changedGraph.InterConnectedBreakerGid);

            return newGraph;
        }

        private void PreprocessRoot(long rootGid, Queue<long> nodesToProcess, SchemaGraph newGraph)
        {
            SchemaGraphNode root = new SchemaGraphNode(rootGid);
            root.IsEnergized = true;
            newGraph.AddNode(root);

            nodesToProcess.Enqueue(rootGid);
        }

        private void ProcessChildren(SchemaGraphNode parentNode, SchemaGraph newGraph, Dictionary<long, List<long>> childBranches, Queue<long> nodesToProcess)
        {
            foreach (var childGid in childBranches[parentNode.GlobalId])
            {
                SchemaGraphNode childNode = CreateNewNode(childGid, newGraph);

                ConnectNodes(parentNode, childNode);
                nodesToProcess.Enqueue(childNode.GlobalId);
            }
        }

        private void ConnectNodes(SchemaGraphNode parent, SchemaGraphNode child)
        {
            SchemaGraphBranch newBranch = new SchemaGraphBranch(parent, child);
            parent.ChildBranches.Add(newBranch);
            child.ParentBranch = newBranch;
        }

        private SchemaGraphNode CreateNewNode(long nodeId, SchemaGraph newGraph)
        {
            SchemaGraphNode newNode;
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(nodeId);

            if (dmsType == DMSType.BREAKER)
            {
                newNode = new SchemaBreakerGraphNode(nodeId);
                newGraph.AddBreaker(newNode as SchemaBreakerGraphNode);
            }
            else
            {
                newNode = new SchemaGraphNode(nodeId);
                newGraph.AddNode(newNode);
            }

            return newNode;
        }
    }
}
