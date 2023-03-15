using System.Xml;
using doodleMaps;

int HEIGHT = 20;
int WIDTH = 20;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = HEIGHT,
    Width = WIDTH,
    Noise = 0.1F,
});




Point Move(Point point, int columnMove, int rowMove)
{
    return new Point(point.Column + columnMove, point.Row + rowMove);
}



string[,] CreatePaths(string[,] mapEdited, Point start, Point goal)
{
    mapEdited[start.Column, start.Row] = "0";
    int counter1 = 0;
    
    while (true)
    {
        int counter = 0;
        for (int i = 0; i < WIDTH; i++)
        {
            for (int k = 0; k < HEIGHT; k++)
            {
                if (mapEdited[i, k] == "█")
                    continue;
                if (int.TryParse(mapEdited[i, k], out int num1))
                {
                    var position = new Point(i, k);
                    List<Point> points = new List<Point>();
                    points.Add(Move(position, 1, 0));
                    points.Add(Move(position, -1, 0));
                    points.Add(Move(position, 0, 1));
                    points.Add(Move(position, 0, -1));
                    foreach (var point in points)
                    {
                        if (0 <= point.Column && point.Column < WIDTH && 0 <= point.Row && point.Row < HEIGHT)
                        {
                            if (mapEdited[point.Column, point.Row] == " ")
                            {
                                var num = num1 + 1;
                                mapEdited[point.Column, point.Row] = num.ToString();
                            }
                            

                            else if (int.TryParse(mapEdited[point.Column, point.Row], out int num2))
                            {
                                if (num2 != num1 + 1 && num2 != num1 - 1 && num2 > num1)
                                {
                                    var num = num1 + 1;
                                    mapEdited[point.Column, point.Row] = num.ToString();
                                }
                            }
                        }
                    }
                    
                }
                else if (mapEdited[i, k] == " ")
                    counter++;
            }
        }
        if(counter == 0 || counter == counter1 || mapEdited[goal.Column, goal.Row] != " ")
            break;
        counter1 = counter;
    }

    return mapEdited;
}

string[,] FindPath(string[,] mapOrigin, string[,] map, Point goal)
{
    mapOrigin[goal.Column, goal.Row] = "B";
    map[goal.Column, goal.Row] = "#";
    Point position = goal;
    List<Point> points = new List<Point>();
    points.Add(Move(position, 1, 0));
    points.Add(Move(position, -1, 0));
    points.Add(Move(position, 0, 1));
    points.Add(Move(position, 0, -1));
    int num = -1;
    foreach (var point in points)
    {
        if (0 <= point.Column && point.Column < WIDTH && 0 <= point.Row && point.Row < HEIGHT)
        {
            if (int.TryParse(map[point.Column, point.Row], out int num1))
            {
                if (num1 < num || num == -1)
                {
                    num = num1;
                    position = point;
                }
            }
        }
    }
    mapOrigin[position.Column, position.Row] = ".";
    while (num != 0)
    {
        points.Clear();
        
        points.Add(Move(position, 1, 0));
        points.Add(Move(position, -1, 0));
        points.Add(Move(position, 0, 1));
        points.Add(Move(position, 0, -1));
        foreach (var point in points)
        {
            if (0 <= point.Column && point.Column < WIDTH && 0 <= point.Row && point.Row < HEIGHT)
            {
                if (int.TryParse(map[point.Column, point.Row], out int n))
                {
                    if (int.Parse(map[point.Column, point.Row]) == num-1)
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
    mapOrigin[position.Column, position.Row] = "A";
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
var mapCopy = new string[WIDTH,HEIGHT];
for (int i = 0; i < WIDTH; i++)
    for (int k = 0; k < HEIGHT; k++)
        mapCopy[i, k] = map[i, k];
//GetShortestPath(map, new Point(0, 0), new Point(6, 12)));
mapCopy = CreatePaths(mapCopy, new Point(0, 0), new Point(WIDTH - 2, HEIGHT - 2));
new MapPrinter().Print(mapCopy);
map = FindPath(map, mapCopy, new Point(WIDTH - 2, HEIGHT - 2));
new MapPrinter().Print(map);