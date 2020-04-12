using System.Collections.Generic;
using System.Diagnostics;

namespace mitoSoft.Graphs.ShortestPathAlgorithms
{
    [DebuggerDisplay(nameof(DeepFirstAlgorithm) + " ({ToString()})")]
    public class DeepFirstAlgorithm : DistanceCalculatorBase
    {
        private readonly double _maxDistance;

        /// <remarks>
        /// <paramref name="maxDistance"/> is set up to 20 by default.
        /// </remarks>
        public DeepFirstAlgorithm(Graph graph) : this(graph, 20d)
        {
        }

        public DeepFirstAlgorithm(Graph graph, double maxDistance) : base(graph)
        {
            _maxDistance = maxDistance;
        }

        /// <summary>
        /// Determines the shortest path between the sourceNode, given by the 'sourceNodeName',
        /// and the targetNode, given by the 'targetNodeName'.
        /// </summary>
        public override Graph GetShortestGraph(string sourceNodeName, string targetNodeName)
        {
            var sourceNode = _graph.GetNode(sourceNodeName);
            var targetNode = _graph.GetNode(targetNodeName);

            return GetShortestGraph(sourceNode, targetNode);
        }

        /// <summary>
        /// Determines the shortest path between the 'sourceNode' and the 'targetNode'.
        /// </summary>
        public override Graph GetShortestGraph(GraphNode sourceNode, GraphNode targetNode)
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
        public override IDictionary<string, double> GetAllDistances(GraphNode sourceNode)
        {
            InitializeSearch(sourceNode);

            CalculateDistancesByDeepFirst(sourceNode, _maxDistance);

            return _distances;
        }

        private void CalculateDistancesByDeepFirst(GraphNode sourceNode, double maxDistance)
        {
            foreach (var edge in sourceNode.Edges)
            {
                var targetNode = edge.TargetNode;

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

        public override string ToString() => $"{this._graph}";
    }
}