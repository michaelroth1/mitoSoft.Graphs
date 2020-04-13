using System;
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

        public IEnumerable<GraphNode> Predecessors => this._edges.Where(e => ReferenceEquals(e.TargetNode, this))
                                                                 .Select(e => e.SourceNode)
                                                                 .Concat(this.Edges.Where(e => e is BidirectionalEdge && ReferenceEquals(e.SourceNode, this))
                                                                                   .Select(e => e.TargetNode));

        public IEnumerable<GraphNode> Successors => this._edges.Where(e => ReferenceEquals(e.SourceNode, this))
                                                               .Select(e => e.TargetNode)
                                                               .Concat(this.Edges.Where(e => e is BidirectionalEdge && ReferenceEquals(e.TargetNode, this))
                                                                                 .Select(e => e.TargetNode));

        internal void AddEdge(GraphNode targetNode, double weight, bool bidirection)
        {
            if (targetNode == null)
            {
                throw new ArgumentNullException(nameof(targetNode));
            }
            else if (weight <= 0)
            {
                throw new ArgumentException("Weight must be positive.");
            }

            GraphEdge edge;
            if (bidirection)
            {
                edge = new BidirectionalEdge(this, targetNode, weight);
            }
            else
            {
                edge = new GraphEdge(this, targetNode, weight);
            }

            this._edges.Add(edge);

            targetNode.AddEdge(edge);
        }

        internal void AddEdge(GraphEdge edge)
        {
            _edges.Add(edge);
        }

        public override string ToString() => $"{this.Name} (Edges: {this._edges.Count})";
    }
}