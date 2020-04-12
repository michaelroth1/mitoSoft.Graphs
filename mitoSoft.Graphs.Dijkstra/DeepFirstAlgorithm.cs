using System.Collections.Generic;
using System.Diagnostics;

namespace mitoSoft.Graphs.ShortestPathAlgorithms
{
    [DebuggerDisplay(nameof(DeepFirstAlgorithm) + " ({ToString()})")]
    public class DeepFirstAlgorithm : DistanceCalculatorBase
    {
        public DeepFirstAlgorithm(Graph graph) : base(graph)
        {
        }

        /// <summary>
        /// Determines the shortest path between the sourceNode, given by the 'sourceNodeName',
        /// and the targetNode, given by the 'targetNodeName'.
        /// </summary>
        /// <remarks>
        /// maxDistance = 20
        /// </remarks>
        public Graph GetShortestGraph(string sourceNodeName, string targetNodeName)
        {
            return GetShortestGraph(sourceNodeName, targetNodeName, 20);
        }

        public Graph GetShortestGraph(string sourceNodeName, string targetNodeName, int maxDistance)
        {
            var sourceNode = _graph.GetNode(sourceNodeName);
            var targetNode = _graph.GetNode(targetNodeName);

            return GetShortestGraph(sourceNode, targetNode, maxDistance);
        }

        /// <summary>
        /// Determines the shortest path between the 'sourceNode' and the 'targetNode'.
        /// </summary>
        /// <remarks>
        /// maxDistance = 20
        /// </remarks>
        public Graph GetShortestGraph(GraphNode sourceNode, GraphNode targetNode)
        {
            return GetShortestGraph(sourceNode, targetNode, 20);
        }

        public Graph GetShortestGraph(GraphNode sourceNode, GraphNode targetNode, int maxDistance = 20)
        {
            InitializeSearch(sourceNode);

            CalculateDistancesByDeepFirst(sourceNode, maxDistance);

            return BuildShortestPathGraph(sourceNode, targetNode);
        }

        public IDictionary<string, double> GetAllDistances(GraphNode sourceNode, int maxDistance = 20)
        {
            InitializeSearch(sourceNode);

            CalculateDistancesByDeepFirst(sourceNode, maxDistance);

            return _distances;
        }

        private void CalculateDistancesByDeepFirst(GraphNode sourceNode, int maxDistance)
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