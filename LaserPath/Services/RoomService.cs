using System.Text.RegularExpressions;
using LaserPath.Domain;
using LaserPath.Extensions;
using LaserPath.Repository;

namespace LaserPath.Services;

public class RoomService(ILaserRepository repository) : IRoomService
{
    public NextRoom CalculateExitRoom()
    {
        SetupMirrors();

        var x = repository.StartX;
        var y = repository.StartY;
        var laser = repository.StartLaser;
        
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
                Width: {repository.Width}, Height: {repository.Height}
                StartX: {repository.StartX}, StartY: {repository.StartY}, Laser: {repository.StartLaser.ToLaserDirection()}
                EndX: {exitRoom.X}, EndY: {exitRoom.Y}, Laser: {exitRoom.Laser.ToLaserDirection()}
                """;
    }
    
    public void SetupMirrors()
    {
        foreach (var mirrorDefinition in repository.Mirrors)
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
    }

    public bool GetNextRoom(int x, int y, Laser inputLaser, out NextRoom nextRoomInfo)
    {
        // Get output laser based on room
        var outputLaser = GetOutputLaser(x, y, inputLaser);
        
        // Convert output laser into input laser based on grid location
        switch (outputLaser)
        {
            case Laser.Bottom:
                // If laser will not go through bottom edge of grid
                if (y != 0)
                {
                    nextRoomInfo = new NextRoom(x, y - 1, Laser.Top);
                    return true;
                }

                break;
            case Laser.Top:
                // If laser will not go through top edge of grid
                if (y != repository.Height - 1)
                {
                    nextRoomInfo = new NextRoom(x, y + 1, Laser.Bottom);
                    return true;
                }

                break;
            case Laser.Left:
                // if laser will not go through left edge of grid
                if (x != 0)
                {
                    nextRoomInfo = new NextRoom(x - 1, y, Laser.Right);
                    return true;
                }

                break;
            case Laser.Right:
                // if laser will not go through right edge of grid
                if (x != repository.Width - 1)
                {
                    nextRoomInfo = new NextRoom(x + 1, y, Laser.Left);
                    return true;
                }

                break;
        }

        // We are at edge of grid, which means we are exiting, set nextRoomInfo to X, Y coordinates of exit room, and use output laser as the direction of exit.
        nextRoomInfo = new NextRoom(x, y, outputLaser);
        return false;
    }

    /*
     *  Get direction of output laser based on Mirror orientation and reflection, and return the direction the laser will exit
     *  Note: Output laser direction is NOT the direction of input laser for the next room
     */
    public Laser GetOutputLaser(int x, int y, Laser inputLaser)
    {
        var room = repository.GetRoom(x, y);
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
            default:
                // This is here to make sure all cases are covered, but as the use cases are exhaustive, this should never be reached
                // Room will never have a reflection without orientation
                throw new ArgumentOutOfRangeException(nameof(inputLaser), inputLaser, "Invalid Room/Laser orientation");
        }
    }
}