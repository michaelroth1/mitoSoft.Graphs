﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mitoSoft.Graphs
{
    [DebuggerDisplay(nameof(GraphNode) + " ({ToString()})")]
    public class GraphNode
    {
        private readonly IList<GraphEdge> _edges;
        
        public object Tag { get; set; }
        
        public GraphNode(string name)
        {
            this._edges = new List<GraphEdge>();

            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public GraphNode(string name, string description) : this(name)
        {
            this.Description = description;
        }

        public string Name { get; }

        public string Description { get; set; }

        public IEnumerable<GraphEdge> Edges => this._edges;

        public IEnumerable<GraphNode> Predecessors => this._edges.Where(c => ReferenceEquals(c.TargetNode, this)).Select(c => c.SourceNode);

        public IEnumerable<GraphNode> Successors => this._edges.Where(c => ReferenceEquals(c.SourceNode, this)).Select(c => c.TargetNode);

        internal void AddEdge(GraphNode targetNode, double weight, bool twoWay)
        {
            if (targetNode == null)
            {
                throw new ArgumentNullException(nameof(targetNode));
            }
            else if (weight <= 0)
            {
                throw new ArgumentException("Weight must be positive.");
            }

            var edge = new GraphEdge(this, targetNode, weight);

            this._edges.Add(edge);

            targetNode.AddConnection(edge);

            if (twoWay)
            {
                targetNode.AddEdge(this, weight, false);
            }
        }

        internal void AddConnection(GraphEdge edge)
        {
            _edges.Add(edge);
        }

        public override string ToString() => $"{this.Name} (Edges: {this._edges.Count})";
    }
}