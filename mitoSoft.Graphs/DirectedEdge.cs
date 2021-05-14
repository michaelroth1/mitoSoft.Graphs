using System;

namespace mitoSoft.Graphs
{
    public class DirectedEdge : Edge
    {
        public DirectedEdge(DirectedGraphNode sourceNode, DirectedGraphNode targetNode, double weight) : base(sourceNode, targetNode)
        {
            if (weight <= 0.0)
            {
                throw new ArgumentException("Weight must be positive.");
            }
            this.Weight = weight;
        }

        public new DirectedGraphNode Source => (DirectedGraphNode)base.Source;

        public new DirectedGraphNode Target => (DirectedGraphNode)base.Target;

        public double Weight { get; }

        public override string ToString() => $"{base.ToString()} (Weight: {this.Weight})";
    }
}