using System.Diagnostics;

namespace mitoSoft.Graphs.Dijkstra
{
    [DebuggerDisplay(nameof(DeepFirstAlgorithm) + " ({ToString()})")]
    public class DeepFirstAlgorithm : DistanceCalculatorBase
    {
        public DeepFirstAlgorithm(DistanceGraph graph) : base(graph)
        {
        }

        public DistanceGraph GetShortestGraph(string sourceNodeName, string targetNodeName, int maxDistance = 20)
        {
            var sourceNode = Check(sourceNodeName);
            var targetNode = Check(targetNodeName);

            return GetShortestGraph(sourceNode, targetNode, maxDistance);
        }

        public DistanceGraph GetShortestGraph(DistanceNode sourceNode, DistanceNode targetNode, int maxDistance = 20)
        {
            InitializeGraph(sourceNode);

            CalculateDistancesByDeepFirst(sourceNode, false, maxDistance);

            return BuildShortestPathGraph(sourceNode, targetNode);
        }

        private void CalculateDistancesByDeepFirst(DistanceNode sourceNode, bool finished, int maxDistance)
        {
            foreach (var connection in sourceNode.Connections)
            {
                var targetNode = (DistanceNode)connection.TargetNode;

                var distance = checked(sourceNode.DistanceFromStart + connection.Distance);

                if (distance > maxDistance)
                {
                    //nothing to do
                }
                else if (distance < targetNode.DistanceFromStart)
                {
                    targetNode.SetDistanceFromStart(distance);

                    CalculateDistancesByDeepFirst(targetNode, finished, maxDistance);
                }
            }
        }

        public override string ToString() => $"{this._graph}";
    }
}