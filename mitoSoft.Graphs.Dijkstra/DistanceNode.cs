using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mitoSoft.Graphs.ShortestPathAlgorithms
{
    [DebuggerDisplay(nameof(DistanceGraph) + " ({ToString()})")]
    public class DistanceNode : GraphNode
    {
        public DistanceNode(string name) : base(name)
        {
            ResetDistanceFromStart();
        }

        public double DistanceFromStart { get; private set; }

        public void SetDistanceFromStart(double distance)
        {
            if (distance < 0)
            {
                throw new ArgumentException("Distance must be 0 or positive.");
            }

            this.DistanceFromStart = distance;
        }

        public void ResetDistanceFromStart()
        {
            this.DistanceFromStart = double.PositiveInfinity;
        }

        /// <summary>
        /// The shortest path predecessors are all nodes which have the smallest sum of predecessorNode.DistanceFromStart + connectionToThisNode.Distance.
        /// </summary>
        public IEnumerable<DistanceNode> GetShortestPathPredecessors()
        {
            var predecessors = Predecessors.Cast<DistanceNode>().ToList();

            if (predecessors.Count > 0)
            {
                var min = predecessors.Min(GetStartDistanceToMe);

                var result = predecessors.Where(p => GetStartDistanceToMe(p) == min);

                return result;
            }
            else
            {
                return Enumerable.Empty<DistanceNode>();
            }
        }

        private double GetStartDistanceToMe(DistanceNode predecessor)
        {
            var predecessorDistanceFromStart = predecessor.DistanceFromStart;

            var predecessorDistanceToMe = predecessor.Edges.First(c => ReferenceEquals(c.TargetNode, this)).Distance;

            var startDistanceToMe = predecessorDistanceFromStart + predecessorDistanceToMe;

            return startDistanceToMe;
        }

#pragma warning disable IDE0071 // Simplify interpolation
        public override string ToString() => $"{base.ToString()} (Distance from start: {this.DistanceFromStart})";
#pragma warning restore IDE0071 // Simplify interpolation

    }
}