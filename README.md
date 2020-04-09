# mitoSoft.Graphs
A .net graph library to build directed graphs and investigate their properties (e.g. shortest path) 

## Example

```c#
public void CalculateShortestDistance()
        {
            var graph = new DistanceGraph(true);

            var startNode = new DistanceNode("Start", new GraphNodeKey("Start"));

            var endNode = new DistanceNode("End", new GraphNodeKey("End"));

            var middleNode1 = new DistanceNode("Middle1", new GraphNodeKey("Middle1"));

            var middleNode2 = new DistanceNode("Middle2", new GraphNodeKey("Middle2"));

            graph.AddNode(startNode);
            graph.AddNode(middleNode1);
            graph.AddNode(middleNode2);
            graph.AddNode(endNode);

            graph.AddConnection(startNode, endNode, 5);

            graph.AddConnection(startNode, middleNode1, 1);
            graph.AddConnection(middleNode1, middleNode2, 1);
            graph.AddConnection(middleNode2, endNode, 1);

            var calculator = new DistanceCalculator(graph);

            var distance = calculator.CalculateDistancesByDeepFirst(ref startNode, ref endNode);

            Assert.AreEqual(3, distance);

            var stepsList = calculator.GetShortestPath(endNode).ToList();

            Assert.AreEqual(1, stepsList.Count);

            var stepList = stepsList[0];

            Assert.AreEqual(3, stepList.Degree);
        }
```

