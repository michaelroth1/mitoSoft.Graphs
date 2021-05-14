using System;
using System.Diagnostics;

namespace mitoSoft.Graphs.Analysis
{
    [DebuggerDisplay(nameof(DirectedGraph) + " ({ToString()})")]
    public class DistanceNode : DirectedGraphNode
    {
        private double _distance;

        public DistanceNode(string name) : base(name)
        {
        }

        public DistanceNode(string name, string description) : base(name, description)
        {
        }

        public double Distance
        {
            get
            {
                return _distance;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Distance must be 0 or positive.");
                }

                _distance = value;
            }
        }
#pragma warning disable IDE0071 // Simplify interpolation
        public override string ToString() => $"{base.ToString()} (Distance from start: {this.Distance})";
#pragma warning restore IDE0071 // Simplify interpolation
    }
}