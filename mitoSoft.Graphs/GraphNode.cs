using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mitoSoft.Graphs
{
    [DebuggerDisplay(nameof(GraphNode) + " ({ToString()})")]
    public class GraphNode
    {
        private readonly IList<GraphEdge> _edges;

        public GraphNode(string name)
        {
            this._edges = new List<GraphEdge>();

            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public IEnumerable<GraphEdge> Edges => this._edges;

        public IEnumerable<GraphNode> Predecessors => this._edges.Where(c => ReferenceEquals(c.TargetNode, this)).Select(c => c.SourceNode);

        public IEnumerable<GraphNode> Successors => this._edges.Where(c => ReferenceEquals(c.SourceNode, this)).Select(c => c.TargetNode);

        public override string ToString() => $"{this.Name} (Connections: {this._edges.Count})";

        internal void AddConnection(GraphNode targetNode, double distance, bool twoWay)
        {
            if (targetNode == null)
            {
                throw new ArgumentNullException(nameof(targetNode));
            }
            else if (distance <= 0)
            {
                throw new ArgumentException("Distance must be positive.");
            }

            var edge = new GraphEdge(this, targetNode, distance);

            this._edges.Add(edge);

            targetNode.AddConnection(edge);

            if (twoWay)
            {
                targetNode.AddConnection(this, distance, false);
            }
        }

        internal void AddConnection(GraphEdge edge)
        {
            if (!_edges.Contains(edge))
            {
                _edges.Add(edge);
            }
            else
            {
                throw new InvalidOperationException("Edge is already in collection.");
            }
        }
    }
}