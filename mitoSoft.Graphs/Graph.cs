using mitoSoft.Graphs.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mitoSoft.Graphs
{
    [DebuggerDisplay(nameof(Graph) + " ({ToString()})")]
    public class Graph
    {
        private readonly Dictionary<string, GraphNode> _nodes;

        public Graph()
        {
            this._nodes = new Dictionary<string, GraphNode>();
        }

        public IEnumerable<GraphNode> Nodes => this._nodes.Values;

        public IEnumerable<GraphEdge> Edges => this.Nodes.SelectMany(n => n.Edges).Distinct();

        public bool TryGetNode(string nodeName, out GraphNode node) => this._nodes.TryGetValue(nodeName, out node);

        public virtual GraphNode GetNode(string nodeName)
        {
            if (this.TryGetNode(nodeName, out var node))
            {
                return node;
            }
            else
            {
                throw new NodeNotInGraphException(nodeName);
            }
        }

        public virtual Graph AddNode(string nodeName)
        {
            var node = new GraphNode(nodeName);

            this.AddNode(node);

            return this;
        }

        public virtual Graph AddNode(GraphNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            else if (!this.TryAddNode(ref node))
            {
                throw new InvalidOperationException($"Node with identical name {node.Name} already in graph.");
            }

            return this;
        }

        /// <summary>
        /// Tries to add the given node to the system. If the node already exists, nothing will be added.
        /// </summary>
        /// <param name="nodeName">Name of the node to be added</param>
        /// <returns>True when the node was actually added or false when a node with an identical name already exists.</returns>
        public virtual bool TryAddNode(string nodeName, out GraphNode node)
        {
            var newNode = new GraphNode(nodeName);

            var existing = this.TryAddNode(ref newNode);

            node = newNode;

            return existing;
        }

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

            if (this._nodes.TryGetValue(node.Name, out var existing))
            {
                node = existing;

                return false;
            }
            else
            {
                this._nodes.Add(node.Name, node);

                return true;
            }
        }

        public virtual bool TryGetEdge(string sourceNodeName, string targetNodeName, out GraphEdge edge)
        {
            edge = null;

            if (!this.TryGetNode(sourceNodeName, out var startNode))
            {
                return false;
            }

            if (!this._nodes.TryGetValue(targetNodeName, out var endNode))
            {
                return false;
            }

            return TryGetEdge(startNode, endNode, out edge);
        }

        public virtual GraphEdge GetEdge(string sourceNodeName, string targetNodeName)
        {
            if (this.TryGetEdge(sourceNodeName, targetNodeName, out var edge))
            {
                return edge;
            }
            else
            {
                throw new EdgeNotInGraphException(sourceNodeName, targetNodeName);
            }
        }

        public virtual GraphEdge GetEdge(GraphNode sourceNode, GraphNode targetNode)
        {
            if (this.TryGetEdge(sourceNode, targetNode, out var edge))
            {
                return edge;
            }
            else
            {
                throw new EdgeNotInGraphException(sourceNode, targetNode);
            }
        }

        public virtual bool TryGetEdge(GraphNode sourceName, GraphNode targetNode, out GraphEdge edge)
        {
            edge = sourceName.Edges.Where(e => ReferenceEquals(e.TargetNode, targetNode)).SingleOrDefault();

            return (edge != null);
        }
        
        public virtual Graph AddEdge(string sourceNodeName, string targetNodeName, double distance, bool twoWay)
        {
            if (!this.TryAddEdge(sourceNodeName, targetNodeName, distance, twoWay))
            {
                throw new InvalidOperationException($"Could not add edge between {sourceNodeName} and {targetNodeName}.");
            }

            return this;
        }

        public virtual bool TryAddEdge(string sourceNodeName, string targetNodeName, double distance, bool twoWay)
        {
            if (!this.TryGetNode(sourceNodeName, out var sourceNode))
            {
                sourceNode = new GraphNode(sourceNodeName);
            }

            if (!this.TryGetNode(targetNodeName, out var targetNode))
            {
                targetNode = new GraphNode(targetNodeName);
            }

            return TryAddEdge(sourceNode, targetNode, distance, twoWay);
        }

        public virtual Graph AddEdge(GraphNode sourceNode, GraphNode targetNode, double distance, bool twoWay)
        {
            if (!this.TryAddEdge(sourceNode, targetNode, distance, twoWay))
            {
                throw new InvalidOperationException($"Could not add edge between {sourceNode.Name} and {targetNode.Name}.");
            }

            return this;
        }

        public virtual bool TryAddEdge(GraphNode sourceNode, GraphNode targetNode, double distance, bool twoWay)
        {
            if (sourceNode == null)
            {
                throw new ArgumentNullException(nameof(sourceNode));
            }
            else if (targetNode == null)
            {
                throw new ArgumentNullException(nameof(targetNode));
            }
            else if (this.TryGetEdge(sourceNode, targetNode, out _))
            {
                return false;
            }

            this.TryAddNode(ref sourceNode);

            this.TryAddNode(ref targetNode);

            sourceNode.AddConnection(targetNode, distance, twoWay);

            return true;
        }

        public virtual void ClearAll()
        {
            this._nodes.Clear();
        }

        public override string ToString() => $"Nodes: {this._nodes.Count}";
    }
}