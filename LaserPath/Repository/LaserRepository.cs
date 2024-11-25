using LaserPath.Domain;

namespace LaserPath.Repository;

public class LaserRepository : ILaserRepository
{
    public int Width { get; }
    public int Height { get; }
    public int StartX { get; }
    public int StartY { get; }
    public Laser StartLaser { get; }
    public string[] Mirrors { get; }
    
    private readonly Room[,] rooms;

    public LaserRepository(InputFile inputFile)
    {
        Width = inputFile.Width;
        Height = inputFile.Height;
        StartX = inputFile.StartX;
        StartY = inputFile.StartY;
        StartLaser = inputFile.StartLaser;
        Mirrors = inputFile.Mirrors;
        
        rooms = new Room[Width, Height];
        for (var i = 0; i < Width; i++)
        {
            for (var j = 0; j < Height; j++)
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