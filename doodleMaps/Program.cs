using System.Xml;
using doodleMaps;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 20,
    Width = 20,
    Seed = 12 ,
});

Point Move(Point point, int columnMove, int rowMove)
{
    return new Point(point.Column + columnMove, point.Row + rowMove);
}

List<Point> GetShortestPath(string[,] map, Point start, Point goal)
{
    Point previousStep = start;
    Point position = start;
    List<Point> path = new List<Point>();
    path.Add(position);
    while (position.Column != goal.Column && position.Column != goal.Column)
    {
        List<Point> points = new List<Point>();
        points.Add(Move(position, 1, 0));
        points.Add(Move(position, -1, 0));
        points.Add(Move(position, 0, 1));
        points.Add(Move(position, 0, -1));
        List<Point> toRemove = new List<Point>();
        points.Remove(previousStep);
        foreach (var point in points)
        {
            if (0 <= point.Column && point.Column <= 20 && 0 <= point.Row && point.Row <= 20)
            {
                if (map[point.Column, point.Row] == "█")
                    toRemove.Add(point);
            }
            else
                toRemove.Add(point);
        }

        foreach (var point in toRemove)
            points.Remove(point);
        if (points.Count == 1)
        {
            path.Add(points[0]);
            previousStep = position;
            position = points[0];
        }
        else
        {
            //тут має бути все інше але я всьо
        }
    }
    return path;
}






string[,] map = generator.Generate();
new MapPrinter().Print(map);
foreach (var point in GetShortestPath(map, new Point(0, 0), new Point(6, 12)))
{
    Console.WriteLine(point.Column +" " + point.Row);
}