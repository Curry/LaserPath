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

    public void SetupMirrors(int x, int y, MirrorOrientation orientation, MirrorReflection reflection)
    {
        rooms[x, y].Orientation = orientation;
        rooms[x, y].Reflection = reflection;
    }
}