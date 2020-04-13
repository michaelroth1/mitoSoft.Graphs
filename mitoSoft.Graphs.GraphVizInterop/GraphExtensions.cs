using mitoSoft.Graphs.GraphVizInterop.Enums;
using mitoSoft.Graphs.GraphVizInterop.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace mitoSoft.Graphs.GraphVizInterop
{
    public static class GraphExtensions
    {
        public static Image ToImage(this Graph graph, string graphVizBinPath)
        {
            var dotText = graph.ToDotText();

            var image = (new ImageRenderer(graphVizBinPath)).RenderImage(dotText);

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
            var dotTextGenerator = new DotTextGenerator();
            var nodesWithoutIllegalDotCharacters = new Dictionary<GraphNode, string>();

            //Performance improvment of 1.5sec in case of 250000 graph elements
            foreach (var node in graph.Nodes)
            {
                var nodeId = node.Name.RemoveIllegalDotCharacters();

                nodesWithoutIllegalDotCharacters.Add(node, nodeId);
            }

            foreach (var node in graph.Nodes)
            {
                var nodeId = nodesWithoutIllegalDotCharacters[node];

                if (string.IsNullOrWhiteSpace(node.Description))
                {
                    dotTextGenerator.SetNode(nodeId, node.Name, Enums.Color.black, Enums.Color.aliceblue, Enums.Shapes.oval);
                }
                else
                {
                    dotTextGenerator.SetNode(nodeId, node.Description, Enums.Color.black, Enums.Color.aliceblue, Enums.Shapes.oval);
                }
            }

            foreach (var edge in graph.Edges)
            {
                var sourceId = nodesWithoutIllegalDotCharacters[edge.SourceNode];
                var targetId = nodesWithoutIllegalDotCharacters[edge.TargetNode];

                dotTextGenerator.SetEdge(sourceId, targetId, edge.Description, Enums.Color.black, Enums.EdgeStyle.solid, Enums.Arrowheads.normal);
            }

            var result = dotTextGenerator.GetText();

            return result;
        }
    }
}