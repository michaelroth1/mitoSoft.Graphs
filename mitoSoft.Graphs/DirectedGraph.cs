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
        /// <param name="name">Name of the node to be added</param>
        /// <exception cref="NodeAlreadyExistingException">If a node with an identical key has already been added.</exception>
        public virtual DirectedGraph AddNode(string name)
        {
            var node = new DirectedGraphNode(name);

            this.AddNode(node);

            return this;
        }

        public new DirectedGraph AddNode(DirectedGraphNode node) => (DirectedGraph)base.AddNode(node);

        /// <summary>
        /// Tries to add a node with the given name to the graph.
        /// </summary>
        /// <param name="name">Name of the node to be added</param>
        public virtual bool TryAddNode(string name, out DirectedGraphNode node)
        {
            if (!this.TryGetNode(name, out node))
            {
                this.AddNode(name);

                return this.TryGetNode(name, out node);
            }

            return false;
        }

        /// <summary>
        /// Add an edge that connects the 'source' node and the 'target' node.
        /// </summary>
        public virtual DirectedGraph AddEdge(DirectedGraphNode source, DirectedGraphNode target, double weight, bool bidirection)
        {
            if (!this.TryAddEdge(source, target, weight, bidirection, out _))
            {
                throw new EdgeAlreadyExistingException(source.Name, target.Name);
            }

            return this;
        }

        /// <summary>
        /// Add an edge that connects the source node, given bv the 'sourceName',
        /// and the target node, given by the 'targetName'.
        /// </summary>
        public virtual DirectedGraph AddEdge(string sourceName, string targetName, double weight, bool bidirection)
        {
            if (!this.TryAddEdge(sourceName, targetName, weight, bidirection, out _))
            {
                throw new EdgeAlreadyExistingException(sourceName, targetName);
            }

            return this;
        }

        public new DirectedGraph AddEdge(DirectedEdge edge) => (DirectedGraph)base.AddEdge(edge);

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
        /// Tries to return the edge, that connects the 'source' node and the 'target' node.
        /// </summary>
        /// <returns>True when the edge was actually added or false when an existing edge already exists.</returns>
        public override bool TryGetEdge(DirectedGraphNode source, DirectedGraphNode target, out DirectedEdge edge)
        {
            edge = source.Edges.Cast<DirectedEdge>().SingleOrDefault(
                e => e is DirectedEdge de && ReferenceEquals(de.Target, target) && ReferenceEquals(de.Source, source)
                  || e is BidirectedEdge be && ( ReferenceEquals(be.Target, target) && ReferenceEquals(be.Source, source)
                                              || ReferenceEquals(be.Source, target) && ReferenceEquals(be.Target, source)));

            return (edge != null);
        }

        public override string ToString() => $"Nodes: {this._nodes.Count}";
    }
}