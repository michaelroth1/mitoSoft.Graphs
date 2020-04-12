using mitoSoft.Graphs.ShortestPathAlgorithms.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mitoSoft.Graphs.ShortestPathAlgorithms
{
    public abstract class DistanceCalculatorBase
    {
        protected Graph _graph;

        protected Dictionary<string, double> _distances;

        public DistanceCalculatorBase(Graph graph)
        {
            this._graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        public abstract Graph GetShortestGraph(GraphNode sourceNode, GraphNode targetNode);

        public abstract Graph GetShortestGraph(string sourceNodeName, string targetNodeName);

        public abstract IDictionary<string, double> GetAllDistances(string sourceNodeName);

        public abstract IDictionary<string, double> GetAllDistances(GraphNode sourceNode);

        protected void InitializeSearch(GraphNode sourceNode)
        {
            _distances = new Dictionary<string, double>();

            foreach (GraphNode node in this._graph.Nodes)
            {
                _distances.Add(node.Name, double.PositiveInfinity);
            }

            _distances.Remove(sourceNode.Name);
            _distances.Add(sourceNode.Name, 0);
        }

        protected Graph BuildShortestPathGraph(GraphNode sourceNode, GraphNode targetNode)
        {
            if (_distances[targetNode.Name] == double.PositiveInfinity)
            {
                throw new PathNotFoundException(sourceNode.Name, targetNode.Name);
            }

            var graph = new Graph();

            //Set start Node
            var node = new DistanceNode(targetNode.Name);
            var distance = _distances[targetNode.Name];
            node.Description = node.Name + Environment.NewLine + "Distance: " + distance;
            node.Distance = distance;

            graph.AddNode(node);

            GetShortestGraph(sourceNode, targetNode, graph);

            return graph;
        }

        private void GetShortestGraph(GraphNode sourceNode, GraphNode targetNode, Graph graph)
        {
            if (sourceNode.Name == targetNode.Name)
            {
                return;
            }

            var predecessorNodes = this.GetShortestPathPredecessors(targetNode);

            foreach (var predecessor in predecessorNodes)
            {
                var node = new DistanceNode(predecessor.Name);

                if (!graph.TryAddNode(node))
                {
                    node = (DistanceNode)graph.GetNode(node.Name);
                }

                var distance = _distances[predecessor.Name];
                node.Description = node.Name + Environment.NewLine + "Distance: " + distance;
                node.Distance = distance;
                _graph.TryGetEdge(predecessor, targetNode, out var edge);

                graph.TryAddEdge(predecessor.Name, targetNode.Name, edge.Weight, false);

                GetShortestGraph(sourceNode, predecessor, graph);
            }
        }

        /// <summary>
        /// The shortest path predecessors are all nodes which have the smallest sum of predecessorNode.DistanceFromStart + connectionToThisNode.Distance.
        /// </summary>
        public IEnumerable<GraphNode> GetShortestPathPredecessors(GraphNode node)
        {
            var predecessors = node.Predecessors.ToList();

            if (predecessors.Count > 0)
            {
                var min = predecessors.Min(p => GetStartDistanceToMe(p, node));

                var result = predecessors.Where(p => GetStartDistanceToMe(p, node) == min);

                return result;
            }
            else
            {
                return Enumerable.Empty<DistanceNode>();
            }
        }

        private double GetStartDistanceToMe(GraphNode predecessor, GraphNode successor)
        {
            var predecessorDistanceFromStart = _distances[predecessor.Name];

            var predecessorDistanceToMe = _graph.GetEdge(predecessor, successor).Weight;

            var startDistanceToMe = predecessorDistanceFromStart + predecessorDistanceToMe;

            return startDistanceToMe;
        }
    }
}