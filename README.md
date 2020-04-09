# mitoSoft.Graphs
A .net graph library to build directed graphs and investigate their properties (e.g. shortest path) 

## Example for shortest path determination via Dijkstra's Shortest Path algorithm

```c#
public void CalculateShortestDistance()
{
  var graph = new DistanceGraph();

  graph.AddNode("Start");
  graph.AddNode("Middle1");
  graph.AddNode("Middle2");
  graph.AddNode("End");

  graph.AddConnection("Start", "End", 5, true);
  graph.AddConnection("Start", "Middle1", 1, true);
  graph.AddConnection("Middle1", "Middle2", 1, true);
  graph.AddConnection("Middle2", "End", 1, true);

  var shortesGraph = (new DeepFirstAlgorithm(graph)).GetShortestGraph("Start", "End");
}
```

