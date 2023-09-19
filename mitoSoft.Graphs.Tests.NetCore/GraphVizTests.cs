using Microsoft.VisualStudio.TestTools.UnitTesting;
using mitoSoft.Graphs.Exceptions;
using mitoSoft.Graphs.GraphVizInterop;
using mitoSoft.Graphs.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace mitoSoft.Graphs.Tests.NetCore
{
    [TestClass]
    public class GraphVizTests
    {
        internal const string GraphVizPath = @"C:\Temp\Graphviz\bin";

        /// <summary>
        /// This test generates a dot-text out of a mitoSoft shortest-path graph.
        /// </summary>
        [TestCategory("GrapVizInterop")]
        [TestMethod]
        public void GenerateDotTextFromDistanceGraph()
        {
            var text = new DirectedGraph()
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
        [TestCategory("GrapVizInterop")]
        [TestMethod]
        public void GenerateImage()
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

            var image = graph.ToImage(GraphVizPath);

            Assert.IsNotNull(image);
        }

        /// <summary>
        /// This test tries to generate a 'System.Drawing' image out of an 
        /// mitoSoft shortest-path graph.
        /// </summary>
        [TestCategory("GrapVizInterop")]
        [TestMethod]
        public void GenerateImageWithoutPath()
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

            var image = graph.ToImage();

            Assert.IsNotNull(image);
        }

        /// <summary>
        /// This test tries to generate an image file out of an 
        /// mitoSoft shortest-path graph.
        /// </summary>
        [TestCategory("GrapVizInterop")]
        [TestMethod]
        public void GenerateImageFile()
        {
            var graph = new DirectedGraph();

            graph.TryAddEdge("Start", "End", 2, true, out _);
            graph.TryAddEdge("Start", "Middle1", 1, true, out _);
            graph.TryAddEdge("Middle1", "Middle2", 1, true, out _);
            graph.TryAddEdge("Middle2", "End", 1, true, out _);

            var imageFile = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "GraphImages", "Graph1.png");
            graph.ToImageFile(imageFile, GraphVizPath);

            Assert.IsTrue(File.Exists(imageFile));
        }

        /// <summary>
        /// This test tries to generates a dot-text out of a mitoSoft standard graph.
        /// </summary>
        [TestCategory("GrapVizInterop")]
        [TestMethod]
        public void GenerateDotTextFromGraph()
        {
            var graph = new DirectedGraph()
                .AddEdge("Start", "End", 2, true)
                .AddEdge("Start", "Middle1", 1, true)
                .AddEdge("Middle1", "Middle2", 1, true)
                .AddEdge("Middle2", "End", 1, true);

            var dotText = graph.ToDotText();

            Assert.IsTrue(dotText.Contains("Start ["));
            Assert.IsTrue(dotText.Contains("Middle1 ["));
            Assert.IsTrue(dotText.Contains("Middle2 ["));
            Assert.IsTrue(dotText.Contains("End ["));
            Assert.IsTrue(dotText.Contains("Start -> End"));
            Assert.IsTrue(dotText.Contains("Start -> Middle1"));
            Assert.IsTrue(dotText.Contains("Middle1 -> Middle2"));
            Assert.IsTrue(dotText.Contains("Middle2 -> End"));
        }

        /// <summary>
        /// Use the dot-text to Generate a mitsSoft graph, translate it back
        /// into a dot-text and finally genearte an image out of it. 
        /// </summary>
        [TestCategory("GrapVizInterop")]
        [TestMethod]
        public void GenerateGraphWithBidirectionalEdge()
        {
            var dotText = new List<string>()
            {
                "digraph D {",
                "Start <->    Middle1",
                "Start     -> End [label=\"2\"]",
                "Middle2->End [label=\"Test\"]",
                "Middle1 -> Middle2 [label=\"2\"]",
                "}",
            };

            var graph = GraphGenerator.FromDotText(dotText);

            Assert.AreEqual(4, graph.Nodes.Count());
            Assert.AreEqual(4, graph.Edges.Count());

            var text = graph.ToDotText();

            Assert.IsTrue(text.Contains("Start -> Middle1 [color=black,arrowhead=normal,fontcolor=black,style=solid,label=\"\",decorate=false,dir=\"both\"]"));
            Assert.IsTrue(text.Contains("Start -> End"));
            Assert.IsTrue(text.Contains("Middle2 -> End"));
            Assert.IsTrue(text.Contains("Middle1 -> Middle2"));
        }

        [TestCategory("GrapVizInterop")]
        [ExpectedException(typeof(EdgeAlreadyExistingException))]
        [TestMethod]
        public void GenerateGraphWithDoubleEdges()
        {
            var dotText = new List<string>()
            {
                "digraph D {",
                "Start <->    Middle1",
                "Start ->    Middle1",
                "Start     -> End [label=\"2\"]",
                "Middle2->End [label=\"Test\"]",
                "Middle1 -> Middle2 [label=\"2\"]",
                "}",
            };

            var _ = GraphGenerator.FromDotText(dotText);
        }

        [TestCategory("GrapVizInterop")]
        [TestMethod]
        public void GenerateGraphWithDifferentWeights()
        {
            var dotText = new List<string>()
            {
                "digraph D {",
                "Start ->    Middle1 [weight=1]",
                "Start ->    Middle2 [weight=2]",
                "Middle2     -> End [weight=\"1\", label=\"2\"]",
                "Middle1     -> End [label=\"2\", weight=1]",
                "}",
            };

            var graph = GraphGenerator.FromDotText(dotText);

            Assert.AreEqual(1d, graph.GetEdge("Start", "Middle1").Weight);
            Assert.AreEqual(2d, graph.GetEdge("Start", "Middle2").Weight);
            Assert.AreEqual(1d, graph.GetEdge("Middle1", "End").Weight);
            Assert.AreEqual(1d, graph.GetEdge("Middle2", "End").Weight);

            var shortestGraph = graph.ToShortestGraph("Start", "End");

            Assert.AreEqual(3, shortestGraph.Nodes.Count());
            Assert.AreEqual(2, shortestGraph.Edges.Count());
        }

        /// <summary>
        /// Use the dot-text to Generate a mitsSoft graph, translate it back
        /// into a dot-text and finally genearte an image out of it. 
        /// </summary>
        [TestCategory("GrapVizInterop")]
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

            var image = graph.ToImage(GraphVizPath);

            Assert.IsNotNull(image);
        }

        /// <summary>
        /// Test to test the included 'dot' layout engine by
        /// using it directly.
        /// </summary>
        [TestCategory("GrapVizInterop")]
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

            var image = (new ImageRenderer(GraphVizPath)).RenderImage(dotText);

            Assert.IsNotNull(image);
        }

        /// <summary>
        /// Test to test the included 'dot' layout engine by
        /// using it directly.
        /// </summary>
        [TestCategory("GrapVizInterop")]
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void SyntaxError()
        {
            var dotText = new List<string>()
            {
                "digraph D {",
                "  Start --    Middle1", //here is a systax error in teh dot-text
                "  Middle1 ->    Start",
                "  Start     -> End [label=\"2\"]",
                "  Middle2->End [label=\"Test\"]",
                "  Middle1 -> Middle2 [label=\"2\"]",
                "}",
            };

            var imageFile = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "TestGraphs", "Graph2.png");
            (new ImageRenderer(GraphVizPath)).RenderImage(dotText, imageFile);

            Assert.IsTrue(File.Exists(imageFile));
        }

        /// <summary>
        /// This test tries build a graph with an label,
        /// that includes a illegal character '"'
        /// </summary>
        [TestCategory("GrapVizInterop")]
        [TestMethod]
        public void IllegalLabelCharacter()
        {
            var graph = new DirectedGraph()
                .AddNode("Start")
                .AddEdge("Start", "End", 2, true)
                .AddEdge("Start", "Middle1", 1, true)
                .AddEdge("Middle1", "Middle2", 1, true)
                .AddEdge("Middle2", "End", 1, true);

            graph.Nodes.ToList().ForEach(n => n.Description = $"\"{n.Name}\"");

            var dotText = graph.ToDotText();

            Assert.IsTrue(dotText.Contains("label=\"'Start'\""));
            Assert.IsTrue(dotText.Contains("label=\"'Middle1'\""));
            Assert.IsTrue(dotText.Contains("label=\"'Middle2'\""));
            Assert.IsTrue(dotText.Contains("label=\"'End'\""));
        }

        /// <summary>
        /// This test tries build a graph with illegal
        /// dot characters included in its node names
        /// </summary>
        [TestCategory("GrapVizInterop")]
        [TestMethod]
        public void IllegalNameCharacter()
        {
            var graph = new DirectedGraph()
                .AddNode("Start(19)")
                .AddNode("Middle1%$§")
                .AddNode("Middle12/3")
                .AddNode("End-123\"4\"")
                .AddEdge("Start(19)", "Middle1", 1, false)
                .AddEdge("Middle1%$§", "Middle12/3", 1, false)
                .AddEdge("Middle12/3", "End-123\"4\"", 1, false);

            var dotText = graph.ToDotText();

            Assert.IsTrue(dotText.Contains("Start_19"));
            Assert.IsTrue(dotText.Contains("Middle1"));
            Assert.IsTrue(dotText.Contains("Middle12_3"));
            Assert.IsTrue(dotText.Contains("End_123_4"));
            Assert.IsTrue(dotText.Contains("label=\"Start(19)\""));
            Assert.IsTrue(dotText.Contains("label=\"Middle1%$§\""));
            Assert.IsTrue(dotText.Contains("label=\"Middle12/3\""));
            Assert.IsTrue(dotText.Contains("label=\"End-123'4'\""));

            var image = graph.ToImage(GraphVizPath);

            Assert.IsNotNull(image);

            var imageFile = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "GraphImages", "Graph3.png");
            image.Save(imageFile);
        }
    }
}