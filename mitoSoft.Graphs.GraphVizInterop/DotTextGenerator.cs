using mitoSoft.Graphs.GraphVizInterop.Enums;
using System;

namespace mitoSoft.Graphs.GraphVizInterop
{
    /// <summary>
    /// Stellt die Funktionen zur verfügung um einen Graphen zu zeichnen
    /// </summary>
    /// <remarks></remarks>
    public sealed class DotTextGenerator
    {
        private string _text;

        public DotTextGenerator()
        {
            this.SetBody();
        }

        public void SetBody()
        {
            this._text = "";
            this._text = this.SetBodyText();
        }

        private string SetBodyText()
        {
            return "digraph unix {" + Environment.NewLine +
                   "node [style=filled];" + Environment.NewLine +
                   "rankdir = TB;" + Environment.NewLine +
                   "overlap=false;" + Environment.NewLine +
                   "#NextState#" + Environment.NewLine +
                   "#NextConnection#" + Environment.NewLine +
                   "}";
        }

        private string SetConnectionText(string text, string fromState, string toState, string label, Color color, EdgeStyle style, Arrowheads arrow, string otherParameters)
        {
            return text.Replace("#NextConnection#", string.Format("{0} -> {1} [color={2},arrowhead={3},fontcolor={2},style={4},label=\"{5}\",decorate=false{6}]" + Environment.NewLine +
                                                                  "#NextConnection#",
                                                                  fromState.Replace(".", ""), toState.Replace(".", ""), color.ToString(), arrow.ToString(), style.ToString(), label, otherParameters));
        }

        private string SetNextNodeText(string text, string name, string label, Color color, Color fillcolor, Shapes shape, string otherParameters)
        {
            return text.Replace("#NextState#", string.Format("{0} [shape={1},label=\"{2}\",color={3},fillcolor={4},style={5}{6}]" + Environment.NewLine +
                                                             "#NextState#",
                                                             name.Replace(".", ""), shape.ToString(), label, color.ToString(), fillcolor.ToString(), "filled", otherParameters));
        }

        public void SetEdge(string fromState, string toState, string label, Color color, EdgeStyle style, Arrowheads arrow)
        {
            this._text = this.SetConnectionText(this._text, fromState, toState, label, color, style, arrow, "");
        }

        public void SetEdge(string fromState, string toState, string label, Color color, EdgeStyle style, Arrowheads arrow, string otherParameters)
        {
            if (otherParameters.IndexOf(",") > 0)
            {
                otherParameters = "," + otherParameters;
            }
            this._text = this.SetConnectionText(this._text, fromState, toState, label, color, style, arrow, otherParameters);
        }


        public void SetNode(string name, string label, Color color, Color fillcolor, Shapes shape)
        {
            this._text = this.SetNextNodeText(this._text, name, label, color, fillcolor, shape, "");
        }

        public void SetNode(string name, string label, Color color, Color fillcolor, Shapes shape, string otherParameters)
        {
            if (otherParameters.IndexOf(",") > 0)
            {
                otherParameters = "," + otherParameters;
            }
            this._text = this.SetNextNodeText(this._text, name, label, color, fillcolor, shape, otherParameters);
        }

        /// <summary>
        /// Platzhalter entfernen
        /// </summary>
        public string GetText()
        {
            this._text = this._text.Replace("#NextState#" + Environment.NewLine, "");
            this._text = this._text.Replace("#NextConnection#" + Environment.NewLine, "");
            return this._text;
        }
    }
}