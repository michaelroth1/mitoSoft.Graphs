using System.Collections.Generic;

namespace mitoSoft.Graphs.Analysis
{
    public static class GraphExtensions
    {
        public static bool IsAcyclic(this DirectedGraph graph)
        {
            var isAcyclic = (new CycleChecker(graph)).IsAcyclic();

            return isAcyclic;
        }

        public static bool IsDirected(this DirectedGraph _)
        {
            return true; // at the moment it is not posible to create a undirected graph
        }

        public static List<string> GetAllPaths(this DirectedGraph graph, DirectedGraphNode startNode)
        {
            var paths = (new PathAlgorithm(graph)).GetAllPaths(startNode);

            return paths;
        }

        public static List<string> GetAllPaths(this DirectedGraph graph, string startNodeName)
        {
            var paths = (new PathAlgorithm(graph)).GetAllPaths(startNodeName);

            return paths;
        }

        /// <summary>
        /// Searches the shortest path to the Source node 
        /// via the DeepFirst-Search Algorithm
        /// </summary>
        public static DirectedGraph ToShortestGraph(this DirectedGraph graph, string sourceNodeName, string targetNodeName)
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
        public static DirectedGraph ToShortestGraph(this DirectedGraph graph, DirectedGraphNode sourceNode, DirectedGraphNode targetNode)
        {
            var shortestGraph = (new DeepFirstAlgorithm(graph)).GetShortestGraph(sourceNode, targetNode);

            return shortestGraph;
        }

        /// <summary>
        /// Returns the node with the given name to the graph.
        /// </summary>
        /// <param name="node">Name of the node to be returned</param>
        /// <exception cref="NodeNotFoundException">If the node could not been found.</exception>
        public static DistanceNode GetDistanceNode(this DirectedGraph graph, string nodeName)
        {
            var distanceNode = (DistanceNode)graph.GetNode(nodeName);

            return distanceNode;
        }

        /// <summary>
        /// Searches the shortest path to the Source node 
        /// via the DeepFirst-Search Algorithm
        /// </summary>
        public static bool TryGetDistanceNode(this DirectedGraph graph, string nodeName, out DistanceNode distanceNode)
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