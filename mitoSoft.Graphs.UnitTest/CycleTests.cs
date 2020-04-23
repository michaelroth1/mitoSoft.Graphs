using Microsoft.VisualStudio.TestTools.UnitTesting;
using mitoSoft.Graphs.GraphVizInterop;
using mitoSoft.Graphs.Analysis;
using System.IO;

namespace mitoSoft.Graphs.UnitTests
{
    [TestClass]
    public class CycleTests
    {
        private static Graph _graph;

        [ClassInitialize]
        public static void Initialize(TestContext _)
        {
            var graphText = File.ReadLines(Path.Combine("TestGraphs", "LargeGraph.txt"));

            _graph = GraphGenerator.FromDotText(graphText);
        }

        /// <summary>
        /// Test to test the included 'dot' layout engine by
        /// using it directly.
        /// </summary>
        [TestCategory("Cycle")]
        [TestMethod]
        public void SmallAcyclic()
        {
            var graph = new Graph()
                .AddEdge("Start", "Middle1", 1, false)
                .AddEdge("Middle1", "Middle2", 1, false)
                .AddEdge("Middle2", "End", 1, false);

            var hasCycle = (new CycleChecker(graph)).IsCyclic();

            Assert.IsFalse(hasCycle);
        }

        /// <summary>
        /// Test to test the included 'dot' layout engine by
        /// using it directly.
        /// </summary>
        [TestCategory("Cycle")]
        [TestMethod]
        public void DirectCycle()
        {
            var graph = new Graph()
                .AddEdge("Start", "Middle1", 1, true)
                .AddEdge("Middle1", "Middle2", 1, false)
                .AddEdge("Middle2", "End", 1, false);

            var hasCycle = (new CycleChecker(graph)).IsCyclic();

            Assert.IsTrue(hasCycle);
        }

        [TestCategory("Cycle")]
        [TestMethod]
        public void Cyclic()
        {
            var graph = new Graph()
                .AddEdge("Start", "Middle1", 1, false)
                .AddEdge("Middle1", "Middle2", 1, false)
                .AddEdge("Middle2", "End", 1, false)
                .AddEdge("End", "Start", 1, false);

            var hasCycle = (new CycleChecker(graph)).IsCyclic();

            Assert.IsTrue(hasCycle);
        }

        [TestCategory("Cycle")]
        [TestMethod]
        public void LargeCyclic()
        {
            var hasCycle = (new CycleChecker(_graph)).IsCyclic();

            Assert.IsTrue(hasCycle);
        }

        [TestCategory("Cycle")]
        [TestMethod]
        public void LargeAcyclic()
        {
            var acyclicGraph = _graph.ToShortestGraph("Actor:Eric Elmosnino(1964)", "Actor:Libuse Safránková(1953)");

            var hasCycle = (new CycleChecker(acyclicGraph)).IsCyclic();

            Assert.IsFalse(hasCycle);
        }
    }
}