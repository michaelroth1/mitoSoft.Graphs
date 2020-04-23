using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mitoSoft.Graphs.Analysis
{
    /// <summary>
    /// https://de.wikipedia.org/wiki/Dijkstra-Algorithmus
    /// </summary>
    [DebuggerDisplay(nameof(DijkstraAlgorithm) + " ({ToString()})")]
    public class DijkstraAlgorithm : DistanceCalculatorBase
    {
        public DijkstraAlgorithm(Graph graph) : base(graph)
        {
        }

        public override Graph GetShortestGraph(string sourceNodeName, string targetNodeName)
        {
            var sourceNode = _graph.GetNode(sourceNodeName);
            var targetNode = _graph.GetNode(targetNodeName);

            return GetShortestGraph(sourceNode, targetNode);
        }

        public override Graph GetShortestGraph(GraphNode sourceNode, GraphNode targetNode)
        {
            InitializeSearch(sourceNode);

            CalculateDistancesByBreadthFirst();

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

            CalculateDistancesByBreadthFirst();

            return _distances;
        }

        private void CalculateDistancesByBreadthFirst()
        {
            var finished = false;

            var queue = this._graph.Nodes.ToList();

            while (!finished)
            {
                queue.Sort(CompareNodes);

                var nextNode = queue.FirstOrDefault(n => !double.IsPositiveInfinity(_distances[n.Name]));

                if (nextNode != null)
                {
                    UpdateDistance(nextNode);

                    queue.Remove(nextNode);
                }
                else
                {
                    finished = true;
                }
            }
        }

        private int CompareNodes(GraphNode left, GraphNode right)
        {
            var leftDistance = _distances[left.Name];
            var rightDistance = _distances[right.Name];

            return leftDistance.CompareTo(rightDistance);
        }

        private void UpdateDistance(GraphNode sourceNode)
        {
            foreach (var edge in sourceNode.Edges)
            {
                var node = edge.TargetNode;

                var distance = checked(_distances[sourceNode.Name] + edge.Weight);

                if (distance < _distances[node.Name])
                {
                    _distances[node.Name] = distance;
                }
            }
        }

        public override string ToString() => $"{this._graph}";
    }
}