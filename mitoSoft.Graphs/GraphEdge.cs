using System;
using System.Diagnostics;

namespace mitoSoft.Graphs
{
    [DebuggerDisplay(nameof(GraphEdge) + " ({ToString()})")]
    public class GraphEdge
    {
        public GraphEdge(GraphNode sourceNode, GraphNode targetNode, double distance)
        {
            this.SourceNode = sourceNode ?? throw new ArgumentNullException(nameof(sourceNode));

            this.TargetNode = targetNode ?? throw new ArgumentNullException(nameof(targetNode));

            this.Distance = distance;
        }

        public GraphNode SourceNode { get; }

        public GraphNode TargetNode { get; }

        public double Distance { get; }

        public GraphEdgeKey Key => new GraphEdgeKey(SourceNode.Key, TargetNode.Key);

        internal ulong ObjectNumber { get; set; }

        public override string ToString() => $"{this.SourceNode.Name} -> {this.TargetNode.Name} (Distance: {this.Distance})";
    }
}
