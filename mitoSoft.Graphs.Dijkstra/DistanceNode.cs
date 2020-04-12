using System;
using System.Diagnostics;

namespace mitoSoft.Graphs.ShortestPathAlgorithms
{
    [DebuggerDisplay(nameof(Graph) + " ({ToString()})")]
    public class DistanceNode : GraphNode
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
        public override string ToString() => $"{base.ToString()} (Distance from start: {this.Distance})";
    }
}