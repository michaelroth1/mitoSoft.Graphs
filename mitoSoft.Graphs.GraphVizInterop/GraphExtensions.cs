using mitoSoft.Graphs.GraphVizInterop.Enums;
using mitoSoft.Graphs.GraphVizInterop.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace mitoSoft.Graphs.GraphVizInterop
{
    public static class GraphExtensions
    {
        public static Image ToImage(this DirectedGraph graph) 
            => graph.ToImage(@"%USERPROFILE%/.nuget/packages/graphviz/2.38.0.2");

        public static Image ToImage(this DirectedGraph graph, string graphVizBinPath)
        {
            var dotText = graph.ToDotText();

            var path = new DirectoryInfo(graphVizBinPath).FullName;

            var image = (new ImageRenderer(path)).RenderImage(dotText);

            return image;
        }

        public static void ToImageFile(this DirectedGraph graph, string fileName) 
            => graph.ToImageFile(fileName, @"%USERPROFILE%/.nuget/packages/graphviz/2.38.0.2");

        public static void ToImageFile(this DirectedGraph graph, string fileName, string graphVizBinPath)
        {
            var dotText = graph.ToDotText();

            var fileInfo = new FileInfo(fileName);

            var path = new DirectoryInfo(graphVizBinPath).FullName;

            (new ImageRenderer(path)).RenderImage(dotText, fileName, LayoutEngine.dot, (ImageFormat)Enum.Parse(typeof(ImageFormat), fileInfo.Extension.Replace(".", "")));
        }

        public static string ToDotText(this DirectedGraph graph)
        {
            var dotTextGenerator = new DotTextGenerator();
            var nodesNames = new Dictionary<DirectedGraphNode, string>();

            //Performance improvment of 1.5sec in case of 250000 graph elements
            foreach (var node in graph.Nodes)
            {
                var nodeId = node.Name.EscapeDotName();

                nodesNames.Add(node, nodeId);
            }

            foreach (var node in graph.Nodes)
            {
                var nodeId = nodesNames[node];

                if (string.IsNullOrWhiteSpace(node.Description))
                {
                    dotTextGenerator.SetNode(nodeId, node.Name.EscapeDotLabel(), Enums.Color.black, Enums.Color.aliceblue, Enums.Shapes.oval);
                }
                else
                {
                    dotTextGenerator.SetNode(nodeId, node.Description.EscapeDotLabel(), Enums.Color.black, Enums.Color.aliceblue, Enums.Shapes.oval);
                }
            }

            foreach (var edge in graph.Edges)
            {
                var sourceId = nodesNames[edge.Source];
                var targetId = nodesNames[edge.Target];

                if (edge is BidirectedEdge)
                {
                    dotTextGenerator.SetEdge(sourceId, targetId, edge.Description, Enums.Color.black, Enums.EdgeStyle.solid, Enums.Arrowheads.normal, "dir=\"both\"");
                }
                else
                {
                    dotTextGenerator.SetEdge(sourceId, targetId, edge.Description, Enums.Color.black, Enums.EdgeStyle.solid, Enums.Arrowheads.normal);
                }
            }

            var result = dotTextGenerator.GetText();

            return result;
        }
    }
}