using System.Collections.Generic;
using System.Diagnostics;

namespace mitoSoft.Graphs.Dijkstra
{
    [DebuggerDisplay(nameof(Steps) + " ({ToString()})")]
    public sealed class Steps
    {
        private readonly List<Step> _steps;

        internal Steps()
        {
            this._steps = new List<Step>();
        }

        private Steps(List<Step> previous, Step next)
        {
            this._steps = new List<Step>(previous)
            {
                next,
            };
        }

        public int Degree => this._steps.Count;

        public IEnumerable<Step> GetSteps()
        {
            foreach (var step in this._steps)
            {
                yield return step;
            }
        }

        public override string ToString() => $"Degree: {this.Degree}";

        internal Steps Add(DistanceNode left, DistanceNode right) => new Steps(this._steps, new Step(left, right));
    }
}
