using System;
using System.Diagnostics;

namespace mitoSoft.Graphs.Dijkstra
{
    [DebuggerDisplay(nameof(DistanceGraph) + " ({ToString()})")]
    public class DistanceGraph : Graph
    {
        public DistanceGraph() : base()
        {
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
        /// Tries to add the given node to the system. If the node already exists, the existing node will be returned.
        /// </summary>
        /// <param name="node">The node to be added or upon return the existing node.</param>
        /// <returns>True when the node was actually added or false when an existing node is returned.</returns>
        public bool TryAddNode(ref DistanceNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

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

            var added = base.TryAddNode(ref node);

            return added;
        }

        public override GraphNode AddNode(string nodeName)
        {
            var node = new DistanceNode(nodeName);

            this.AddNode(node);

            return node;
        }

        public override void AddNode(GraphNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            else if (!(node is DistanceNode))
            {
                throw new ArgumentException($"{nameof(node)} is not a {nameof(DistanceNode)}.");
            }

            base.AddNode(node);
        }
                
        public override void AddConnection(GraphNode sourceNode, GraphNode targetNode, double distance, bool twoWay)
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

            base.AddConnection(sourceNode, targetNode, distance, twoWay);
        }

    }
}