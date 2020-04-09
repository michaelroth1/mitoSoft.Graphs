using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mitoSoft.Graphs
{
    [DebuggerDisplay(nameof(GraphNode) + " ({ToString()})")]
    public class GraphNode
    {
        private readonly IList<GraphEdge> _connections;

        public GraphNode(string name)
        {
            this._connections = new List<GraphEdge>();

            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public IEnumerable<GraphEdge> Connections => this._connections;

        public IEnumerable<GraphNode> Predecessors => this._connections.Where(c => ReferenceEquals(c.TargetNode, this)).Select(c => c.SourceNode);

        public IEnumerable<GraphNode> Successors => this._connections.Where(c => ReferenceEquals(c.SourceNode, this)).Select(c => c.TargetNode);

        public override string ToString() => $"{this.Name} (Connections: {this._connections.Count})";

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

            this._connections.Add(edge);

            targetNode.AddConnection(edge);

            if (twoWay)
            {
                targetNode.AddConnection(this, distance, false);
            }
        }

        //private GraphEdge AddConnection(GraphNode sourceNode, GraphNode targetNode, double distance)
        //{
        //    if (!_connections.TryGetValue(edgeKey, out var existingEdge))
        //    {
        //        _connections.Add(edgeKey, existingEdge);
        //    }
        //    else if (existingEdge.Distance != distance)
        //    {
        //        throw new InvalidOperationException("Cannot change distance for existing connection.");
        //    }

        //    return existingEdge;
        //}

        internal void AddConnection(GraphEdge edge)
        {
            if (!_connections.Contains(edge))
            {
                _connections.Add(edge);
            }
            else
            {
                throw new InvalidOperationException("Edge is already in collection.");
            }
        }
    }
}