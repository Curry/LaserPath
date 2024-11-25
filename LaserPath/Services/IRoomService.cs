using LaserPath.Domain;

namespace LaserPath.Services;

public interface IRoomService
{
    NextRoom CalculateExitRoom();
    void SetupMirrors();
    bool GetNextRoom(int x, int y, Laser inputLaser, out NextRoom nextRoomInfo);
    Laser GetOutputLaser(Room room, Laser inputLaser);
}