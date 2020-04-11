using System;
using System.Diagnostics;

namespace mitoSoft.Graphs.ShortestPathAlgorithms
{
    [DebuggerDisplay(nameof(DistanceGraph) + " ({ToString()})")]
    public class DistanceGraph : Graph
    {
        public DistanceGraph() : base()
        {
        }

        public new DistanceNode GetNode(string nodeName)
        {
            return (DistanceNode)base.GetNode(nodeName);
        }

        public bool TryGetNode(string nodeName, out DistanceNode node)
        {
            if (base.TryGetNode(nodeName, out GraphNode graphNode))
            {
                node = (DistanceNode)graphNode;

                return true;
            }
            else
            {
                node = null;

                return false;
            }
        }

        /// <summary>
        /// Tries to add the given node to the system. If the node already exists, nothing will be added.
        /// </summary>
        /// <param name="nodeName">Name of the node to be added</param>
        /// <returns>True when the node was actually added or false when a node with an identical name already exists.</returns>
        public override bool TryAddNode(string nodeName, out GraphNode node)
        {
            var newNode = new DistanceNode(nodeName);

            var existing = this.TryAddNode(ref newNode);

            node = newNode;

            return existing;
        }

        /// <summary>
        /// Tries to add the given node to the system. If the node already exists, the existing node will be returned.
        /// </summary>
        /// <param name="node">The node to be added or upon return the existing node.</param>
        /// <returns>True when the node was actually added or false when an existing node is returned.</returns>
        public bool TryAddNode(ref DistanceNode node)
        {
            GraphNode baseNode = node;

            var added = base.TryAddNode(ref baseNode);

            node = (DistanceNode)baseNode;

            return added;
        }

        /// <summary>
        /// Tries to add the given node to the system. If the node already exists, the existing node will be returned.
        /// </summary>
        /// <param name="node">The node to be added or upon return the existing node.</param>
        /// <returns>True when the node was actually added or false when an existing node is returned.</returns>
        public override bool TryAddNode(ref GraphNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            else if (!(node is DistanceNode))
            {
                throw new ArgumentException($"{nameof(node)} is not a {nameof(DistanceNode)}.");
            }

            return this.TryAddNode(ref node);
        }

        public new DistanceGraph AddNode(string nodeName)
        {
            var node = new DistanceNode(nodeName);

            this.AddNode(node);

            return this;
        }

        public new DistanceGraph AddNode(GraphNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            else if (!(node is DistanceNode))
            {
                throw new ArgumentException($"{nameof(node)} is not a {nameof(DistanceNode)}.");
            }

            return (DistanceGraph)base.AddNode(node);
        }

        public new DistanceGraph AddEdge(string sourceNodeName, string targetNodeName, double distance, bool twoWay)
        {
            return (DistanceGraph)base.AddEdge(sourceNodeName, targetNodeName, distance, twoWay);
        }

        public new DistanceGraph AddEdge(GraphNode sourceNode, GraphNode targetNode, double distance, bool twoWay)
        {
            return (DistanceGraph)base.AddEdge(sourceNode, targetNode, distance, twoWay);
        }

        public override bool TryAddEdge(GraphNode sourceNode, GraphNode targetNode, double distance, bool twoWay)
        {
            if (sourceNode == null)
            {
                throw new ArgumentNullException(nameof(sourceNode));
            }
            else if (!(sourceNode is DistanceNode))
            {
                throw new ArgumentException($"{nameof(sourceNode)} is not a {nameof(DistanceNode)}.");
            }
            else if (targetNode == null)
            {
                throw new ArgumentNullException(nameof(targetNode));
            }
            else if (!(targetNode is DistanceNode))
            {
                throw new ArgumentException($"{nameof(targetNode)} is not a {nameof(DistanceNode)}.");
            }

            return base.TryAddEdge(sourceNode, targetNode, distance, twoWay);
        }

        /// <summary>
        /// Shortest Graph Calculation via Deep First Search
        /// </summary>
        /// <returns></returns>
        public DistanceGraph GetShortestGraph(string sourceNodeName, string targetNodeName)
        {
            var shortesGraph = (new DeepFirstAlgorithm(this)).GetShortestGraph(sourceNodeName, targetNodeName);

            return shortesGraph;
        }
    }
}