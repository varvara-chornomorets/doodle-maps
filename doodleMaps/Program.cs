using doodleMaps;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 100,
    Width = 100,
    Seed = 5687687,
    Noise = 0.1f,
    AddTraffic = true,
    TrafficSeed = 58,
});

string[,] map = generator.Generate();

void DrawPath(string[,] myMap, List<Point> path)
{
    Point startPoint = path[0];
    myMap[startPoint.Column, startPoint.Row] = "A";
    for (int i = 1; i< path.Count-1; i++)
    {
        Point point = path[i];
        myMap[point.Column, point.Row] = ".";
    }
    Point endPoint = path[path.Count - 1];
    myMap[endPoint.Column, endPoint.Row] = "B";
}

bool IsEqual(Point a, Point b)
{
    return (a.Column == b.Column && a.Row == b.Row);
}

List<Point> GetNeighbours(Point mainPoint, string[,] myMap)
{
    List<Point> neighbours = new List<Point>();
    int x = mainPoint.Column;
    int y = mainPoint.Row;
    List<Point> potentialNeighbours = new List<Point>
    {
        new Point(x + 1, y),
        new Point(x - 1, y),
        new Point(x, y + 1),
        new Point(x, y - 1)
    };
    foreach (var varPotentialNeighbour in potentialNeighbours)
    {
        // if it is in the map AND it is not wall
        if (0 <= varPotentialNeighbour.Column && varPotentialNeighbour.Column < myMap.GetLength(0) &&
            0 <= varPotentialNeighbour.Row && varPotentialNeighbour.Row < myMap.GetLength(1) &&
            myMap[varPotentialNeighbour.Column, varPotentialNeighbour.Row] != "█")
        {
            neighbours.Add(varPotentialNeighbour);
        }
    
    }

    return neighbours;
}

List<Point> SimpleBFS(string[,] myMap, Point start, Point end)
{
    Queue<Point> nextPoints = new Queue<Point>();
    nextPoints.Enqueue(start);
    List<Point> checkedPoints = new List<Point>();
    var origins = new Dictionary<Point, Point?>();
    origins[start] = null;
    while (nextPoints.Count > 0)
    {
        // take first point
        Point currentPoint = nextPoints.Dequeue();
        // check if it is finish
        if (IsEqual(currentPoint, end))
        {
            break;
        }
        // add point to the list of checked Points
        checkedPoints.Add(currentPoint);
        // add its non-checked neighbours to the queue
        List<Point> currentNeighbours = GetNeighbours(currentPoint, myMap);
        foreach (var neighbour in currentNeighbours)
        {
            if (!checkedPoints.Contains(neighbour))
            {
                nextPoints.Enqueue(neighbour);
                checkedPoints.Add(neighbour);
                // add this points to the dictionary, their parent is currentPoint
                origins[neighbour] = currentPoint;
            }
        }
    }

    List<Point> result = new List<Point>();
    result.Add(end);
    Point currentValue = end;
    while (!IsEqual(currentValue, start))
    {
        var currentParent = origins[currentValue];
        result.Add(currentParent.Value);
        currentValue = currentParent.Value;


    }

    result.Reverse();
    return result;
}


Point MinimalPoint (List<Point> ToBeChecked, Dictionary<Point, float> costs)
{
    Point minimal = ToBeChecked[0];
    foreach (var point in ToBeChecked)
    {
        if (costs[point] < costs[minimal])
        {
            minimal = point;
        }
    }

    return minimal;
}

(List<Point>,string) Dijkstra(string[,] myMap, Point start, Point end)
{
    List<Point> ToBeChecked = new List<Point>();
    ToBeChecked.Add(start);
    List<Point> checkedPoints = new List<Point>();
    var origins = new Dictionary<Point, Point?>();
    origins[start] = null;
    var costs = new Dictionary<Point, float>();
    costs[start] = float.Parse(map[start.Column, start.Row]);
    Point currentPoint = new Point(0, 0);
    while (ToBeChecked.Count > 0)
    {
        // take minimal point
        currentPoint = MinimalPoint(ToBeChecked, costs);
        ToBeChecked.Remove(currentPoint);
        // check if it is finish
        if (IsEqual(currentPoint, end))
        {
            break;
        }
        // add point to the list of checked Points
        checkedPoints.Add(currentPoint);
        // add its non-checked neighbours to the list
        List<Point> currentNeighbours = GetNeighbours(currentPoint, myMap);
        foreach (var neighbour in currentNeighbours)
        {
            if (!checkedPoints.Contains(neighbour))
            {
                int column = neighbour.Column;
                int row = neighbour.Row;
                int nieghbourTraffic = int.Parse(myMap[column, row]);
                costs[neighbour] = costs[currentPoint] + 1.0f/(60-(nieghbourTraffic-1)*6);
                ToBeChecked.Add(neighbour);
                checkedPoints.Add(neighbour);
                // add this points to the dictionary, their parent is currentPoint
                origins[neighbour] = currentPoint;
            }
        }
    }

    float timeTaken = costs[currentPoint] - 1.0f/(60-(float.Parse(map[end.Column, end.Row])-1))*6;
    string totalTime = timeTaken.ToString("F2");
    List<Point> result = new List<Point>();
    result.Add(end);
    Point currentValue = end;
    while (!IsEqual(currentValue, start))
    {
        var currentParent = origins[currentValue];
        result.Add(currentParent.Value);
        currentValue = currentParent.Value;


    }

    result.Reverse();
    return (result, totalTime);
    {
        
    }
}


new MapPrinter().Print(map);
(List<Point> myPath, string totalTime) = Dijkstra(map, new Point(6,0), new Point(8,8) );
DrawPath(map, myPath);

new MapPrinter().Print(map);
Console.WriteLine("time taken: "+totalTime);
myPath = SimpleBFS(map, new Point(6, 0), new Point(8, 8));
DrawPath(map, myPath);

new MapPrinter().Print(map);
