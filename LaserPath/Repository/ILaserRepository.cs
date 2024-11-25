using LaserPath.Domain;

namespace LaserPath.Repository;

public interface ILaserRepository
{
    int Width { get; }
    int Height { get; }
    int StartX { get; }
    int StartY { get; }
    Laser StartLaser { get; }
    string[] Mirrors { get; }
    Room GetRoom(int x, int y);
    void SetupMirrors(int x, int y, MirrorOrientation orientation, MirrorReflection reflection);
}