using Microsoft.VisualStudio.TestTools.UnitTesting;
using mitoSoft.Graphs.GraphVizInterop;
using mitoSoft.Graphs.ShortestPathAlgorithms;
using System.IO;
using System.Reflection;

namespace mitoSoft.Graphs.UnitTests
{
    public partial class ShortestPathAlgorithmsTests
    {
        private static Graph _graph;

        [ClassInitialize]
        public static void Initialize(TestContext _)
        {
            var graphText = File.ReadLines(Path.Combine("TestGraphs", "LargeGraph.txt"));

            _graph = GraphGenerator.FromDotText(graphText);
        }

        /// <summary>
        /// The connection can e.g. be made via 'The Expendables'
        /// </summary>
        [TestCategory("LargeGraph")]
        [TestMethod]
        public void Degree1()
        {
            var shortestGraph = _graph.ToShortestGraph("Actor:Dolph Lundgren(1957)", "Actor:Sylvester Stallone(1946)");

            Assert.AreEqual(2, ((DistanceNode)shortestGraph.GetNode("Actor:Sylvester Stallone(1946)")).Distance);

            var dotText = shortestGraph.ToDotText();

            Assert.IsTrue(dotText.Contains("Actor_Dolph_Lundgren_1957 -> Movie_The_Expendables"));
            Assert.IsTrue(dotText.Contains("Movie_The_Expendables -> Actor_Sylvester_Stallone_1946"));
        }

        /// <summary>
        /// The connection can e.g. be made via 'The Expendables'
        /// </summary>
        [TestCategory("LargeGraph")]
        [TestMethod]
        public void Degree1ViceVersa()
        {
            var shortestGraph = _graph.ToShortestGraph("Actor:Sylvester Stallone(1946)", "Actor:Dolph Lundgren(1957)");

            Assert.AreEqual(2, ((DistanceNode)shortestGraph.GetNode("Actor:Dolph Lundgren(1957)")).Distance);

            var dotText = shortestGraph.ToDotText();

            Assert.IsTrue(dotText.Contains("Actor_Sylvester_Stallone_1946 -> Movie_The_Expendables"));
            Assert.IsTrue(dotText.Contains("Movie_The_Expendables -> Actor_Dolph_Lundgren_1957"));
        }

        /// <summary>
        /// The connection can be made via 'TRON' and 'Spider-man: Far From Home'
        /// </summary>
        [TestCategory("LargeGraph")]
        [TestMethod]
        public void Degree2()
        {
            var shortestGraph = _graph.ToShortestGraph("Actor:Tom Holland(1996)", "Actor:Charlie Picerni(1935)");

            Assert.AreEqual(4, ((DistanceNode)shortestGraph.GetNode("Actor:Charlie Picerni(1935)")).Distance);

            var dotText = shortestGraph.ToDotText();

            Assert.IsTrue(dotText.Contains("Actor_Tom_Holland_1996 -> Movie_Spider_Man_Far_From_Home"));
            Assert.IsTrue(dotText.Contains("Movie_Spider_Man_Far_From_Home -> Actor_Jeff_Bridges_1949"));
            Assert.IsTrue(dotText.Contains("Actor_Jeff_Bridges_1949 -> Movie_TRON"));
            Assert.IsTrue(dotText.Contains("Movie_TRON -> Actor_Charlie_Picerni_1935"));
        }

        /// <summary>
        /// The connection can be made via 'TRON' and 'Spider-man: Far From Home'
        /// </summary>
        [TestCategory("LargeGraph")]
        [TestMethod]
        public void Degree2ViceVersa()
        {
            var shortestGraph = _graph.ToShortestGraph("Actor:Charlie Picerni(1935)", "Actor:Tom Holland(1996)");

            Assert.AreEqual(4, ((DistanceNode)shortestGraph.GetNode("Actor:Tom Holland(1996)")).Distance);

            var dotText = shortestGraph.ToDotText();

            Assert.IsTrue(dotText.Contains("Actor_Charlie_Picerni_1935 -> Movie_TRON"));
            Assert.IsTrue(dotText.Contains("Movie_TRON -> Actor_Jeff_Bridges_1949"));
            Assert.IsTrue(dotText.Contains("Actor_Jeff_Bridges_1949 -> Movie_Spider_Man_Far_From_Home"));
            Assert.IsTrue(dotText.Contains("Movie_Spider_Man_Far_From_Home -> Actor_Tom_Holland_1996"));
        }


        [TestCategory("LargeGraph")]
        [TestMethod]
        public void Degree4()
        {
            var source = _graph.GetNode("Actor:Toshirô Mifune(1920)");
            var target = _graph.GetNode("Actor:Tom Holland(1996)");

            var shortestGraph = _graph.ToShortestGraph(source, target);

            Assert.AreEqual(8, ((DistanceNode)shortestGraph.GetNode("Actor:Tom Holland(1996)")).Distance);
        }

        [TestCategory("LargeGraph")]
        [TestMethod]
        public void Degree4ViceVersa()
        {
            var source = _graph.GetNode("Actor:Tom Holland(1996)");
            var target = _graph.GetNode("Actor:Toshirô Mifune(1920)");

            var shortestGraph = _graph.ToShortestGraph(source, target);

            Assert.AreEqual(8, ((DistanceNode)shortestGraph.GetNode("Actor:Toshirô Mifune(1920)")).Distance);
        }

        [TestCategory("LargeGraph")]
        [TestMethod]
        public void Degree4ToImageFile()
        {
            var graphVizFolder = @"C:\Temp\Graphviz\bin";
            var imageFile = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "TestGraphs", "Graph.png");

            var source = _graph.GetNode("Actor:Toshirô Mifune(1920)");
            var target = _graph.GetNode("Actor:Tom Holland(1996)");

            var shortestGraph = _graph.ToShortestGraph(source, target);

            shortestGraph.ToImageFile(graphVizFolder, imageFile);

            Assert.IsTrue(File.Exists(imageFile));
        }
    }
}
