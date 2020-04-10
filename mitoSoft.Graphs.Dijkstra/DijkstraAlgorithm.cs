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
        public DijkstraAlgorithm(DistanceGraph graph) : base(graph)
        {
        }

        public DistanceGraph GetShortestGraph(string sourceNodeName, string targetNodeName, bool equallyWeighted = false)
        {
            var sourceNode = Check(sourceNodeName);
            var targetNode = Check(targetNodeName);

            return GetShortestGraph(sourceNode, targetNode, equallyWeighted);
        }

        public DistanceGraph GetShortestGraph(DistanceNode sourceNode, DistanceNode targetNode, bool equallyWeighted = false)
        {
            InitializeGraph(sourceNode);

            CalculateDistancesByBreadthFirst(targetNode, equallyWeighted);

            return BuildShortestPathGraph(sourceNode, targetNode);
        }

        private void CalculateDistancesByBreadthFirst(DistanceNode targetNode, bool equallyWeighted)
        {
            var finished = false;

            var queue = this._graph.Nodes.Cast<DistanceNode>().ToList();

            while (!finished)
            {
                queue.Sort((left, right) => left.DistanceFromStart.CompareTo(right.DistanceFromStart));

                DistanceNode nextNode = queue.FirstOrDefault(n => !double.IsPositiveInfinity(n.DistanceFromStart));

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
        
        private void UpdateDistance(DistanceNode sourceNode)
        {
            foreach (var connection in sourceNode.Edges)
            {
                var node = (DistanceNode)connection.TargetNode;

                var distance = checked(sourceNode.DistanceFromStart + connection.Distance);

                if (distance < node.DistanceFromStart)
                {
                    node.SetDistanceFromStart(distance);
                }
            }
        }

        public override string ToString() => $"{this._graph}";
    }
}