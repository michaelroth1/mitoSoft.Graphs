﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mitoSoft.Graphs.GraphVizInterop;
using mitoSoft.Graphs.Analysis;
using mitoSoft.Graphs.Analysis.Exceptions;

namespace mitoSoft.Graphs.Tests.NetCore
{
    [TestClass]
    public partial class DijkstraTests
    {
        [TestCategory("Basic")]
        [TestMethod]
        public void SinglePath1()
        {
            var graph = new DirectedGraph();

            graph.AddNode("Start");
            graph.AddNode("Middle1");
            graph.AddNode("Middle2");
            graph.AddNode("End");

            graph.TryAddEdge("End", "Start", 5, true, out _); //switches the target and source of the edge in this test
            graph.TryAddEdge("Start", "Middle1", 1, true, out _);
            graph.TryAddEdge("Middle1", "Middle2", 1, true, out _);
            graph.TryAddEdge("Middle2", "End", 1, true, out _);

            var shortestGraph = (new DijkstraAlgorithm(graph)).GetShortestGraph("Start", "End");

            var endNode = shortestGraph.GetDistanceNode("End");

            Assert.AreEqual(3, endNode.Distance);
            Assert.AreEqual(4, shortestGraph.Nodes.Count());
            Assert.AreEqual(3, shortestGraph.Edges.Count());
        }

        [TestCategory("Basic")]
        [TestMethod]
        public void DoublePathsEquallyWeighted()
        {
            var graph = new DirectedGraph()
                .AddNode("Start")
                .AddNode("Middle1")
                .AddNode("Middle2")
                .AddNode("End")
                .AddEdge("Start", "Middle1", 1, false)
                .AddEdge("Start", "Middle2", 1, false)
                .AddEdge("Middle1", "End", 1, false)
                .AddEdge("Middle2", "End", 1, false);

            var shortestGraph = (new DijkstraAlgorithm(graph)).GetShortestGraph("Start", "End");
            var endNode = shortestGraph.GetDistanceNode("End");

            Assert.AreEqual(2, endNode.Distance);

            var dotText = shortestGraph.ToDotText();

            Assert.IsTrue(dotText.Contains("Start -> Middle1"));
            Assert.IsTrue(dotText.Contains("Start -> Middle2"));
            Assert.IsTrue(dotText.Contains("Middle1 -> End"));
            Assert.IsTrue(dotText.Contains("Middle2 -> End"));
        }

        [TestCategory("Basic")]
        [TestMethod]
        public void DoublePathsDifferentWeights()
        {
            var graph = new DirectedGraph()
                .AddNode("Start")
                .AddNode("Middle1")
                .AddNode("Middle2")
                .AddNode("End")
                .AddEdge("Start", "Middle1", 2, false)
                .AddEdge("Start", "Middle2", 1, false)
                .AddEdge("Middle1", "End", 1, false)
                .AddEdge("Middle2", "End", 2, false);

            var shortestGraph = (new DijkstraAlgorithm(graph)).GetShortestGraph("Start", "End");
            var endNode = shortestGraph.GetDistanceNode("End");

            Assert.AreEqual(3, endNode.Distance);

            var dotText = shortestGraph.ToDotText();

            Assert.IsTrue(dotText.Contains("Start -> Middle1"));
            Assert.IsTrue(dotText.Contains("Start -> Middle2"));
            Assert.IsTrue(dotText.Contains("Middle1 -> End"));
            Assert.IsTrue(dotText.Contains("Middle2 -> End"));
        }

        [TestCategory("Basic")]
        [TestMethod]
        public void Circular()
        {
            var graph = new DirectedGraph()
                .AddNode("Start")
                .AddNode("Middle1")
                .AddNode("Middle2")
                .AddNode("End")
                .AddEdge("Start", "Middle1", 2, false)
                .AddEdge("Start", "Middle2", 1, false)
                .AddEdge("Start", "Start", 1, false)
                .AddEdge("Middle1", "End", 1, false)
                .AddEdge("Middle2", "End", 2, false);

            var shortestGraph = (new DijkstraAlgorithm(graph)).GetShortestGraph("Start", "End");
            var endNode = shortestGraph.GetDistanceNode("End");

            Assert.AreEqual(3, endNode.Distance);

            var dotText = shortestGraph.ToDotText();

            Assert.AreEqual(4, shortestGraph.Edges.Count());
            Assert.IsTrue(dotText.Contains("Start -> Middle1"));
            Assert.IsTrue(dotText.Contains("Start -> Middle2"));
            Assert.IsTrue(dotText.Contains("Middle1 -> End"));
            Assert.IsTrue(dotText.Contains("Middle2 -> End"));
        }

        [TestCategory("Basic")]
        [TestMethod]
        public void SinglePath2()
        {
            var graph = new DirectedGraph();

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
            var graph = new DirectedGraph();

            graph.AddEdge("Start", "Middle1", 1, false);
            graph.AddEdge("Middle1", "Middle2", 1, false);
            graph.AddEdge("Middle2", "End", 1, false);

            var _ = (new DijkstraAlgorithm(graph)).GetShortestGraph("End", "Start");
        }

        [TestCategory("Basic")]
        [TestMethod]
        public void SinglePath3()
        {
            var graph = new DirectedGraph()
                .AddNode("Start")
                .AddNode("Middle1")
                .AddNode("Middle2")
                .AddNode("End")
                .AddEdge("Start", "Middle1", 3, true)
                .AddEdge("Start", "Middle2", 1, true)
                .AddEdge("Middle1", "End", 1, true)
                .AddEdge("Middle2", "End", 1, true);

            var shortestGraph = (new DijkstraAlgorithm(graph)).GetShortestGraph("Start", "End");

            var endNode = shortestGraph.GetDistanceNode("End");

            Assert.AreEqual(2, endNode.Distance);
            Assert.AreEqual(3, shortestGraph.Nodes.Count());
            Assert.AreEqual(2, shortestGraph.Edges.Count());
            Assert.AreEqual("Start -> Middle2 (Weight: 1)", shortestGraph.GetEdge("Start", "Middle2").ToString());
            Assert.AreEqual("Middle2 -> End (Weight: 1)", shortestGraph.GetEdge("Middle2", "End").ToString());
        }

        [TestCategory("Basic")]
        [TestMethod]
        public void SinglePath4()
        {
            var graph = new DirectedGraph()
                .AddEdge("Start", "Middle1", 1, false)
                .AddEdge("Start", "Middle2", 1, false)
                .AddEdge("Middle1", "End", 1, false)
                .AddEdge("Middle2", "End", 2, false);

            var shortestGraph = (new DijkstraAlgorithm(graph)).GetShortestGraph("Start", "End");
            var endNode = shortestGraph.GetDistanceNode("End");

            Assert.AreEqual(2, endNode.Distance);

            var dotText = shortestGraph.ToDotText();

            Assert.IsTrue(dotText.Contains("Start -> Middle1"));
            Assert.IsFalse(dotText.Contains("Start -> Middle2"));
            Assert.IsTrue(dotText.Contains("Middle1 -> End"));
            Assert.IsFalse(dotText.Contains("Middle2 -> End"));
        }
    }
}