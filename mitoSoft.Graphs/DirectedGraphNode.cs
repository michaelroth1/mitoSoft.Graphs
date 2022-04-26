using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mitoSoft.Graphs
{
    [Serializable]
    [DebuggerDisplay(nameof(Node) + " ({ToString()})")]
    public class DirectedGraphNode : Node
    {
        public DirectedGraphNode(string name) : base(name) { }

        public DirectedGraphNode(string name, string description) : base(name, description) { }

        public new IEnumerable<DirectedEdge> Edges => base.Edges.Cast<DirectedEdge>();

        public new IEnumerable<DirectedGraphNode> Predecessors => base.Predecessors.Cast<DirectedGraphNode>();

        public new IEnumerable<DirectedGraphNode> Successors => base.Successors.Cast<DirectedGraphNode>();

        public object Tag { get; set; }
    }
}