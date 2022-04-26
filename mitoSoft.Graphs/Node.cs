using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mitoSoft.Graphs
{
    [Serializable]
    [DebuggerDisplay(nameof(Node) + " ({ToString()})")]
    public abstract class Node
    {
        protected readonly IList<Edge> _edges;

        public Node(string name)
        {
            this._edges = new List<Edge>();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            else
            {
                this.Name = name;
            }
        }

        public Node(string name, string description) : this(name)
        {
            this.Description = description;
        }

        public string Name { get; internal set; }

        public string Description { get; set; }

        public virtual IEnumerable<Edge> Edges => this._edges;

        public virtual IEnumerable<Node> Predecessors => this._edges.Where(e => ReferenceEquals(e.Target, this))
                                                                 .Select(e => e.Source)
                                                                 .Concat(this.Edges.Where(e => e is BidirectedEdge && ReferenceEquals(e.Source, this))
                                                                                   .Select(e => e.Target));

        public virtual IEnumerable<Node> Successors => this._edges.Where(e => ReferenceEquals(e.Source, this))
                                                               .Select(e => e.Target)
                                                               .Concat(this.Edges.Where(e => e is BidirectedEdge && ReferenceEquals(e.Target, this))
                                                                                 .Select(e => e.Target));

        public void AddEdge(Edge edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            this._edges.Add(edge);

            if (!edge.Target.Edges.Contains(edge))
            {
                edge.Target.AddEdge(edge);
            }
        }

        public override string ToString() => $"{this.Name} (Edges: {this._edges.Count})";
    }
}