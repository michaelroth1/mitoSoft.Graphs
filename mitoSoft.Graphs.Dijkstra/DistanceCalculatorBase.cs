using System;

namespace mitoSoft.Graphs.Dijkstra
{
    public abstract class DistanceCalculatorBase
    {
        protected DistanceGraph _graph;

        public DistanceCalculatorBase(DistanceGraph graph)
        {
            this._graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }
               
        protected void InitializeGraph(DistanceNode sourceNode)
        {
            foreach (DistanceNode node in this._graph.Nodes)
            {
                node.ResetDistanceFromStart();
            }

            sourceNode.SetDistanceFromStart(0);
        }

        protected DistanceGraph BuildShortestPathGraph(DistanceNode sourceNode, DistanceNode targetNode)
        {
            var graph = new DistanceGraph();

            var node = (DistanceNode)graph.AddNode(targetNode.Name);
            node.SetDistanceFromStart(targetNode.DistanceFromStart);

            GetShortestGraph(sourceNode, targetNode, graph);

            return graph;
        }

        protected DistanceNode Check(string sourceNodeName)
        {
            if (string.IsNullOrEmpty(sourceNodeName))
            {
                throw new ArgumentNullException(nameof(sourceNodeName));
            }

            if (!this._graph.TryGetNode(sourceNodeName, out var node))
            {
                throw new NodeNotInGraphException(sourceNodeName);
            }

            return node;
        }

        protected DistanceNode Check(DistanceNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (!this._graph.TryGetNode(node.Name, out var existingNode))
            {
                throw new NodeNotInGraphException(node);
            }

            return existingNode;
        }
        private void GetShortestGraph(DistanceNode sourceNode, DistanceNode targetNode, DistanceGraph graph)
        {
            if (sourceNode.Name == targetNode.Name)
            {
                return;
            }

            var predecessorNodes = targetNode.GetShortestPathPredecessors();

            foreach (var predecessor in predecessorNodes)
            {
                var node = (DistanceNode)graph.AddNode(predecessor.Name);
                node.SetDistanceFromStart(predecessor.DistanceFromStart);

                var con = _graph.TryGetConnector(predecessor, targetNode);

                graph.AddConnection(predecessor.Name, targetNode.Name, con.Distance, false);

                GetShortestGraph(sourceNode, predecessor, graph);
            }
        }
    }
}
