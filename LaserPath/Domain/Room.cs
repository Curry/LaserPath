namespace LaserPath.Domain;

public class Room(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public MirrorOrientation Orientation { get; set; } = MirrorOrientation.None;
    public MirrorReflection Reflection { get; set; } = MirrorReflection.None;
}