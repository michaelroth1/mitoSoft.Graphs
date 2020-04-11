using System;

namespace mitoSoft.Graphs.Exceptions
{
    public sealed class NodeNotInGraphException : Exception
    {
        public NodeNotInGraphException(GraphNode node) : base($"Node '{node.Name}' is not in graph.")
        {
        }

        public NodeNotInGraphException(string name) : base($"Node '{name}' is not in graph.")
        {
        }
    }
}