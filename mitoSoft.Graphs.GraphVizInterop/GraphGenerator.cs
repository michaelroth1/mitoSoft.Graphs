using mitoSoft.Graphs.Exceptions;
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

        public static Graph FromDotText(IEnumerable<string> dotText)
        {
            var dotLines = dotText.ToList();

            dotLines.ToList().RemoveAll(s => s == string.Empty);

            var graph = new Graph();

            var nodeLines = new List<string>();
            var edgeLines = new List<string>();

            foreach (var line in dotLines)
            {
                if (line.Contains("<->"))
                {
                    edgeLines.Add(line);
                }
                else if (line.Contains("->"))
                {
                    edgeLines.Add(line);
                }
                else if (line.Contains("--"))
                {
                    throw new InvalidOperationException("Undirected egedes are not supported.");
                }
                else
                {
                    nodeLines.Add(line);
                }
            }

            foreach (var line in edgeLines)
            {
                if (line.Contains("<->"))
                {
                    string left = line.Substring(0, line.IndexOf("<->")).Trim();
                    string right = line.Between("<->", "[").Trim();
                    AddEgde(left, right, line, graph, nodeLines);
                }
                else if (line.Contains("->"))
                {
                    string left = line.Substring(0, line.IndexOf("->")).Trim();
                    string right = line.Between("->", "[").Trim();
                    AddEgde(left, right, line, graph, nodeLines);
                }
            }

            return graph;
        }

        private static void AddEgde(string left, string right, string line, Graph graph, List<string> nodeLines)
        {
            string label = line.Between("label=", ',', ']').Trim().Trim('\"');
            string weightText = line.Between("weight=", ',', ']').Trim().Trim('\"');
            double weight = 1d;
            if (weightText != "")
            {
                weight = double.Parse(weightText);
            }
            bool bidirection = false;
            if (line.Replace(" ", "").Contains("dir=\"both\"")
             || line.Replace(" ", "").Contains("dir=both")
             || line.Replace(" ", "").Contains("<->"))
            {
                bidirection = true;
            }

            AddNode(graph, left, nodeLines);
            AddNode(graph, right, nodeLines);

            if (graph.TryAddEdge(left, right, weight, bidirection, out var edge))
            {
                edge.Description = label;
            }
            else
            {
                throw new EdgeAlreadyExistingException(left, right);
            }
        }

        private static void AddNode(Graph graph, string text, List<string> lines)
        {
            var line = lines.SingleOrDefault(l => l.Between(0, '[').Contains(text) == true);
            string label = string.Empty;
            if (!string.IsNullOrEmpty(line))
            {
                label = line.Between("label=", ',', ']').Trim().Trim('\"');
            }

            var node = new GraphNode(text, label);
            graph.TryAddNode(node);
        }
    }
}