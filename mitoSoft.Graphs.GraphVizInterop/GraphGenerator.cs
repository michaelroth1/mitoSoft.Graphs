using mitoSoft.Graphs.GraphVizInterop.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mitoSoft.Graphs.GraphVizInterop
{
    public static class GraphGenerator
    {
        public static Graph FromDotText(string dotText)
        {
            var lines = dotText.Split(Environment.NewLine.ToCharArray()).ToList();

            return GraphGenerator.FromDotText(lines);
        }

        public static Graph FromDotText(List<string> dotText)
        {
            dotText.RemoveAll(s => s == string.Empty);

            var graph = new Graph();

            foreach (var line in dotText)
            {
                if (line.Contains("->"))
                {
                    string left = line.Substring(0, line.IndexOf("->")).Trim();
                    string right = line.Between("->", "[").Trim();
                    string label = line.Between("label=", ",").Trim().Trim(']').Trim('\"');
                    if (!double.TryParse(label, out var distance))
                    {
                        distance = 1d;
                    }
                    graph.TryAddNode(left, out _);
                    graph.TryAddNode(right, out _);
                    graph.TryAddEdge(left, right, distance, false);
                }
            }

            return graph;
        }
    }
}