using Microsoft.VisualStudio.TestTools.UnitTesting;
using mitoSoft.Graphs.Analysis;
using mitoSoft.Graphs.Analysis.Exceptions;
using System.Linq;

namespace mitoSoft.Graphs.UnitTests
{
    [TestClass]
    public partial class ShortestPathTests
    {
        [TestCategory("Basic")]
        [TestMethod]
        public void SinglePath1()
        {
            var graph = new Graph();

            graph.AddNode("Start");
            graph.AddNode("Middle1");
            graph.AddNode("Middle2");
            graph.AddNode("End");

            graph.TryAddEdge("End", "Start", 5, true, out _); //switches the target and source of the edge in this test
            graph.TryAddEdge("Start", "Middle1", 1, true, out _);
            graph.TryAddEdge("Middle1", "Middle2", 1, true, out _);
            graph.TryAddEdge("Middle2", "End", 1, true, out _);

            var shortestGraph = (new DeepFirstAlgorithm(graph)).GetShortestGraph("Start", "End");

            var endNode = shortestGraph.GetDistanceNode("End");

            Assert.AreEqual(3, endNode.Distance);
            Assert.AreEqual(4, shortestGraph.Nodes.Count());
            Assert.AreEqual(3, shortestGraph.Edges.Count());
        }

        [TestCategory("Basic")]
        [TestMethod]
        public void SinglePath2()
        {
            var graph = new Graph();

            graph.AddNode("Start");
            graph.AddNode("Middle1");
            graph.AddNode("Middle2");
            graph.AddNode("End");

            graph.TryAddEdge("Start", "End", 2, true, out _);
            graph.TryAddEdge("Start", "Middle1", 1, true, out _);
            graph.TryAddEdge("Middle1", "Middle2", 1, true, out _);
            graph.TryAddEdge("Middle2", "End", 1, true, out _);

            var shortestGraph = (new DeepFirstAlgorithm(graph)).GetShortestGraph("Start", "End");

            var endNode = shortestGraph.GetDistanceNode("End");

            Assert.AreEqual(2, ((DistanceNode)endNode).Distance);
            Assert.AreEqual(2, shortestGraph.Nodes.Count());
            Assert.AreEqual(1, shortestGraph.Edges.Count());
        }

        [TestCategory("Basic")]
        [TestMethod]
        [ExpectedException(typeof(PathNotFoundException))]
        public void ImpossiblePath()
        {
            var graph = new Graph()
               .AddEdge("Start", "Middle1", 1, false)
               .AddEdge("Middle1", "Middle2", 1, false)
               .AddEdge("Middle2", "End", 1, false);

            var _ = graph.ToShortestGraph("End", "Start");
        }

        [TestCategory("Basic")]
        [TestMethod]
        public void SinglePath3()
        {
            var shortestGraph = new Graph()
                .AddNode("Start")
                .AddNode("Middle1")
                .AddNode("Middle2")
                .AddNode("End")
                .AddEdge("Start", "Middle1", 3, true)
                .AddEdge("Start", "Middle2", 1, true)
                .AddEdge("Middle1", "End", 1, true)
                .AddEdge("Middle2", "End", 1, true)
                .ToShortestGraph("Start", "End");

            var endNode = shortestGraph.GetDistanceNode("End");

            Assert.AreEqual(2, endNode.Distance);
            Assert.AreEqual(3, shortestGraph.Nodes.Count());
            Assert.AreEqual(2, shortestGraph.Edges.Count());
            Assert.AreEqual("Start -> Middle2 (Weight: 1)", shortestGraph.GetEdge("Start", "Middle2").ToString());
            Assert.AreEqual("Middle2 -> End (Weight: 1)", shortestGraph.GetEdge("Middle2", "End").ToString());
        }
    }
}