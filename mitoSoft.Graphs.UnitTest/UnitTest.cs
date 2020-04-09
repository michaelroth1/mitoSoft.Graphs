using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mitoSoft.Graphs.Dijkstra
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void ShortestDistance_Start_Middle1_Middle2_End()
        {
            var graph = new DistanceGraph();

            graph.AddNode("Start");
            graph.AddNode("Middle1");
            graph.AddNode("Middle2");
            graph.AddNode("End");

            graph.AddConnection("Start", "End", 5, true);
            graph.AddConnection("Start", "Middle1", 1, true);
            graph.AddConnection("Middle1", "Middle2", 1, true);
            graph.AddConnection("Middle2", "End", 1, true);

            var shortesGraph = (new DeepFirstAlgorithm(graph)).GetShortestGraph("Start", "End");

            shortesGraph.TryGetNode("End", out var endNode);

            Assert.AreEqual(3, endNode.DistanceFromStart);
            Assert.AreEqual(4, shortesGraph.Nodes.Count());
            Assert.AreEqual(3, shortesGraph.Connections.Count());
        }

        [TestMethod]
        public void ShortestDistance_Start_End()
        {
            var graph = new DistanceGraph();

            graph.AddNode("Start");
            graph.AddNode("Middle1");
            graph.AddNode("Middle2");
            graph.AddNode("End");

            graph.AddConnection("Start", "End", 2, true);
            graph.AddConnection("Start", "Middle1", 1, true);
            graph.AddConnection("Middle1", "Middle2", 1, true);
            graph.AddConnection("Middle2", "End", 1, true);

            var shortesGraph = (new DeepFirstAlgorithm(graph)).GetShortestGraph("Start", "End");

            shortesGraph.TryGetNode("End", out var endNode);

            Assert.AreEqual(2, endNode.DistanceFromStart);
            Assert.AreEqual(2, shortesGraph.Nodes.Count());
            Assert.AreEqual(1, shortesGraph.Connections.Count());
        }

        [TestMethod]
        public void ShortestDistance_Start_Middle2_End()
        {
            var graph = new DistanceGraph();

            graph.AddNode("Start");
            graph.AddNode("Middle1");
            graph.AddNode("Middle2");
            graph.AddNode("End");

            graph.AddConnection("Start", "Middle1", 3, true);
            graph.AddConnection("Start", "Middle2", 1, true);
            graph.AddConnection("Middle1", "End", 1, true);
            graph.AddConnection("Middle2", "End", 1, true);

            var shortesGraph = (new DeepFirstAlgorithm(graph)).GetShortestGraph("Start", "End");

            shortesGraph.TryGetNode("End", out var endNode);

            Assert.AreEqual(2, endNode.DistanceFromStart);
            Assert.AreEqual(3, shortesGraph.Nodes.Count());
            Assert.AreEqual(2, shortesGraph.Connections.Count());
            Assert.AreEqual("Start -> Middle2 (Distance: 1)", shortesGraph.TryGetConnector("Start", "Middle2").ToString());
            Assert.AreEqual("Middle2 -> End (Distance: 1)", shortesGraph.TryGetConnector("Middle2", "End").ToString());
        }
    }
}