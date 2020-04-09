using System;
using System.Diagnostics;

namespace mitoSoft.Graphs
{
    [DebuggerDisplay(nameof(GraphEdgeKey) + " ({ToString()})")]
    public sealed class GraphEdgeKey
    {
        private readonly int _hashCode;

        internal GraphEdgeKey(GraphNodeKeyBase sourceNodeKey, GraphNodeKeyBase targetNodeKey)
        {
            this.SourceNodeKey = sourceNodeKey ?? throw new ArgumentNullException(nameof(sourceNodeKey));
            this.TargetNodeKey = targetNodeKey ?? throw new ArgumentNullException(nameof(targetNodeKey));

            this._hashCode = sourceNodeKey.GetHashCode() ^ targetNodeKey.GetHashCode();
        }

        public GraphNodeKeyBase SourceNodeKey { get; }

        public GraphNodeKeyBase TargetNodeKey { get; }

        public override int GetHashCode() => this._hashCode;

        public override bool Equals(object obj)
        {
            if (!(obj is GraphEdgeKey other))
            {
                return false;
            }
            else
            {
                var areEqual = this.SourceNodeKey.KeysAreEqual(other.SourceNodeKey)
                    && this.TargetNodeKey.KeysAreEqual(other.TargetNodeKey);

                return areEqual;
            }
        }

        public override string ToString() => $"{this.SourceNodeKey.GetKeyDisplayValue()} -> {this.TargetNodeKey.GetKeyDisplayValue()}";
    }
}