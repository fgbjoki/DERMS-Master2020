﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace CalculationEngine.Graphs.GraphProcessors
{
    public abstract class BaseGraphProcessor<GraphType> : IGraphProcessor<GraphType>
    {
        /// <summary>
        /// Key: globalId of root, Value: roots graph
        /// </summary>
        protected Dictionary<long, GraphType> graphs;

        protected BaseGraphProcessor()
        {
            graphs = new Dictionary<long, GraphType>();
        }

        public virtual AutoResetEvent AlignEvent
        {
            set
            {
                
            }
        }

        public virtual bool AddGraph(GraphType graph)
        {
            foreach (var rootGlobalId in GetRootsGlobalId(graph))
            {
                if (graphs.ContainsKey(rootGlobalId))
                {
                    return false;
                }
            }

            foreach (var rootGlobalId in GetRootsGlobalId(graph))
            {
                graphs.Add(rootGlobalId, graph);
            }

            return true;
        }

        protected abstract IEnumerable<long> GetRootsGlobalId(GraphType graph);

    }
}
