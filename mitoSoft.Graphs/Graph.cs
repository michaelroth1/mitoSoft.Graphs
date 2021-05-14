using mitoSoft.Graphs.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mitoSoft.Graphs
{
    public abstract class Graph<TNode, TEdge>
        where TNode : Node
        where TEdge : Edge
    {
        protected readonly Dictionary<string, TNode> _nodes = new Dictionary<string, TNode>();

        /// <summary>
        /// Returns all nodes of the graph
        /// </summary>
        public virtual IEnumerable<TNode> Nodes => this._nodes.Values;

        /// <summary>
        /// Returns all edges of the graph
        /// </summary>
        public virtual IEnumerable<TEdge> Edges => this.Nodes.Cast<TNode>().SelectMany(n => n.Edges.Cast<TEdge>()).Distinct();

        /// <summary>
        /// Tries to add the given node to the graph.
        /// </summary>
        /// <param name="node">The node to be added.</param>
        /// <returns>The Graph with the added node <paramref name="node"/></returns>
        public virtual Graph<TNode, TEdge> AddNode(TNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (!this._nodes.ContainsKey(node.Name))
            {
                this._nodes.Add(node.Name, node);
            }
            else
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
        public virtual bool TryAddNode(TNode node)
        {
            if (!_nodes.ContainsKey(node.Name))
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
        /// Returns the node with the given name to the graph.
        /// </summary>
        /// <param name="node">Name of the node to be returned</param>
        /// <exception cref="NodeNotFoundException">If the node could not been found.</exception>
        public virtual TNode GetNode(string nodeName)
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
        /// Tries to returns the node with the given name to the graph.
        /// </summary>
        /// <param name="node">Name of the node to be returned</param>
        /// <returns>True when the node was found or false when the node does not exist.</returns>
        public virtual bool TryGetNode(string nodeName, out TNode node) => this._nodes.TryGetValue(nodeName, out node);

        /// <summary>
        /// Tries to add the given edge to the graph.
        /// </summary>
        /// <param name="edge">The edge to be added.</param>
        /// <returns>The Graph with the added edge <paramref name="edge"/></returns>
        public virtual Graph<TNode, TEdge> AddEdge(TEdge edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            if (this.Edges.Contains(edge))
            {
                throw new EdgeAlreadyExistingException(edge.Source?.Name, edge.Target?.Name);
            }

            edge.Source.AddEdge(edge);

            return this;
        }

        /// <summary>
        /// Return the edge that connects the 'sourceNode' and the 'targetNode'.
        /// </summary>
        public virtual TEdge GetEdge(TNode source, TNode target)
        {
            if (this.TryGetEdge(source, target, out var edge))
            {
                return edge;
            }
            else
            {
                throw new EdgeNotFoundException(source.Name, target.Name);
            }
        }

        /// <summary>
        /// Return the edge that connects the 'sourceNode' and the 'targetNode'.
        /// </summary>
        public virtual TEdge GetEdge(string sourceName, string targetName)
        {
            if (this.TryGetEdge(sourceName, targetName, out var edge))
            {
                return edge;
            }
            else
            {
                throw new EdgeNotFoundException(sourceName, targetName);
            }
        }

        /// <summary>
        /// Tries to return the edge, that connects the 'sourceNode' and the 'targetNode'.
        /// </summary>
        /// <returns>True when the edge was actually added or false when an existing edge already exists.</returns>
        public virtual bool TryGetEdge(TNode sourceNode, TNode targetNode, out TEdge edge)
        {
            edge = sourceNode.Edges.Cast<TEdge>().SingleOrDefault(e => ReferenceEquals(e.Target, targetNode));

            return (edge != null);
        }

        /// <summary>
        /// Tries to return the edge, that connects the 'sourceNode' and the 'targetNode'.
        /// </summary>
        /// <returns>True when the edge was actually added or false when an existing edge already exists.</returns>
        public virtual bool TryGetEdge(string sourceName, string targetName, out TEdge edge)
        {
            try
            {
                var sourceNode = this.GetNode(sourceName);
                var targetNode = this.GetNode(targetName);

                edge = sourceNode.Edges.Cast<TEdge>().SingleOrDefault(e => ReferenceEquals(e.Target, targetNode));

                return (edge != null);
            }
            catch (NodeNotFoundException)
            {
                edge = null;
                return false;
            }
        }
    }
}