using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace mitoSoft.Graphs
{
    [DebuggerDisplay(nameof(Graph) + " ({ToString()})")]
    public class Graph
    {
        private static ulong _nodeCounter = 0;

        private readonly Dictionary<GraphNodeKeyBase, GraphNode> _nodes;

        private readonly bool _twoWay;

        public Graph(bool twoWay)
        {
            this._nodes = new Dictionary<GraphNodeKeyBase, GraphNode>();
            this._twoWay = twoWay;
        }

        public IEnumerable<GraphNode> Nodes
        {
            get
            {
                foreach (var node in this._nodes.Values)
                {
                    yield return node;
                }
            }
        }

        public bool TryGetNode(GraphNodeKeyBase nodeKey, out GraphNode node) => this._nodes.TryGetValue(nodeKey, out node);

        /// <summary>
        /// Tries to add the given node to the system. If the node already exists, the existing node will be returned.
        /// </summary>
        /// <param name="node">The node to be added or upon return the existing node.</param>
        /// <returns>True when the node was actually added or false when an existing node is returned.</returns>
        public virtual bool TryAddNode(ref GraphNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (this._nodes.TryGetValue(node.Key, out var existing))
            {
                node = existing;

                return false;
            }
            else
            {
                this.DoAdd(node);

                return true;
            }
        }

        public virtual void AddNode(GraphNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            this.DoAdd(node);
        }

        public virtual void AddConnection(GraphNode sourceNode, GraphNode targetNode, double distance)
        {
            if (sourceNode == null)
            {
                throw new ArgumentNullException(nameof(sourceNode));
            }
            else if (targetNode == null)
            {
                throw new ArgumentNullException(nameof(targetNode));
            }
            else if (!_nodes.ContainsKey(sourceNode.Key))
            {
                throw new NodeNotInGraphException(sourceNode);
            }
            else if (!_nodes.ContainsKey(targetNode.Key))
            {
                throw new NodeNotInGraphException(targetNode);
            }

            sourceNode.AddConnection(targetNode, distance, this._twoWay);
        }

        public override string ToString() => $"Nodes: {this._nodes.Count}";

        private void DoAdd(GraphNode node)
        {
            this._nodes.Add(node.Key, node);

            node.ObjectNumber = ++_nodeCounter;
        }
    }
}