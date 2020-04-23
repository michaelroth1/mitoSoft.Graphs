using System;

namespace mitoSoft.Graphs.Analysis.Exceptions
{
    [Serializable]
    public sealed class PathNotFoundException : Exception
    {
        public PathNotFoundException(string sourceNodeName, string targetNodeName) : base($"No Path found between '{sourceNodeName}' and '{targetNodeName}'.")
        {
        }
    }
}