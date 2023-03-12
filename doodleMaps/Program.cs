using System.Xml;
using doodleMaps;

int HEIGHT = 20;
int WIDTH = 20;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = HEIGHT,
    Width = WIDTH,
    Seed = 12 ,
});




Point Move(Point point, int columnMove, int rowMove)
{
    return new Point(point.Column + columnMove, point.Row + rowMove);
}




void CreateNodes(string[,] map)
{
    for (int i = 0; i < WIDTH; i++)
    {
        for (int k = 0; k < HEIGHT; k++)
        {
            if (map[i, k] == "█")
                continue;
            var position = new Point(i, k);
            List<Point> points = new List<Point>();
            points.Add(Move(position, 1, 0));
            points.Add(Move(position, -1, 0));
            points.Add(Move(position, 0, 1));
            points.Add(Move(position, 0, -1));
            List<Point> toRemove = new List<Point>();
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
            if (points.Count >= 3)
                map[position.Column, position.Row] = "*";
        }
    }
}

string[,] CreatePaths(string[,] mapEdited, Point start, Point goal)
{
    mapEdited[start.Column, start.Row] = "0";
    mapEdited[goal.Column, goal.Row] = "#";
    while (true)
    {
        int counter = 0;
        for (int i = 0; i < WIDTH; i++)
        {
            for (int k = 0; k < HEIGHT; k++)
            {
                if (mapEdited[i, k] == "█")
                    continue;
                if (int.TryParse(mapEdited[i, k], out int n))
                {
                    var position = new Point(i, k);
                    List<Point> points = new List<Point>();
                    points.Add(Move(position, 1, 0));
                    points.Add(Move(position, -1, 0));
                    points.Add(Move(position, 0, 1));
                    points.Add(Move(position, 0, -1));
                    foreach (var point in points)
                    {
                        if (0 <= point.Column && point.Column <= 20 && 0 <= point.Row && point.Row <= 20)
                        {
                            if (mapEdited[point.Column, point.Row] == " ")
                            {
                                var num = int.Parse(mapEdited[position.Column, position.Row]);
                                num++;
                                mapEdited[point.Column, point.Row] = num.ToString();
                            }
                        }
                    }
                }

                else if (mapEdited[i, k] == "#")
                {
                    var position = new Point(i, k);
                    List<Point> points = new List<Point>();
                    points.Add(Move(position, 1, 0));
                    points.Add(Move(position, -1, 0));
                    points.Add(Move(position, 0, 1));
                    points.Add(Move(position, 0, -1));
                    foreach (var point in points)
                    {
                        if (0 <= point.Column && point.Column <= 20 && 0 <= point.Row && point.Row <= 20)
                        {
                            int.TryParse(mapEdited[point.Column, point.Row], out int num);
                            if (mapEdited[point.Column, point.Row] == " ")
                            {
                                num++;
                                mapEdited[point.Column, point.Row] = num.ToString();
                            }
                        }
                    }
                }
                else if (mapEdited[i, k] == " ")
                    counter++;
            }
        }
        if(counter == 0)
            break;
    }

    return mapEdited;
}

string[,] FindPath(string[,] mapOrigin, string[,] map, Point goal)
{
    mapOrigin[goal.Column, goal.Row] = "B";
    Point position = goal;
    List<Point> points = new List<Point>();
    points.Add(Move(position, 1, 0));
    points.Add(Move(position, -1, 0));
    points.Add(Move(position, 0, 1));
    points.Add(Move(position, 0, -1));
    int num = 200;
    foreach (var point in points)
    {
        if (0 <= point.Column && point.Column <= 20 && 0 <= point.Row && point.Row <= 20)
        {
            if (map[point.Column, point.Row] != "█")
            {
                var num1 = int.Parse(map[point.Column, point.Row]);
                if (num1 < num)
                {
                    num = num1;
                    position = point;
                    mapOrigin[point.Column, point.Row] = ".";
                }
            }
        }
    }
    while (num != 0)
    {
        points.Clear();
        points.Add(Move(position, 1, 0));
        points.Add(Move(position, -1, 0));
        points.Add(Move(position, 0, 1));
        points.Add(Move(position, 0, -1));
        foreach (var point in points)
        {
            if (0 <= point.Column && point.Column <= 20 && 0 <= point.Row && point.Row <= 20)
            {
                if (int.TryParse(map[point.Column, point.Row], out int n))
                {
                    if (int.Parse(map[point.Column, point.Row]) == num--)
                    {
                        position = point;
                        num = int.Parse(map[point.Column, point.Row]);
                        mapOrigin[point.Column, point.Row] = ".";
                        break;
                    }
                }
            }
        }
    }

    return mapOrigin;
}


/*
List<Point> GetShortestPath(string[,] map, Point start, Point goal)
{
    CreateNodes(map);
}
*/





string[,] map = generator.Generate();
new MapPrinter().Print(map);
string[,] mapCopy = generator.Generate();
//GetShortestPath(map, new Point(0, 0), new Point(6, 12)));
mapCopy = CreatePaths(mapCopy, new Point(0, 0), new Point(6, 12));
new MapPrinter().Print(mapCopy);
map = FindPath(map, mapCopy, new Point(6, 12));
new MapPrinter().Print(map);