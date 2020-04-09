using System;
using System.Diagnostics;

namespace mitoSoft.Graphs
{
    [DebuggerDisplay(nameof(GraphNodeKey) + " ({ToString()})")]
    public sealed class GraphNodeKey : GraphNodeKeyBase
    {
        private readonly string _name;

        private readonly int _hashCode;

        public GraphNodeKey(string name)
        {
            this._name = name ?? throw new ArgumentNullException(nameof(name));

            this._hashCode = name.GetHashCode();
        }

        public override string GetKeyDisplayValue() => this._name;

        public override int GetKeyHashCode() => this._hashCode;

        public override bool KeysAreEqual(GraphNodeKeyBase other)
        {
            if (!(other is GraphNodeKey gnk))
            {
                return false;
            }

            var areEqual = this._name.Equals(gnk._name);

            return areEqual;
        }

        public override string ToString() => this.GetKeyDisplayValue();
    }
}