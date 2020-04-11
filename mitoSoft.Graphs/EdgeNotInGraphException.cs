using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mitoSoft.Graphs.Exceptions
{
    public sealed class EdgeNotInGraphException : Exception
    {
        public EdgeNotInGraphException(GraphNode sourceNode, GraphNode targetNode) : base($"Edge between '{sourceNode.Name}' and '{targetNode.Name}' is not in graph.")
        {
        }

        public EdgeNotInGraphException(string sourceNodeName, string targetNodeName) : base($"Edge between '{sourceNodeName}' and '{targetNodeName}' is not in graph.")
        {
        }
    }
}
