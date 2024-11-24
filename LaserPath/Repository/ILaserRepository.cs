using LaserPath.Domain;

namespace LaserPath.Repository;

public interface ILaserRepository
{
    public Room GetRoom(int x, int y);
    public void SetupMirrors(int x, int y, string[] values);
}