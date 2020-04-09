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
        //{ TODO Warum???
        //    get
        //    {
        //        foreach (var node in this._nodes.Values)
        //        {
        //            yield return node;
        //        }
        //    }
        //}

        public IEnumerable<GraphEdge> Connections => this.Nodes.SelectMany(n => n.Connections).Distinct();

        public bool TryGetNode(string nodeName, out GraphNode node) => this._nodes.TryGetValue(nodeName, out node);

        public virtual GraphNode AddNode(string nodeName)
        {
            var node = new GraphNode(nodeName);

            this.AddNode(node);

            return node;
        }

        public virtual void AddNode(GraphNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            else if (!this.TryAddNode(ref node))
            {
                throw new InvalidOperationException($"Node with identical name {node.Name} already in graph.");
            }
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

        public GraphEdge TryGetConnector(string startNodeName, string endNodeName)
        {
            if (!this.TryGetNode(startNodeName, out var startNode))
            {
                throw new NodeNotInGraphException(startNodeName);
            }
            
            if (!this._nodes.TryGetValue(endNodeName, out var endNode))
            {
                throw new NodeNotInGraphException(endNodeName);
            }

            return TryGetConnector(startNode, endNode);
        }

        public GraphEdge TryGetConnector(GraphNode startNode, GraphNode endNode)
        {
            if (!this._nodes.TryGetValue(startNode.Name, out _))
            {
                throw new NodeNotInGraphException(startNode);
            }
            else if (!this._nodes.TryGetValue(endNode.Name, out _))
            {
                throw new NodeNotInGraphException(endNode);
            }

            foreach (var connector in startNode.Connections)
            {
                if (ReferenceEquals(connector.TargetNode, endNode))
                {
                    return connector;
                }
            }

            throw new InvalidOperationException($"No dircect connection between {startNode.Name} and {endNode.Name}.");
        }

        public virtual void AddConnection(string sourceNodeName, string targetNodeName, double distance, bool twoWay)
        {
            if (!this.TryGetNode(sourceNodeName, out var sourceNode))
            {
                throw new NodeNotInGraphException(sourceNodeName);
            }

            if (!this.TryGetNode(targetNodeName, out var targetNode))
            {
                throw new NodeNotInGraphException(targetNodeName);
            }

            AddConnection(sourceNode, targetNode, distance, twoWay);
        }

        public virtual void AddConnection(GraphNode sourceNode, GraphNode targetNode, double distance, bool twoWay)
        {
            if (sourceNode == null)
            {
                throw new ArgumentNullException(nameof(sourceNode));
            }
            else if (targetNode == null)
            {
                throw new ArgumentNullException(nameof(targetNode));
            }
            else if (!_nodes.ContainsKey(sourceNode.Name))
            {
                throw new NodeNotInGraphException(sourceNode);
            }
            else if (!_nodes.ContainsKey(targetNode.Name))
            {
                throw new NodeNotInGraphException(targetNode);
            }

            sourceNode.AddConnection(targetNode, distance, twoWay);
        }

        public override string ToString() => $"Nodes: {this._nodes.Count}";
    }
}