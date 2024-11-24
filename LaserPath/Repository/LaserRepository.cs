using LaserPath.Domain;

namespace LaserPath.Repository;

public class LaserRepository : ILaserRepository
{
    private readonly Room[,] rooms;

    public LaserRepository(int width, int height)
    {
        rooms = new Room[width, height];
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                rooms[i, j] = new Room(i, j);
            }
        }
    }

    public Room GetRoom(int x, int y)
    {
        return rooms[x, y];
    }

    public void SetupMirrors(int x, int y, string[] values)
    {
        var room = rooms[x, y];
        
        room.Orientation = values[0] switch
        {
            "L" => MirrorOrientation.Left,
            "R" => MirrorOrientation.Right,
            _ => throw new ArgumentOutOfRangeException()
        };
        if (values.Length == 2)
        {
            room.Reflection = values[1] switch
            {
                "L" => MirrorReflection.Left,
                "R" => MirrorReflection.Right,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else
        {
            room.Reflection = MirrorReflection.Both;
        }
    }
}