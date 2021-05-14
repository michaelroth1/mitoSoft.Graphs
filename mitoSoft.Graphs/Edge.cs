﻿using System;
using System.Diagnostics;

namespace mitoSoft.Graphs
{
    [DebuggerDisplay(nameof(Edge) + " ({ToString()})")]
    public class Edge
    {
        public Edge(Node source, Node target)
        {
            this.Source = source ?? throw new ArgumentNullException(nameof(source));

            this.Target = target ?? throw new ArgumentNullException(nameof(target));

            this.Description = string.Empty;
        }

        public Node Source { get; }

        public Node Target { get; }

        public string Description { get; set; }

        public string Name => $"{this.Source.Name} -> {this.Target.Name}";

        public override string ToString() => $"{this.Source.Name} -> {this.Target.Name}";
    }
}