using System.Text.RegularExpressions;
using LaserPath.Domain;
using LaserPath.Repository;

namespace LaserPath.Services;

public class RoomService : IRoomService
{
    private readonly int height;
    private readonly int width;
    private readonly int startX;
    private readonly int startY;
    private readonly Laser startLaser;
    
    private readonly ILaserRepository repository;

    public RoomService(InputFile inputFile)
    {
        width = inputFile.Width;
        height = inputFile.Height;
        startX = inputFile.StartX;
        startY = inputFile.StartY;
        startLaser = inputFile.StartLaser;
        
        repository = new LaserRepository(width, height);

        foreach (var mirror in inputFile.Mirrors) SetupMirrors(mirror);
    }

    public void SetupMirrors(string mirrorDefinition)
    {
        var mirror = Regex.Split(mirrorDefinition, "([0-9]+),([0-9]+)([LRlr]{1})([LRlr]{0,1})")
            .Where(y => !string.IsNullOrWhiteSpace(y)).ToArray();
        var x = int.Parse(mirror[0]);
        var y = int.Parse(mirror[1]);

        repository.SetupMirrors(x, y, mirror[2..]);
    }

    public NextRoom GetExitRoom()
    {
        int x = startX;
        int y = startY;
        var laser = startLaser;

        NextRoom info;
        
        while (GetNextRoom(x, y, laser, out info))
        {
            x = info.X;
            y = info.Y;
            laser = info.Laser;
        }
        
        return info;
    }

    public bool GetNextRoom(int x, int y, Laser inputLaser, out NextRoom nextRoomInfo)
    {
        var room = repository.GetRoom(x, y);
        var outputLaser = GetOutputLaser(room, inputLaser);
        
        switch (outputLaser)
        {
            case Laser.Bottom:
                if (room.Y != 0)
                {
                    nextRoomInfo = new NextRoom(room.X, room.Y - 1, Laser.Top);
                    return true;
                }

                break;
            case Laser.Top:
                if (room.Y != height - 1)
                {
                    nextRoomInfo = new NextRoom(room.X, room.Y + 1, Laser.Bottom);
                    return true;
                }

                break;
            case Laser.Left:
                if (room.X != 0)
                {
                    nextRoomInfo = new NextRoom(room.X - 1, room.Y, Laser.Right);
                    return true;
                }

                break;
            case Laser.Right:
                if (room.X != width - 1)
                {
                    nextRoomInfo = new NextRoom(room.X + 1, room.Y, Laser.Left);
                    return true;
                }

                break;
        }

        nextRoomInfo = new NextRoom(room.X, room.Y, outputLaser);
        return false;
    }

    public Laser GetOutputLaser(Room room, Laser inputLaser)
    {
        switch (inputLaser)
        {
            case Laser.Left:
                /*
                 * ---> \
                 *      |
                 *      |
                 *      |
                 *      v
                 */
                if (room is
                    {
                        Orientation: MirrorOrientation.Left,
                        Reflection: MirrorReflection.Both or MirrorReflection.Left
                    })
                    return Laser.Bottom;
                /*
                 *      ^
                 *      |
                 *      |
                 *      |
                 * ---> /
                 */
                if (room is
                    {
                        Orientation: MirrorOrientation.Right,
                        Reflection: MirrorReflection.Both or MirrorReflection.Left
                    })
                    return Laser.Top;
                
                /*
                 * --->   --->
                 * ---> / --->
                 * ---> \ --->
                 */
                return Laser.Right;
            case Laser.Right:
                /*
                 * ^
                 * |
                 * |
                 * |
                 * \ <---
                 */
                if (room is
                    {
                        Orientation: MirrorOrientation.Left,
                        Reflection: MirrorReflection.Both or MirrorReflection.Right
                    })
                    return Laser.Top;

                /*
                 *\/ <---
                 * |
                 * |
                 * |
                 * v
                 */
                if (room is
                    {
                        Orientation: MirrorOrientation.Right,
                        Reflection: MirrorReflection.Both or MirrorReflection.Right
                    })
                    return Laser.Bottom;
                
                /*
                 * <---   <---
                 * <--- / <---
                 * <--- \ <---
                 */
                return Laser.Left;
            case Laser.Bottom:
                /*
                 * <--- \
                 *      ^
                 *      |
                 *      |
                 *      |
                 */
                if (room is
                    {
                        Orientation: MirrorOrientation.Left,
                        Reflection: MirrorReflection.Both or MirrorReflection.Left
                    })
                    return Laser.Left;
                /*
                 *\/ --->
                 * ^
                 * |
                 * |
                 * |
                 */
                if (room is
                    {
                        Orientation: MirrorOrientation.Right,
                        Reflection: MirrorReflection.Both or MirrorReflection.Right
                    })
                    return Laser.Right;
                /*
                 * ^   ^   ^  
                 * |   |   |
                 * |   |   |
                 *     \   /
                 * ^   ^   ^
                 * |   |   |
                 * |   |   |
                 */
                return Laser.Top;
            case Laser.Top:
                /*
                 * |
                 * |
                 * |
                 * v
                 * \ --->
                 */
                if (room is
                    {
                        Orientation: MirrorOrientation.Left,
                        Reflection: MirrorReflection.Both or MirrorReflection.Right
                    })
                    return Laser.Right;
                /*
                 *      |
                 *      |
                 *      |
                 *      v
                 * <--- \
                 */
                if (room is
                    {
                        Orientation: MirrorOrientation.Right,
                        Reflection: MirrorReflection.Both or MirrorReflection.Left
                    })
                    return Laser.Left;
                /*
                 * |   |   |
                 * |   |   |
                 * v   v   v
                 *     \   /
                 * |   |   |
                 * |   |   |
                 * v   v   v
                 */
                return Laser.Bottom;
        }

        throw new ArgumentOutOfRangeException(nameof(inputLaser), inputLaser, "Invalid Room/Laser orientation");
    }
}