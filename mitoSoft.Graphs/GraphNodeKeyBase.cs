using System.Diagnostics;

namespace mitoSoft.Graphs
{
    /// <summary>
    /// Identifies a unique node in the graph.
    /// </summary>
    /// <remarks>
    /// A key object must be immutable, its HashCode and all properties (that are used for equality comparison) never changing.
    /// </remarks>
    [DebuggerDisplay(nameof(GraphNodeKeyBase) + " ({ToString()})")]
    public abstract class GraphNodeKeyBase
    {
        public abstract int GetKeyHashCode();

        public abstract bool KeysAreEqual(GraphNodeKeyBase other);

        public abstract string GetKeyDisplayValue();

        public sealed override bool Equals(object obj) => this.KeysAreEqual(obj as GraphNodeKeyBase);

        public sealed override int GetHashCode() => this.GetKeyHashCode();

        public override string ToString() => this.GetKeyDisplayValue();
    }
}