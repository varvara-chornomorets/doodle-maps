using doodleMaps;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 20,
    Width = 20,
    Seed = 12 ,
});

string[,] map = generator.Generate();
new MapPrinter().Print(map);

void DrawPath(string[,] myMap, List<Point> path)
{
    Point startPoint = path[0];
    myMap[startPoint.Row, startPoint.Column] = "A";
    for (int i = 1; i< path.Count-1; i++)
    {
        Point point = path[i];
        myMap[point.Row, point.Column] = ".";
    }
    Point endPoint = path[path.Count - 1];
    myMap[endPoint.Row, endPoint.Column] = "B";
}
