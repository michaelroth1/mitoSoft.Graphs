using System.Collections.Generic;
using System.Diagnostics;

namespace mitoSoft.Graphs.Analysis
{
    [DebuggerDisplay(nameof(DeepFirstAlgorithm) + " ({ToString()})")]
    public class DeepFirstAlgorithm : DistanceCalculatorBase
    {
        private readonly double _maxDistance;

        /// <remarks>
        /// <paramref name="maxDistance"/> is set up to 20 by default.
        /// </remarks>
        public DeepFirstAlgorithm(DirectedGraph graph) : this(graph, 20d)
        {
        }

        public DeepFirstAlgorithm(DirectedGraph graph, double maxDistance) : base(graph)
        {
            _maxDistance = maxDistance;
        }

        /// <summary>
        /// Determines the shortest path between the sourceNode, given by the 'sourceNodeName',
        /// and the targetNode, given by the 'targetNodeName'.
        /// </summary>
        public override DirectedGraph GetShortestGraph(string sourceNodeName, string targetNodeName)
        {
            var sourceNode = _graph.GetNode(sourceNodeName);
            var targetNode = _graph.GetNode(targetNodeName);

            return GetShortestGraph(sourceNode, targetNode);
        }

        /// <summary>
        /// Determines the shortest path between the 'sourceNode' and the 'targetNode'.
        /// </summary>
        public override DirectedGraph GetShortestGraph(DirectedGraphNode sourceNode, DirectedGraphNode targetNode)
        {
            InitializeSearch(sourceNode);

            CalculateDistancesByDeepFirst(sourceNode, _maxDistance);

            return BuildShortestPathGraph(sourceNode, targetNode);
        }

        /// <summary>
        /// Determines the distances of all nodes starting from the sourceNode, 
        /// given by the 'sourceNodeName'.
        /// </summary>
        public override IDictionary<string, double> GetAllDistances(string sourceNodeName)
        {
            var sourceNode = _graph.GetNode(sourceNodeName);

            return GetAllDistances(sourceNode);
        }

        /// <summary>
        /// Determines the distances of all nodes starting from the 'sourceNode'.
        /// </summary>
        public override IDictionary<string, double> GetAllDistances(DirectedGraphNode sourceNode)
        {
            InitializeSearch(sourceNode);

            CalculateDistancesByDeepFirst(sourceNode, _maxDistance);

            return _distances;
        }

        private void CalculateDistancesByDeepFirst(DirectedGraphNode sourceNode, double maxDistance)
        {
            foreach (var edge in sourceNode.Edges)
            {
                DirectedGraphNode targetNode = this.GetTargetNode(edge, sourceNode);

                var distance = checked(_distances[sourceNode.Name] + edge.Weight);

                if (distance > maxDistance)
                {
                    //nothing to do
                }
                else if (distance < _distances[targetNode.Name])
                {
                    _distances[targetNode.Name] = distance;

                    CalculateDistancesByDeepFirst(targetNode, maxDistance);
                }
            }
        }

        private DirectedGraphNode GetTargetNode(DirectedEdge edge, DirectedGraphNode sourceNode)
        {
            DirectedGraphNode targetNode;
            if (edge is BidirectedEdge && edge.Source != sourceNode)
            {
                targetNode = edge.Source;
            }
            else
            {
                targetNode = edge.Target;
            }
            return targetNode;
        }

        public override string ToString() => $"{this._graph}";
    }
}