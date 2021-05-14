using mitoSoft.Graphs.Exceptions;
using System;
using System.Diagnostics;
using System.Linq;

namespace mitoSoft.Graphs
{
    [DebuggerDisplay(nameof(DirectedGraph) + " ({ToString()})")]
    public class DirectedGraph : Graph<DirectedGraphNode, DirectedEdge>
    {
        /// <summary>
        /// Tries to add a node with the given name to the graph.
        /// </summary>
        /// <param name="node">Name of the node to be added</param>
        /// <exception cref="NodeAlreadyExistingException">If a node with an identical key has already been added.</exception>
        public virtual DirectedGraph AddNode(string name)
        {
            var node = new DirectedGraphNode(name);

            this.AddNode(node);

            return this;
        }

        /// <summary>
        /// Tries to add a node with the given name to the graph.
        /// </summary>
        /// <param name="node">Name of the node to be added</param>
        public bool TryAddNode(string nodeName, out DirectedGraphNode node)
        {
            if (!this.TryGetNode(nodeName, out node))
            {
                this.AddNode(nodeName);

                return this.TryGetNode(nodeName, out node);
            }

            return false;
        }

        /// <summary>
        /// Add an edge that connects the 'sourceNode' and the 'targetNode'.
        /// </summary>
        public virtual DirectedGraph AddEdge(DirectedGraphNode sourceNode, DirectedGraphNode targetNode, double weight, bool bidirection)
        {
            if (!this.TryAddEdge(sourceNode, targetNode, weight, bidirection, out _))
            {
                throw new EdgeAlreadyExistingException(sourceNode.Name, targetNode.Name);
            }

            return this;
        }

        /// <summary>
        /// Add an edge that connects the sourceNode, given bv the 'sourceNodeName',
        /// and the targetNode, given by the 'targetNodeName'.
        /// </summary>
        public virtual DirectedGraph AddEdge(string sourceNodeName, string targetNodeName, double weight, bool bidirection)
        {
            if (!this.TryAddEdge(sourceNodeName, targetNodeName, weight, bidirection, out _))
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
        public virtual bool TryAddEdge(string sourceName, string targetName, double weight, bool bidirection, out DirectedEdge edge)
        {
            try
            {
                this.TryAddNode(sourceName, out var sourceNode);
                this.TryAddNode(targetName, out var targetNode);

                return this.TryAddEdge(sourceNode, targetNode, weight, bidirection, out edge);
            }
            catch (NodeNotFoundException)
            {
                edge = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to add an edge that connects the 'sourceNode' and the 'targetNode'.
        /// </summary>
        /// <returns>True when the edge was actually added or false when an existing edge already exists.</returns>
        public virtual bool TryAddEdge(DirectedGraphNode source, DirectedGraphNode target, double weight, bool bidirection, out DirectedEdge edge)
        {
            try
            {
                edge = this.GetEdge(source, target);
                return false;
            }
            catch (EdgeNotFoundException)
            {
                if (bidirection)
                {
                    edge = new BidirectedEdge(source, target, weight);
                }
                else
                {
                    edge = new DirectedEdge(source, target, weight);
                }

                source.AddEdge(edge);
                return true;
            }
            catch (Exception)
            {
                edge = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to return the edge, that connects the 'sourceNode' and the 'targetNode'.
        /// </summary>
        /// <returns>True when the edge was actually added or false when an existing edge already exists.</returns>
        public override bool TryGetEdge(DirectedGraphNode sourceNode, DirectedGraphNode targetNode, out DirectedEdge edge)
        {
            edge = sourceNode.Edges.Where(e => ReferenceEquals(e.Target, targetNode) || e is BidirectedEdge && ReferenceEquals(e.Source, targetNode)).SingleOrDefault();

            return (edge != null);
        }

        public override string ToString() => $"Nodes: {this._nodes.Count}";
    }
}