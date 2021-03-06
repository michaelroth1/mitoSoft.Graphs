﻿using mitoSoft.Graphs.Analysis.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mitoSoft.Graphs.Analysis
{
    public abstract class DistanceCalculatorBase
    {
        protected DirectedGraph _graph;

        protected Dictionary<string, double> _distances;

        public DistanceCalculatorBase(DirectedGraph graph)
        {
            this._graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        public abstract DirectedGraph GetShortestGraph(DirectedGraphNode sourceNode, DirectedGraphNode targetNode);

        public abstract DirectedGraph GetShortestGraph(string sourceNodeName, string targetNodeName);

        public abstract IDictionary<string, double> GetAllDistances(string sourceNodeName);

        public abstract IDictionary<string, double> GetAllDistances(DirectedGraphNode sourceNode);

        protected void InitializeSearch(DirectedGraphNode sourceNode)
        {
            _distances = new Dictionary<string, double>();

            foreach (DirectedGraphNode node in this._graph.Nodes)
            {
                _distances.Add(node.Name, double.PositiveInfinity);
            }

            _distances.Remove(sourceNode.Name);
            _distances.Add(sourceNode.Name, 0);
        }

        protected DirectedGraph BuildShortestPathGraph(DirectedGraphNode sourceNode, DirectedGraphNode targetNode)
        {
            if (_distances[targetNode.Name] == double.PositiveInfinity)
            {
                throw new PathNotFoundException(sourceNode.Name, targetNode.Name);
            }

            var graph = new DirectedGraph();

            //Set start Node
            var distanceNode = new DistanceNode(targetNode.Name)
            {
                Tag = targetNode.Tag
            };
            var distance = _distances[targetNode.Name];
            distanceNode.Distance = distance;

            graph.AddNode(distanceNode);

            GetShortestGraph(sourceNode, targetNode, graph);

            //set node distances
            foreach (var node in graph.Nodes)
            {
                node.Description = node.Name + Environment.NewLine + "Distance:" + _distances[node.Name].ToString();
            }

            //set edge description
            foreach (var edge in graph.Edges)
            {
                edge.Description = edge.Weight.ToString();
            }

            return graph;
        }

        private void GetShortestGraph(DirectedGraphNode sourceNode, DirectedGraphNode targetNode, DirectedGraph graph)
        {
            if (sourceNode.Name == targetNode.Name)
            {
                return;
            }

            var predecessorNodes = this.GetShortestPathPredecessors(targetNode);

            foreach (var predecessor in predecessorNodes)
            {
                var node = new DistanceNode(predecessor.Name)
                {
                    Tag = predecessor.Tag,
                };

                if (!graph.TryAddNode(node))
                {
                    node = (DistanceNode)graph.GetNode(node.Name);
                }

                var distance = _distances[predecessor.Name];
                node.Distance = distance;
                _graph.TryGetEdge(predecessor, targetNode, out var edge);

                if (graph.TryAddEdge(predecessor.Name, targetNode.Name, edge.Weight, false, out _))
                {
                    GetShortestGraph(sourceNode, predecessor, graph);
                }
            }
        }

        /// <summary>
        /// The shortest path predecessors are all nodes which have the smallest sum of predecessorNode.DistanceFromStart + connectionToThisNode.Distance.
        /// </summary>
        private IEnumerable<DirectedGraphNode> GetShortestPathPredecessors(DirectedGraphNode node)
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

        private double GetStartDistanceToMe(DirectedGraphNode predecessor, DirectedGraphNode successor)
        {
            var predecessorDistanceFromStart = _distances[predecessor.Name];

            var predecessorDistanceToMe = _graph.GetEdge(predecessor, successor).Weight;

            var startDistanceToMe = predecessorDistanceFromStart + predecessorDistanceToMe;

            return startDistanceToMe;
        }
    }
}