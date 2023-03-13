using doodleMaps;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 20,
    Width = 40,
    Seed = 50 ,
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
new MapPrinter().Print(map);
List<Point> myPath = SimpleBFS(map, new Point(0,0), new Point(18,18) );
DrawPath(map, myPath);

new MapPrinter().Print(map);