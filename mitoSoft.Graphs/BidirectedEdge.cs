using System;
using System.Diagnostics;

namespace mitoSoft.Graphs
{
    [Serializable]
    [DebuggerDisplay(nameof(Node) + " ({ToString()})")]
    public class BidirectedEdge : DirectedEdge
    {
        public BidirectedEdge(DirectedGraphNode sourceNode, DirectedGraphNode targetNode, double weight) : base(sourceNode, targetNode, weight)
        {
        }

        public override string ToString() => $"{base.ToString().Replace(" -> ", " <-> ")})";
    }
}