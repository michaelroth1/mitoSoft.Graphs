using Microsoft.VisualStudio.TestTools.UnitTesting;
using mitoSoft.Graphs.Analysis;
using mitoSoft.Graphs.Exceptions;
using System.Linq;

namespace mitoSoft.Graphs.Tests.NetCore
{
    [TestClass]
    public partial class GraphTests
    {
        [TestCategory("Basic")]
        [TestMethod]
        public void BuildAcyclicGraphViaStrings()
        {
            var graph = new DirectedGraph();

            graph.AddNode("Start");
            graph.AddNode("Middle");
            graph.AddNode("End");

            graph.AddEdge("Start", "Middle", 1, false);
            graph.AddEdge("Middle", "End", 1, false);

            Assert.AreEqual(3, graph.Nodes.Count());
            Assert.AreEqual(2, graph.Edges.Count());
        }

        [TestCategory("Basic")]
        [TestMethod]
        public void BuildCyclicGraphViaStrings()
        {
            var graph = new DirectedGraph();

            graph.AddNode("Start");
            graph.AddNode("Middle");
            graph.AddNode("End");

            graph.AddEdge("Start", "Middle", 1, false);
            graph.AddEdge("Middle", "Middle", 1, false);
            graph.AddEdge("Middle", "End", 1, false);

            Assert.AreEqual(3, graph.Edges.Count());
        }

        [TestCategory("Basic")]
        [TestMethod]
        public void BuildCyclicGraphViaObjects()
        {
            var graph = new DirectedGraph();

            var start = new DirectedGraphNode("Start");
            var middle = new DirectedGraphNode("Middle");
            var end = new DirectedGraphNode("End");

            graph.AddNode(start);
            graph.AddNode(middle);
            graph.AddNode(end);

            graph.AddEdge(start, middle, 1, false);
            graph.AddEdge(middle, middle, 1, false);
            graph.AddEdge(middle, end, 1, false);

            Assert.AreEqual(3, graph.Edges.Count());
        }

        [TestCategory("Basic")]
        [TestMethod]
        [ExpectedException(typeof(EdgeAlreadyExistingException))]
        public void AlreadyExistingDirectedEdge()
        {
            var graph = new DirectedGraph();

            var start = new DirectedGraphNode("Start");
            var middle = new DirectedGraphNode("Middle");
            var end = new DirectedGraphNode("End");

            graph.AddNode(start);
            graph.AddNode(middle);
            graph.AddNode(end);

            graph.AddEdge(start, middle, 1, false);
            graph.AddEdge(middle, middle, 1, false);
            graph.AddEdge(middle, middle, 1, false);
            graph.AddEdge(middle, end, 1, false);

            Assert.AreEqual(3, graph.Edges.Count());
        }

        [TestCategory("Basic")]
        [TestMethod]
        public void ComplementaryDirectedEdges()
        {
            var graph = new DirectedGraph();

            var start = new DirectedGraphNode("Start");
            var middle = new DirectedGraphNode("Middle");
            var end = new DirectedGraphNode("End");

            graph.AddNode(start);
            graph.AddNode(middle);
            graph.AddNode(end);

            graph.AddEdge(start, middle, 1, false);
            graph.AddEdge(middle, start, 1, false);
            graph.AddEdge(middle, end, 1, false);

            Assert.AreEqual(3, graph.Edges.Count());
        }

        [TestCategory("Basic")]
        [TestMethod]
        [ExpectedException(typeof(EdgeAlreadyExistingException))]
        public void AlreadyExistingBidirectedEdge()
        {
            var graph = new DirectedGraph();

            var start = new DirectedGraphNode("Start");
            var middle = new DirectedGraphNode("Middle");
            var end = new DirectedGraphNode("End");

            graph.AddNode(start);
            graph.AddNode(middle);
            graph.AddNode(end);

            graph.AddEdge(start, middle, 1, true);
            graph.AddEdge(middle, start, 1, true);
            graph.AddEdge(middle, end, 1, false);

            Assert.AreEqual(3, graph.Edges.Count());
        }
    }
}