using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mitoSoft.Graphs
{
    [DebuggerDisplay(nameof(GraphNode) + " ({ToString()})")]
    public class GraphNode
    {
        private static ulong _edgeCounter = 0;

        private readonly Dictionary<GraphEdgeKey, GraphEdge> _connections;

        public GraphNode(string name, GraphNodeKeyBase key)
        {
            this._connections = new Dictionary<GraphEdgeKey, GraphEdge>();

            this.Name = name ?? throw new ArgumentNullException(nameof(name));

            this.Key = key;
        }

        public string Name { get; }

        public GraphNodeKeyBase Key { get; }

        public IEnumerable<GraphEdge> Connections
        {
            get
            {
                foreach (var connection in this._connections.Values)
                {
                    yield return connection;
                }
            }
        }

        public IEnumerable<GraphNode> Predecessors => this._connections.Values.Where(c => ReferenceEquals(c.TargetNode, this)).Select(c => c.SourceNode);

        public IEnumerable<GraphNode> Successors => this._connections.Values.Where(c => ReferenceEquals(c.SourceNode, this)).Select(c => c.TargetNode);

        internal ulong ObjectNumber { get; set; }

        public override string ToString() => $"{this.Name} (Connections: {this._connections.Count})";

        internal void AddConnection(GraphNode targetNode, double distance, bool twoWay)
        {
            if (targetNode == null)
            {
                throw new ArgumentNullException(nameof(targetNode));
            }
            else if (targetNode.Key.KeysAreEqual(this.Key))
            {
                throw new ArgumentException("Node must not connect to itself.");
            }
            else if (distance <= 0)
            {
                throw new ArgumentException("Distance must be positive.");
            }

            var forwardKey = new GraphEdgeKey(this.Key, targetNode.Key);

            var backwardKey = new GraphEdgeKey(targetNode.Key, this.Key);

            var forwardEdge = this.AddConnection(forwardKey, this, targetNode, distance);

            var backwardEdge = targetNode.AddConnection(backwardKey, targetNode, this, distance);

            if (twoWay)
            {
                this.AddReverseConnection(backwardKey, backwardEdge, distance);

                targetNode.AddReverseConnection(forwardKey, forwardEdge, distance);
            }
        }

        private GraphEdge AddConnection(GraphEdgeKey edgeKey, GraphNode sourceNode, GraphNode targetNode, double distance)
        {
            if (!_connections.TryGetValue(edgeKey, out var existingEdge))
            {
                existingEdge = new GraphEdge(sourceNode, targetNode, distance)
                {
                    ObjectNumber = ++_edgeCounter,
                };

                _connections.Add(edgeKey, existingEdge);
            }
            else if (existingEdge.Distance != distance)
            {
                throw new InvalidOperationException("Cannot change distance for existing connection.");
            }

            return existingEdge;
        }

        private void AddReverseConnection(GraphEdgeKey edgeKey, GraphEdge newEdge, double distance)
        {
            if (!_connections.TryGetValue(edgeKey, out var existingEdge))
            {
                _connections.Add(edgeKey, newEdge);
            }
            else if (existingEdge.Distance != distance)
            {
                throw new InvalidOperationException("Cannot change distance for existing connection.");
            }
        }
    }
}