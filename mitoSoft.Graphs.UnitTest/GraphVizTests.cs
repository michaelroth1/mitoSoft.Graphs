using Microsoft.VisualStudio.TestTools.UnitTesting;
using mitoSoft.Graphs.GraphVizInterop;
using mitoSoft.Graphs.ShortestPathAlgorithms;
using System.Collections.Generic;
using System.Linq;

namespace mitoSoft.Graphs.UnitTests
{
    [TestClass]
    public class GraphVizTests
    {
        private const string GraphVizBinPath = @"C:\Temp\Graphviz\bin";

        /// <summary>
        /// This test generates a dot-text out of a mitoSoft shortest-path graph.
        /// </summary>
        [TestMethod]
        public void GenerateDotTextFromDistanceGraph()
        {
            var text = new DistanceGraph()
                .AddNode("Start")
                .AddNode("Middle1")
                .AddNode("Middle2")
                .AddNode("End")
                .AddEdge("Start", "End", 2d, true)
                .AddEdge("Start", "Middle1", 1d, true)
                .AddEdge("Middle1", "Middle2", 1d, true)
                .AddEdge("Middle2", "End", 1, true)
                .ToDotText();

            Assert.IsTrue(text.Contains("Start ["));
            Assert.IsTrue(text.Contains("Middle1 ["));
            Assert.IsTrue(text.Contains("Middle2 ["));
            Assert.IsTrue(text.Contains("End ["));
            Assert.IsTrue(text.Contains("Start -> End"));
            Assert.IsTrue(text.Contains("Start -> Middle1"));
            Assert.IsTrue(text.Contains("Middle1 -> Middle2"));
            Assert.IsTrue(text.Contains("Middle2 -> End"));
        }

        /// <summary>
        /// This test tries to generate a 'System.Drawing' image out of an 
        /// mitoSoft shortest-path graph.
        /// </summary>
        [TestMethod]
        public void GenerateImage()
        {
            var graph = new DistanceGraph();

            graph.AddNode("Start");
            graph.AddNode("Middle1");
            graph.AddNode("Middle2");
            graph.AddNode("End");

            graph.TryAddEdge("Start", "End", 2, true);
            graph.TryAddEdge("Start", "Middle1", 1, true);
            graph.TryAddEdge("Middle1", "Middle2", 1, true);
            graph.TryAddEdge("Middle2", "End", 1, true);

            var image = graph.ToPng(GraphVizBinPath);

            Assert.IsNotNull(image);
        }

        /// <summary>
        /// This test tries to generates a dot-text out of a mitoSoft standard graph.
        /// </summary>
        [TestMethod]
        public void GenerateDotTextFromGraph()
        {
            var graph = new Graph()
                .AddEdge("Start", "End", 2, true)
                .AddEdge("Start", "Middle1", 1, true)
                .AddEdge("Middle1", "Middle2", 1, true)
                .AddEdge("Middle2", "End", 1, true);

            var text = graph.ToDotText();

            Assert.IsTrue(text.Contains("Start ["));
            Assert.IsTrue(text.Contains("Middle1 ["));
            Assert.IsTrue(text.Contains("Middle2 ["));
            Assert.IsTrue(text.Contains("End ["));
            Assert.IsTrue(text.Contains("Start -> End"));
            Assert.IsTrue(text.Contains("Start -> Middle1"));
            Assert.IsTrue(text.Contains("Middle1 -> Middle2"));
            Assert.IsTrue(text.Contains("Middle2 -> End"));
        }

        /// <summary>
        /// Use the dot-text to Generate a mitsSoft graph, translate it back
        /// into a dot-text and finally genearte an image out of it. 
        /// </summary>
        [TestMethod]
        public void GenerateGraph()
        {
            var dotText = new List<string>()
            {
                "digraph D {",
                "Start ->    Middle1",
                "Middle1 ->    Start",
                "Start     -> End [label=\"2\"]",
                "Middle2->End [label=\"Test\"]",
                "Middle1 -> Middle2 [label=\"2\"]",
                "}",
            };

            var graph = GraphGenerator.FromDotText(dotText);

            Assert.AreEqual(4, graph.Nodes.Count());
            Assert.AreEqual(5, graph.Edges.Count());

            var text = graph.ToDotText();

            Assert.IsTrue(text.Contains("Start -> Middle1"));
            Assert.IsTrue(text.Contains("Middle1 -> Start"));
            Assert.IsTrue(text.Contains("Start -> End"));
            Assert.IsTrue(text.Contains("Middle2 -> End"));
            Assert.IsTrue(text.Contains("Middle1 -> Middle2"));

            var image = graph.ToPng(GraphVizBinPath);

            Assert.IsNotNull(image);
        }


        /// <summary>
        /// Test to test the included 'dot' layout engine by
        /// using it directly.
        /// </summary>
        [TestMethod]
        public void DirectImageGeneration()
        {
            var dotText = new List<string>()
            {
                "digraph D {",
                "  Start ->    Middle1",
                "  Middle1 ->    Start",
                "  Start     -> End [label=\"2\"]",
                "  Middle2->End [label=\"Test\"]",
                "  Middle1 -> Middle2 [label=\"2\"]",
                "}",
            };

            var image = (new ImageRenderer(GraphVizBinPath)).RenderImage(dotText);

            Assert.IsNotNull(image);
        }
    }
}