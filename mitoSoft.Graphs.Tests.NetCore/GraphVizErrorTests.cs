using Microsoft.VisualStudio.TestTools.UnitTesting;
using mitoSoft.Graphs.Exceptions;

namespace mitoSoft.Graphs.UnitTests
{
    [TestClass]
    public class GraphVizErrorTests
    {
        /// <summary>
        /// This test tries to add an identical edge twice.
        /// This throws an 'EdgeAlreadyExistingException'.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(EdgeAlreadyExistingException))]
        public void EqualEdges()
        {
            var _ = new Graph()
                .AddEdge("Start", "End", 2, true)
                .AddEdge("Start", "End", 2, true)
                .AddEdge("Start", "Middle1", 1, true)
                .AddEdge("Middle1", "Middle2", 1, true)
                .AddEdge("Middle2", "End", 1, true);
        }

        /// <summary>
        /// This test tries to add an identical edge twice.
        /// This throws an 'NodeAlreadyExistingException'.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NodeAlreadyExistingException))]
        public void EqualNodes()
        {
            var _ = new Graph()
                .AddNode("Start")
                .AddNode("Start")
                .AddEdge("Start", "End", 2, true)
                .AddEdge("Start", "Middle1", 1, true)
                .AddEdge("Middle1", "Middle2", 1, true)
                .AddEdge("Middle2", "End", 1, true);
        }

        /// <summary>
        /// This test tries to get a not included node
        /// via the 'Graph.GetNode' method which throws an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NodeNotFoundException))]
        public void InvalidNodeSearchByGetFunction()
        {
            var graph = new Graph()
                .AddNode("Start")
                .AddEdge("Start", "End", 2, true)
                .AddEdge("Start", "Middle1", 1, true)
                .AddEdge("Middle1", "Middle2", 1, true)
                .AddEdge("Middle2", "End", 1, true);

            var _ = graph.GetNode("Hurz");
        }

        /// <summary>
        /// This test tries to get a not included node
        /// via the 'Graph.TryGetNode' method which do not throw an excepton.
        /// </summary>
        [TestMethod]
        public void InvalidNodeSearchByTryFunction()
        {
            var graph = new Graph()
                .AddNode("Start")
                .AddEdge("Start", "End", 2, true)
                .AddEdge("Start", "Middle1", 1, true)
                .AddEdge("Middle1", "Middle2", 1, true)
                .AddEdge("Middle2", "End", 1, true);

            var result = graph.TryGetNode("Hurz", out var node);

            Assert.IsFalse(result);
            Assert.IsNull(node);
        }

        /// <summary>
        /// This test tries to get a not included edge
        /// via the 'Graph.GetEdge' method which throws an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(EdgeNotFoundException))]
        public void InvalidEdgeSearchByGetFunction()
        {
            var graph = new Graph()
                .AddNode("Start")
                .AddEdge("Start", "End", 2, true)
                .AddEdge("Start", "Middle1", 1, true)
                .AddEdge("Middle1", "Middle2", 1, true)
                .AddEdge("Middle2", "End", 1, true);

            var _ = graph.GetEdge("Start", "Hurz");
        }

        /// <summary>
        /// This test tries to get a not included edge
        /// via the 'Graph.TryGetEdge' method which do not throw an excepton.
        /// </summary>
        [TestMethod]
        public void InvalidEdgeSearchByTryFunction()
        {
            var graph = new Graph()
                .AddNode("Start")
                .AddEdge("Start", "End", 2, true)
                .AddEdge("Start", "Middle1", 1, true)
                .AddEdge("Middle1", "Middle2", 1, true)
                .AddEdge("Middle2", "End", 1, true);

            var result = graph.TryGetEdge("Start", "Hurz", out var edge);

            Assert.IsFalse(result);
            Assert.IsNull(edge);
        }
    }
}