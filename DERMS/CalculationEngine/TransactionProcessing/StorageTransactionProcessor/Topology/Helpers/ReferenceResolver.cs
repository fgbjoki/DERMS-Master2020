using CalculationEngine.Model.Topology.Transaction;
using System;
using System.Collections.Generic;

namespace CalculationEngine.TransactionProcessing.StorageTransactionProcessor.Topology
{
    public class ReferenceResolver : IDisposable
    {
        public ReferenceResolver()
        {
            Terminals = new Dictionary<long, Terminal>();
            ConnectivityNodes = new Dictionary<long, ConnectivityNode>();
            ConductingEquipments = new Dictionary<long, ConductingEquipment>();
        }
        public void Dispose()
        {
            Terminals.Clear();
            ConnectivityNodes.Clear();
            ConductingEquipments.Clear();

            Terminals = null;
            ConnectivityNodes = null;
            ConductingEquipments = null;
        }

        public bool AddReference(Terminal terminal)
        {
            if (!Terminals.ContainsKey(terminal.GlobalId))
            {
                Terminals.Add(terminal.GlobalId, terminal);
                return true;
            }

            return false;
        }

        public bool AddReference(ConnectivityNode connectivityNode)
        {
            if (!ConnectivityNodes.ContainsKey(connectivityNode.GlobalId))
            {
                ConnectivityNodes.Add(connectivityNode.GlobalId, connectivityNode);
                return true;
            }

            return false;
        }

        public bool AddReference(ConductingEquipment conductingEquipment)
        {
            if (!ConductingEquipments.ContainsKey(conductingEquipment.GlobalId))
            {
                ConductingEquipments.Add(conductingEquipment.GlobalId, conductingEquipment);
                return true;
            }

            return false;
        }


        public Terminal GetTerminal(long globalId)
        {
            Terminal terminal;
            Terminals.TryGetValue(globalId, out terminal);

            return terminal;
        }

        public ConnectivityNode GetConnectivityNode(long globalId)
        {
            ConnectivityNode connectivityNode;
            ConnectivityNodes.TryGetValue(globalId, out connectivityNode);

            return connectivityNode;
        }

        public ConductingEquipment GetConductingEquipment(long globalId)
        {
            ConductingEquipment conductingEquipment;
            ConductingEquipments.TryGetValue(globalId, out conductingEquipment);

            return conductingEquipment;
        }

        public Dictionary<long, Terminal> Terminals { get; private set; }
        public Dictionary<long, ConnectivityNode> ConnectivityNodes { get; private set; }
        public Dictionary<long, ConductingEquipment> ConductingEquipments { get; private set; }
    }
}
