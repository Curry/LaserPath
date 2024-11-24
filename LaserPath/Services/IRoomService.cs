using LaserPath.Domain;

namespace LaserPath.Services;

public interface IRoomService
{
    NextRoom GetExitRoom();
    void SetupMirrors(string mirrorDefinition);
    bool GetNextRoom(int x, int y, Laser inputLaser, out NextRoom nextRoomInfo);
    Laser GetOutputLaser(Room room, Laser inputLaser);
}