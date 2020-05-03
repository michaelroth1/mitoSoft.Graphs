using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mitoSoft.Graphs.GraphVizInterop;
using mitoSoft.Graphs.Analysis;

namespace mitoSoft.Graphs.Tests.NetCore
{
    [TestClass]
    public class PathListTests
    {
        [TestMethod]
        public void SimpleList()
        {
            var graph = new Graph()
                .AddEdge("Start", "Middle1", 1, false)
                .AddEdge("Middle1", "Middle2", 1, false)
                .AddEdge("Middle2", "End", 1, false);

            var paths = (new PathAlgorithm(graph)).GetAllPaths("Start");

            Assert.AreEqual(1, paths.Count);
            Assert.AreEqual("Start->Middle1->Middle2->End", paths[0]);
        }

        [TestMethod]
        public void Tree()
        {
            var graph = new Graph()
               .AddEdge("Start", "Middle1", 1, false)
               .AddEdge("Middle1", "End1", 1, false)
               .AddEdge("Start", "Middle2", 1, false)
               .AddEdge("Middle2", "End2", 1, false);

            var paths = graph.GetAllPaths("Start");

            Assert.AreEqual(2, paths.Count);
            Assert.IsTrue(paths.Any(path => path == "Start->Middle1->End1"));
            Assert.IsTrue(paths.Any(path => path == "Start->Middle2->End2"));
        }

        [TestMethod]
        public void Degree8()
        {
            var graphText = File.ReadLines(Path.Combine("TestGraphs", "LargeGraph.txt"));

            var graph = GraphGenerator.FromDotText(graphText);

            var shortestGraph = graph.ToShortestGraph("Actor:Eric Elmosnino(1964)", "Actor:Libuse Safránková(1953)");

            var paths = (new PathAlgorithm(shortestGraph)).GetAllPaths("Actor:Eric Elmosnino(1964)");

            Assert.AreEqual(1032, paths.Count); //only copied

            foreach (var path in paths)
            {
                Assert.IsTrue(path.StartsWith("Actor:Eric Elmosnino(1964)"));
                Assert.IsTrue(path.EndsWith("Actor:Libuse Safránková(1953)"));
            }
        }
    }
}