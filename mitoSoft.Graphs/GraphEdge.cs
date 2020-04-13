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

            this.Weight = distance;

            this.Description = string.Empty;
        }

        public GraphNode SourceNode { get; }

        public GraphNode TargetNode { get; }

        public double Weight { get; }

        public string Description { get; set; }

        public string Name => $"{this.SourceNode.Name} -> {this.TargetNode.Name}";

        public override string ToString() => $"{this.SourceNode.Name} -> {this.TargetNode.Name} (Weight: {this.Weight})";
    }
}
