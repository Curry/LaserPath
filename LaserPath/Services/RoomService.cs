using System.Text.RegularExpressions;
using LaserPath.Domain;
using LaserPath.Extensions;
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

        foreach (var mirror in inputFile.Mirrors)
            SetupMirrors(mirror);
    }
    
    public void SetupMirrors(string mirrorDefinition)
    {
        // Regex.Split adds empty strings at the head/tail due to pattern matching, so removing those will allow better parsing later
        var mirror = Regex.Split(mirrorDefinition, "([0-9]+),([0-9]+)([LRlr]{1})([LRlr]{0,1})")
            .Where(y => !string.IsNullOrWhiteSpace(y)).ToArray();
        var x = int.Parse(mirror[0]);
        var y = int.Parse(mirror[1]);
        
        // ranging operator allows array selection from 3rd element to end of array
        var mirrorState = mirror[2..];

        var orientation = mirrorState[0].ToMirrorOrientation();
        var reflection = MirrorReflection.Both;
        
        if (mirrorState.Length == 2)
            reflection = mirrorState[1].ToMirrorReflection();
        
        repository.SetupMirrors(x, y, orientation, reflection);
    }

    public NextRoom GetExitRoom()
    {
        int x = startX;
        int y = startY;
        var laser = startLaser;

        NextRoom nextRoom;
        
        // Loop until false, which means nextRoom is exiting the grid
        while (GetNextRoom(x, y, laser, out nextRoom))
        {
            // set current room to next rooms coordinates
            x = nextRoom.X;
            y = nextRoom.Y;
            laser = nextRoom.Laser;
        }
        
        return nextRoom;
    }

    public string GetConsoleOutput(NextRoom exitRoom)
    {
        return $"""
                Width: {width}, Height: {height}
                StartX: {startX}, StartY: {startY}, Laser: {startLaser.ToLaserDirection()}
                EndX: {exitRoom.X}, EndY: {exitRoom.Y}, Laser: {exitRoom.Laser.ToLaserDirection()}
                """;
    }

    public bool GetNextRoom(int x, int y, Laser inputLaser, out NextRoom nextRoomInfo)
    {
        // Get room by coordinates from repository
        var room = repository.GetRoom(x, y);
        // Get output laser based on room definition
        var outputLaser = GetOutputLaser(room, inputLaser);
        
        // Convert output laser into input laser based on grid location
        switch (outputLaser)
        {
            case Laser.Bottom:
                // If laser will not go through bottom edge of grid
                if (room.Y != 0)
                {
                    nextRoomInfo = new NextRoom(room.X, room.Y - 1, Laser.Top);
                    return true;
                }

                break;
            case Laser.Top:
                // If laser will not go through top edge of grid
                if (room.Y != height - 1)
                {
                    nextRoomInfo = new NextRoom(room.X, room.Y + 1, Laser.Bottom);
                    return true;
                }

                break;
            case Laser.Left:
                // if laser will not go through left edge of grid
                if (room.X != 0)
                {
                    nextRoomInfo = new NextRoom(room.X - 1, room.Y, Laser.Right);
                    return true;
                }

                break;
            case Laser.Right:
                // if laser will not go through right edge of grid
                if (room.X != width - 1)
                {
                    nextRoomInfo = new NextRoom(room.X + 1, room.Y, Laser.Left);
                    return true;
                }

                break;
        }

        // We are at edge of grid, which means we are exiting, set nextRoomInfo to X, Y coordinates of exit room, and use output laser as the direction of exit.
        nextRoomInfo = new NextRoom(room.X, room.Y, outputLaser);
        return false;
    }

    /*
     *  Get direction of output laser based on Mirror orientation and reflection, and return the direction the laser will exit
     *  Output laser direction is opposite the direction of input laser
     */
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