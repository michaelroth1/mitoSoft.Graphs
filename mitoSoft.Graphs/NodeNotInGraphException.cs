using System;

namespace mitoSoft.Graphs
{
    public sealed class NodeNotInGraphException : Exception
    {
        public GraphNode Node { get; }

        public string NodeName { get; }

        public NodeNotInGraphException(GraphNode node) : base($"Node '{node.Name}' is not in graph.")
        {
            this.Node = node;
        }

        public NodeNotInGraphException(string name) : base($"Node '{name}' is not in graph.")
        {
            this.NodeName = name;
        }
    }
}
