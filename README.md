# mitoSoft.Graphs
A .net graph library to build directed graphs and investigate their properties (e.g. shortest path) 


## Example for shortest path determination via Dijkstra's Shortest Path algorithm

```c#
public void CalculateShortestDistance()
{
  var shortesGraph = new Graph()
	.AddNode("Start")
	.AddNode("Middle1")
	.AddNode("Middle2")
	.AddNode("End")
	.AddEdge("Start", "Middle1", 1, false)
	.AddEdge("Start", "Middle2", 1, false)
	.AddEdge("Middle1", "End", 1, false)
	.AddEdge("Middle2", "End", 1, false);
	
	var shortesGraph = (new DijkstraAlgorithm(graph)).GetShortestGraph("Start", "End");

    var endNode = shortesGraph.GetDistanceNode("End");
    Assert.AreEqual(2, endNode.Distance);
	
	...  
}
```


## Example for shortest path determination via a Deep-First-Search algorithm

```c#
public void CalculateShortestDistance()
{
  var shortesGraph = new Graph()
	.AddNode("Start")
	.AddNode("Middle1")
	.AddNode("Middle2")
	.AddNode("End")
	.AddEdge("Start", "Middle1", 1, false)
	.AddEdge("Start", "Middle2", 1, false)
	.AddEdge("Middle1", "End", 1, false)
	.AddEdge("Middle2", "End", 1, false)
	.ToShortestGraph("Start", "End");

    var endNode = shortesGraph.GetDistanceNode("End");
    Assert.AreEqual(2, endNode.Distance);
	
	...  
}
```


## Example for greating a shortes path grap via GraphViz

```c#

private const string GraphVizBinPath = @"C:\Temp\Graphviz\bin";

public void CalculateShortestDistance()
{	
  var image = new Graph()
	.AddNode("Start")
	.AddNode("Middle1")
	.AddNode("Middle2")
	.AddNode("End")
	.AddEdge("Start", "Middle1", 1, false)
	.AddEdge("Start", "Middle2", 1, false)
	.AddEdge("Middle1", "End", 1, false)
	.AddEdge("Middle2", "End", 1, false)
	.ToImage(GraphVizBinPath);

	...  
}
```

for more examples see the [mitoSoft.Graphs.UnitTests](../master/mitoSoft.Graphs.UnitTest) project.