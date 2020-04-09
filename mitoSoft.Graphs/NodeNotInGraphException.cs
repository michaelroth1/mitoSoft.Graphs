using System;

namespace mitoSoft.Graphs
{
    public sealed class NodeNotInGraphException : Exception
    {
        public GraphNode Node { get; }

        public GraphNodeKeyBase Key { get; }

        public NodeNotInGraphException(GraphNode node) : base($"Node '{node.Name}' is not in graph.")
        {
            this.Node = node;
        }

        public NodeNotInGraphException(GraphNodeKeyBase key) : base($"Node '{key.GetKeyDisplayValue()}' is not in graph.")
        {
            this.Key = key;
        }
    }
}
