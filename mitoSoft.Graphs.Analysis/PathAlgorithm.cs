using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mitoSoft.Graphs.Analysis
{
    public class PathAlgorithm
    {
        private readonly DirectedGraph _graph;

        public PathAlgorithm(DirectedGraph graph)
        {
            _graph = graph;
        }

        public List<string> GetAllPaths(string startNodeName)
        {
            var startNode = _graph.GetNode(startNodeName);

            return GetAllPaths(startNode);
        }

        public List<string> GetAllPaths(DirectedGraphNode startNode)
        {
            if (!_graph.IsDirected() || !_graph.IsAcyclic())
            {
                throw new InvalidOperationException("To determine the pathset the graph has to be directed and asyclic.");
            }

            var paths = new List<string>();

            BuildPath(startNode, startNode.Name, paths);

            return paths;
        }

        private void BuildPath(DirectedGraphNode node, string path, List<string> paths)
        {
            if (node.Successors.Count() == 0)
            {
                paths.Add(path);
            }
            else
            {
                foreach (var successor in node.Successors)
                {
                    BuildPath(successor, path + "->" + successor.Name, paths);
                }
            }
        }
    }
}