using System;
using System.Collections.Generic;

namespace mitoSoft.Graphs.Exceptions
{
    public sealed class NodeNotFoundException : KeyNotFoundException
    {
        public NodeNotFoundException(string name) : base($"Node '{name}' is not in graph.")
        {
        }
    }

    public sealed class EdgeNotFoundException : KeyNotFoundException
    {
        public EdgeNotFoundException(string sourceNodeName, string targetNodeName) : base($"Edge between '{sourceNodeName}' and '{targetNodeName}' is not in graph.")
        {
        }
    }

    public sealed class NodeAlreadyExistingException : ArgumentException
    {
        public NodeAlreadyExistingException(string name) : base($"A node with the same name: '{name}' has already been added to the graph.")
        {
        }
    }

    public sealed class EdgeAlreadyExistingException : ArgumentException
    {
        public EdgeAlreadyExistingException(string sourceNodeName, string targetNodeName) : base($"A edge between the '{sourceNodeName}' and '{targetNodeName}'  has already been added to the graph.")
        {
        }
    }
}