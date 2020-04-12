using System;

namespace mitoSoft.Graphs.ShortestPathAlgorithms.Exceptions
{
    [Serializable]
    public sealed class PathNotFoundException : Exception
    {
        public PathNotFoundException(string sourceNodeName, string targetNodeName) : base($"No Path found between '{sourceNodeName}' and '{targetNodeName}'.")
        {
        }
    }
}