namespace mitoSoft.Graphs
{
    public class BidirectionalEdge : GraphEdge
    {
        public BidirectionalEdge(GraphNode sourceNode, GraphNode targetNode, double distance) : base(sourceNode, targetNode, distance)
        {
        }
               
        public override string ToString() => $"{this.SourceNode.Name} <-> {this.TargetNode.Name} (Weight: {this.Weight})";
    }
}