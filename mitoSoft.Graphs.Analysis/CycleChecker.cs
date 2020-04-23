using System.Collections.Generic;

namespace mitoSoft.Graphs.Analysis
{
    public class CycleChecker
    {
        private readonly Graph _graph;

        public CycleChecker(Graph graph)
        {
            _graph = graph;
        }

        public bool IsAcyclic()
        {
            return !IsCyclic();
        }

        /// <summary>
        /// Checks if the graph is ascyclic or not
        /// </summary>
        /// <returns>
        /// true if the graph contains a cycle, else false.  
        /// </returns>
        /// <remarks>
        /// This function is a variation of DFS() in  
        /// https://www.geeksforgeeks.org/archives/18212  
        /// </remarks>.  
        public bool IsCyclic()
        {
            var visited = new Dictionary<GraphNode, bool>();
            var recStack = new Dictionary<GraphNode, bool>();

            foreach (var node in _graph.Nodes)
            {
                visited.Add(node, false);
                recStack.Add(node, false);
            }

            foreach (var node in _graph.Nodes)
            {
                if (IsCyclicUtil(node, visited, recStack))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsCyclicUtil(GraphNode node, Dictionary<GraphNode, bool> visited, Dictionary<GraphNode, bool> recStack)// i, bool[] visited, bool[] recStack)
        {
            // Mark the current node as visited and  
            // part of recursion stack  
            if (recStack[node])
            {
                return true;
            }

            if (visited[node])
            {
                return false;
            }

            visited[node] = true;

            recStack[node] = true;

            foreach (var sucessor in node.Successors)
            {
                if (IsCyclicUtil(sucessor, visited, recStack))
                {
                    return true;
                }
            }

            recStack[node] = false;

            return false;
        }
    }
}