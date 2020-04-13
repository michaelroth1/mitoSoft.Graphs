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
        private readonly Dictionary<string, GraphNode> _nodes = new Dictionary<string, GraphNode>();

        /// <summary>
        /// Returns all nodes of the graph
        /// </summary>
        public IEnumerable<GraphNode> Nodes => this._nodes.Values;

        /// <summary>
        /// Returns all edges of the graph
        /// </summary>
        public IEnumerable<GraphEdge> Edges => this.Nodes.SelectMany(n => n.Edges).Distinct();

        /// <summary>
        /// Tries to returns the node with the given name to the graph.
        /// </summary>
        /// <param name="node">Name of the node to be returned</param>
        /// <returns>True when the node was found or false when the node does not exist.</returns>
        public bool TryGetNode(string nodeName, out GraphNode node) => this._nodes.TryGetValue(nodeName, out node);

        /// <summary>
        /// Returns the node with the given name to the graph.
        /// </summary>
        /// <param name="node">Name of the node to be returned</param>
        /// <exception cref="NodeNotFoundException">If the node could not been found.</exception>
        public virtual GraphNode GetNode(string nodeName)
        {
            if (this.TryGetNode(nodeName, out var node))
            {
                return node;
            }
            else
            {
                throw new NodeNotFoundException(nodeName);
            }
        }

        /// <summary>
        /// Tries to add a node with the given name to the graph.
        /// </summary>
        /// <param name="node">Name of the node to be added</param>
        /// <exception cref="NodeAlreadyExistingException">If a node with an identical key has already been added.</exception>
        public virtual Graph AddNode(string nodeName)
        {
            var node = new GraphNode(nodeName);

            this.AddNode(node);

            return this;
        }

        /// <summary>
        /// Tries to add the given node to the graph.
        /// </summary>
        /// <param name="node">Node to be added</param>
        /// <exception cref="NodeAlreadyExistingException">If a node with an identical key has already been added.</exception>
        public virtual Graph AddNode(GraphNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            else if (!this.TryAddNode(node))
            {
                throw new NodeAlreadyExistingException(node.Name);
            }

            return this;
        }

        /// <summary>
        /// Tries to add the given node to the graph. If the node already exists, nothing will be added.
        /// </summary>
        /// <param name="nodeName">Name of the node to be added</param>
        /// <returns>True when the node was actually added or false when a node with an identical name already exists.</returns>
        public virtual bool TryAddNode(string nodeName, out GraphNode node)
        {
            var newNode = new GraphNode(nodeName);

            if (TryAddNode(newNode))
            {
                node = this._nodes[nodeName];
                return true;
            }
            else
            {
                node = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to add the given node to the graph.
        /// </summary>
        /// <param name="node">The node to be added.</param>
        /// <returns>True when the node was actually added or false when an existing node is returned.</returns>
        public virtual bool TryAddNode(GraphNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (!this._nodes.ContainsKey(node.Name))
            {
                this._nodes.Add(node.Name, node);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to return the edge that connects the sourceNode, given bv the 'sourceNodeName',
        /// and the targetNode, given by the 'targetNodeName'.
        /// </summary>
        /// <returns>True when the edge was actually added or false when an existing edge already exists.</returns>
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

        /// <summary>
        /// Tries to return the edge, that connects the 'sourceNode' and the 'targetNode'.
        /// </summary>
        /// <returns>True when the edge was actually added or false when an existing edge already exists.</returns>
        public virtual bool TryGetEdge(GraphNode sourceNode, GraphNode targetNode, out GraphEdge edge)
        {
            edge = sourceNode.Edges.Where(e => ReferenceEquals(e.TargetNode, targetNode) || e is BidirectionalEdge && ReferenceEquals(e.SourceNode, targetNode)).SingleOrDefault();

            return (edge != null);
        }

        /// <summary>
        /// Return the edge that connects the sourceNode, given bv the 'sourceNodeName',
        /// and the targetNode, given by the 'targetNodeName'.
        /// </summary>
        public virtual GraphEdge GetEdge(string sourceNodeName, string targetNodeName)
        {
            if (this.TryGetEdge(sourceNodeName, targetNodeName, out var edge))
            {
                return edge;
            }
            else
            {
                throw new EdgeNotFoundException(sourceNodeName, targetNodeName);
            }
        }

        /// <summary>
        /// Return the edge that connects the 'sourceNode' and the 'targetNode'.
        /// </summary>
        public virtual GraphEdge GetEdge(GraphNode sourceNode, GraphNode targetNode)
        {
            if (this.TryGetEdge(sourceNode, targetNode, out var edge))
            {
                return edge;
            }
            else
            {
                throw new EdgeNotFoundException(sourceNode.Name, targetNode.Name);
            }
        }

        /// <summary>
        /// Add an edge that connects the 'sourceNode' and the 'targetNode'.
        /// </summary>
        public virtual Graph AddEdge(GraphNode sourceNode, GraphNode targetNode, double weight, bool bidirection)
        {
            if (!this.TryAddEdge(sourceNode, targetNode, weight, bidirection))
            {
                throw new EdgeAlreadyExistingException(sourceNode.Name, targetNode.Name);
            }

            return this;
        }

        /// <summary>
        /// Add an edge that connects the sourceNode, given bv the 'sourceNodeName',
        /// and the targetNode, given by the 'targetNodeName'.
        /// </summary>
        public virtual Graph AddEdge(string sourceNodeName, string targetNodeName, double weight, bool bidirection)
        {
            if (!this.TryAddEdge(sourceNodeName, targetNodeName, weight, bidirection))
            {
                throw new EdgeAlreadyExistingException(sourceNodeName, targetNodeName);
            }

            return this;
        }

        /// <summary>
        /// Tries to add an edge that connects the sourceNode, given bv the 'sourceNodeName',
        /// and the targetNode, given by the 'targetNodeName'.
        /// </summary>
        /// <returns>True when the edge was actually added or false when an existing edge already exists.</returns>
        public virtual bool TryAddEdge(string sourceNodeName, string targetNodeName, double weight, bool bidirection)
        {
            if (!this.TryGetNode(sourceNodeName, out var sourceNode))
            {
                sourceNode = new GraphNode(sourceNodeName);
            }

            if (!this.TryGetNode(targetNodeName, out var targetNode))
            {
                targetNode = new GraphNode(targetNodeName);
            }

            return TryAddEdge(sourceNode, targetNode, weight, bidirection);
        }

        /// <summary>
        /// Tries to add an edge that connects the 'sourceNode' and the 'targetNode'.
        /// </summary>
        /// <returns>True when the edge was actually added or false when an existing edge already exists.</returns>
        public virtual bool TryAddEdge(GraphNode sourceNode, GraphNode targetNode, double weight, bool bidirection)
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

            this.TryAddNode(sourceNode);

            this.TryAddNode(targetNode);

            sourceNode.AddEdge(targetNode, weight, bidirection);

            return true;
        }

        public override string ToString() => $"Nodes: {this._nodes.Count}";
    }
}