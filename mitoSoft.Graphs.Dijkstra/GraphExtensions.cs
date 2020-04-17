using System.Collections.Generic;

namespace mitoSoft.Graphs.ShortestPathAlgorithms
{
    public static class GraphExtensions
    {
        public static bool IsAcyclic(this Graph graph)
        {
            var isAcyclic = (new CycleChecker(graph)).IsAcyclic();

            return isAcyclic;
        }

        public static bool IsDirected(this Graph _)
        {
            return true; // at the moment it is not posible to create a undirected graph
        }

        public static List<string> GetAllPaths(this Graph graph, GraphNode startNode)
        {
            var paths = (new PathAlgorithm(graph)).GetAllPaths(startNode);

            return paths;
        }

        public static List<string> GetAllPaths(this Graph graph, string startNodeName)
        {
            var paths = (new PathAlgorithm(graph)).GetAllPaths(startNodeName);

            return paths;
        }

        /// <summary>
        /// Searches the shortest path to the Source node 
        /// via the DeepFirst-Search Algorithm
        /// </summary>
        public static Graph ToShortestGraph(this Graph graph, string sourceNodeName, string targetNodeName)
        {
            var sourceNode = graph.GetNode(sourceNodeName);
            var targetNode = graph.GetNode(targetNodeName);

            var shortestGraph = (new DeepFirstAlgorithm(graph)).GetShortestGraph(sourceNode, targetNode);

            return shortestGraph;
        }

        /// <summary>
        /// Searches the shortest path to the Source node 
        /// via the DeepFirst-Search Algorithm
        /// </summary>
        public static Graph ToShortestGraph(this Graph graph, GraphNode sourceNode, GraphNode targetNode)
        {
            var shortestGraph = (new DeepFirstAlgorithm(graph)).GetShortestGraph(sourceNode, targetNode);

            return shortestGraph;
        }

        /// <summary>
        /// Returns the node with the given name to the graph.
        /// </summary>
        /// <param name="node">Name of the node to be returned</param>
        /// <exception cref="NodeNotFoundException">If the node could not been found.</exception>
        public static DistanceNode GetDistanceNode(this Graph graph, string nodeName)
        {
            var distanceNode = (DistanceNode)graph.GetNode(nodeName);

            return distanceNode;
        }

        /// <summary>
        /// Searches the shortest path to the Source node 
        /// via the DeepFirst-Search Algorithm
        /// </summary>
        public static bool TryGetDistanceNode(this Graph graph, string nodeName, out DistanceNode distanceNode)
        {
            var result = graph.TryGetNode(nodeName, out var existingNode);
            if (result == true && existingNode is DistanceNode node)
            {
                distanceNode = node;

                return true;
            }
            else
            {
                distanceNode = null;

                return false;
            }
        }
    }
}