using mitoSoft.Graphs.GraphVizInterop.Enums;
using System;
using System.Drawing;
using System.IO;

namespace mitoSoft.Graphs.GraphVizInterop
{
    public static class GraphExtensions
    {
        public static Image ToPng(this Graph graph, string graphVizBinPath)
        {
            var dotText = graph.ToDotText();

            var image = (new ImageRenderer(graphVizBinPath)).RenderImage(dotText);

            return image;
        }

        public static Image ToSvg(this Graph graph, string graphVizBinPath)
        {
            var dotText = graph.ToDotText();

            var image = (new ImageRenderer(graphVizBinPath)).RenderImage(dotText, LayoutEngine.dot, ImageFormat.svg);

            return image;
        }

        public static void ToImageFile(this Graph graph, string graphVizBinPath, string fileName)
        {
            var dotText = graph.ToDotText();

            var fileInfo = new FileInfo(fileName);

            (new ImageRenderer(graphVizBinPath)).RenderImage(dotText, fileName, LayoutEngine.dot, (ImageFormat)Enum.Parse(typeof(ImageFormat), fileInfo.Extension.Replace(".", "")));
        }

        public static string ToDotText(this Graph graph)
        {
            var builder = new DotTextGenerator();

            foreach (var node in graph.Nodes)
            {
                var nodeId = GenerateNodeId(node.Name);
                builder.SetNode(nodeId, node.Name, Enums.Color.black, Enums.Color.aliceblue, Enums.Shapes.circle);
            }

            foreach (var edge in graph.Edges)
            {
                var sourceId = GenerateNodeId(edge.SourceNode.Name);
                var targetId = GenerateNodeId(edge.TargetNode.Name);
                builder.SetEdge(sourceId, targetId, edge.Distance.ToString(), Enums.Color.black, Enums.EdgeStyle.solid, Enums.Arrowheads.normal);
            }

            var result = builder.GetText();
            return result;
        }

        private static string GenerateNodeId(string name)
        {
            name = name.Replace(":", " ");
            name = name.Replace(" ", "_");
            name = name.Replace("___", "_");
            name = name.Replace("__", "_");
            name = name.Replace("(", "");
            name = name.Replace(")", "");
            name = name.Trim();
            return name;
        }
    }
}
