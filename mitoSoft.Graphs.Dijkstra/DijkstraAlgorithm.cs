using System.Diagnostics;
using System.Linq;

namespace mitoSoft.Graphs.ShortestPathAlgorithms
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

        public Graph GetShortestGraph(string sourceNodeName, string targetNodeName, bool equallyWeighted = false)
        {
            var sourceNode = _graph.GetNode(sourceNodeName);
            var targetNode = _graph.GetNode(targetNodeName);

            return GetShortestGraph(sourceNode, targetNode, equallyWeighted);
        }

        public Graph GetShortestGraph(GraphNode sourceNode, GraphNode targetNode, bool equallyWeighted = false)
        {
            InitializeSearch(sourceNode);

            CalculateDistancesByBreadthFirst(targetNode, equallyWeighted);

            return BuildShortestPathGraph(sourceNode, targetNode);
        }

        private void CalculateDistancesByBreadthFirst(GraphNode targetNode, bool equallyWeighted)
        {
            var finished = false;

            var queue = this._graph.Nodes.Cast<DistanceNode>().ToList();

            while (!finished)
            {
                queue.Sort((left, right) => left.Distance.CompareTo(right.Distance));

                DistanceNode nextNode = queue.FirstOrDefault(n => !double.IsPositiveInfinity(n.Distance));

                if (nextNode != null)
                {
                    //only possible by equally weighted graphs
                    if (equallyWeighted == true && ReferenceEquals(nextNode, targetNode))
                    {
                        finished = true;
                    }

                    UpdateDistance(nextNode);

                    queue.Remove(nextNode);
                }
                else
                {
                    finished = true;
                }
            }
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