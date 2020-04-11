using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace mitoSoft.Graphs.UnitTests
{
    [TestClass]
    public class GraphVizErrorTests
    {
        /// <summary>
        /// This test tries to add an identical edge twice
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
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
        /// This test tries to add an identical edge twice
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EqualNodes()
        {
            var _ = new Graph()
                .AddNode ("Start")
                .AddNode ("Start")
                .AddEdge("Start", "End", 2, true)
                .AddEdge("Start", "Middle1", 1, true)
                .AddEdge("Middle1", "Middle2", 1, true)
                .AddEdge("Middle2", "End", 1, true);
        }
    }
}
