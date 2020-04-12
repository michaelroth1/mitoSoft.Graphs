using mitoSoft.Graphs.GraphVizInterop.Enums;
using System;
using System.Collections.Generic;

namespace mitoSoft.Graphs.GraphVizInterop
{
    public sealed class DotTextGenerator
    {
        private List<string> edgeTexts;
        private List<string> nodeTexts;

        public DotTextGenerator()
        {
            this.SetBody();
        }

        public void SetBody()
        {
            this.nodeTexts = new List<string>();
            this.edgeTexts = new List<string>();
        }

        private string GetHeader()
        {
            return "digraph unix {" + Environment.NewLine +
                   "node [style=filled];" + Environment.NewLine +
                   "rankdir = TB;" + Environment.NewLine +
                   "overlap=false;" + Environment.NewLine;
        }

        private void SetNextNodeText(string name, string label, Color color, Color fillcolor, Shapes shape, string otherParameters)
        {
            nodeTexts.Add(string.Format("{0} [shape={1},label=\"{2}\",color={3},fillcolor={4},style={5}{6}]",
                                        name.Replace(".", ""), shape.ToString(), label, color.ToString(), fillcolor.ToString(), "filled", otherParameters));
        }

        private void SetEdgeText(string fromState, string toState, string label, Color color, EdgeStyle style, Arrowheads arrow, string otherParameters)
        {
            edgeTexts.Add(string.Format("{0} -> {1} [color={2},arrowhead={3},fontcolor={2},style={4},label=\"{5}\",decorate=false{6}]",
                                        fromState.Replace(".", ""), toState.Replace(".", ""), color.ToString(), arrow.ToString(), style.ToString(), label, otherParameters));
        }

        public void SetEdge(string fromState, string toState, string label, Color color, EdgeStyle style, Arrowheads arrow)
        {
            this.SetEdgeText(fromState, toState, label, color, style, arrow, "");
        }

        public void SetEdge(string fromState, string toState, string label, Color color, EdgeStyle style, Arrowheads arrow, string otherParameters)
        {
            if (otherParameters.IndexOf(",") > 0)
            {
                otherParameters = "," + otherParameters;
            }

            this.SetEdgeText(fromState, toState, label, color, style, arrow, otherParameters);
        }

        public void SetNode(string name, string label, Color color, Color fillcolor, Shapes shape)
        {
            this.SetNextNodeText(name, label, color, fillcolor, shape, "");
        }

        public void SetNode(string name, string label, Color color, Color fillcolor, Shapes shape, string otherParameters)
        {
            if (otherParameters.IndexOf(",") > 0)
            {
                otherParameters = "," + otherParameters;
            }

            this.SetNextNodeText(name, label, color, fillcolor, shape, otherParameters);
        }

        /// <summary>
        /// Get dot-text
        /// </summary>
        public string GetText()
        {
            string nodeText = string.Join(Environment.NewLine, nodeTexts.ToArray());
            string edgeText = string.Join(Environment.NewLine, edgeTexts.ToArray());

            return this.GetHeader() + Environment.NewLine
                   + nodeText + Environment.NewLine
                   + edgeText + Environment.NewLine
                   + "}";
        }
    }
}