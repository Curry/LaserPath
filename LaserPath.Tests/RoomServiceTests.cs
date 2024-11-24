using System.Collections;
using FluentAssertions;
using LaserPath.Domain;
using LaserPath.Services;

namespace LaserPath.Tests;

public class FullDataTestGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return
        [
            """
            5,4
            -1
            1,2RR
            3,2L
            -1
            1,0V
            -1
            """,
            new NextRoom(3, 0, Laser.Bottom)
        ];
        yield return
        [
            """
            5,4
            -1
            1,2RR
            3,2L
            -1
            0,2H
            -1
            """,
            new NextRoom(3, 0, Laser.Bottom)
        ];
        yield return
        [
            """
            5,4
            -1
            1,2RR
            3,2L
            3,1R
            1,1RL
            -1
            0,2H
            -1
            """,
            new NextRoom(0, 1, Laser.Left)
        ];
        yield return
        [
            """
            5,4
            -1
            1,0R
            1,2LR
            1,3RR
            4,3L
            -1
            0,0H
            -1
            """,
            new NextRoom(4, 0, Laser.Bottom)
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class LaserPathDataGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Laser Left
        yield return
        [
            Laser.Left,
            MirrorOrientation.Left,
            MirrorReflection.Both,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.Left,
            MirrorReflection.Left,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.Left,
            MirrorReflection.Right,
            Laser.Right
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.Right,
            MirrorReflection.Both,
            Laser.Top
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.Right,
            MirrorReflection.Left,
            Laser.Top
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.Right,
            MirrorReflection.Right,
            Laser.Right
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.None,
            MirrorReflection.None,
            Laser.Right
        ];
        
        // Laser Right
        yield return
        [
            Laser.Right,
            MirrorOrientation.Left,
            MirrorReflection.Both,
            Laser.Top
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.Left,
            MirrorReflection.Left,
            Laser.Left
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.Left,
            MirrorReflection.Right,
            Laser.Top
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.Right,
            MirrorReflection.Both,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.Right,
            MirrorReflection.Left,
            Laser.Left
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.Right,
            MirrorReflection.Right,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.None,
            MirrorReflection.None,
            Laser.Left
        ];
        
        // Laser Top
        yield return
        [
            Laser.Top,
            MirrorOrientation.Left,
            MirrorReflection.Both,
            Laser.Right
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.Left,
            MirrorReflection.Left,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.Left,
            MirrorReflection.Right,
            Laser.Right
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.Right,
            MirrorReflection.Both,
            Laser.Left
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.Right,
            MirrorReflection.Left,
            Laser.Left
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.Right,
            MirrorReflection.Right,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.None,
            MirrorReflection.None,
            Laser.Bottom
        ];
        
        // Laser Bottom
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Left,
            MirrorReflection.Both,
            Laser.Left
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Left,
            MirrorReflection.Left,
            Laser.Left
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Left,
            MirrorReflection.Right,
            Laser.Top
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Right,
            MirrorReflection.Both,
            Laser.Right
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Right,
            MirrorReflection.Left,
            Laser.Top
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Right,
            MirrorReflection.Right,
            Laser.Right
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.None,
            MirrorReflection.None,
            Laser.Top
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class RoomServiceTests
{
    [Theory]
    [ClassData(typeof(FullDataTestGenerator))]
    public void TestRoomServiceParser(string file, NextRoom expectedExitRoom)
    {
        var fileService = new InputFileReaderService();
        var parsedFile = fileService.ParseInputFileText(file);
        var service = new RoomService(parsedFile);

        var room = service.GetExitRoom();

        room.Should().BeEquivalentTo(expectedExitRoom);
    }

    [Theory]
    [ClassData(typeof(LaserPathDataGenerator))]
    public void TestOutputLaserPath(Laser laser, MirrorOrientation orientation, MirrorReflection reflection,
        Laser expectedLaser)
    {
        var room = new Room(0, 0)
        {
            Orientation = orientation,
            Reflection = reflection
        };

        var input = new InputFile(1, 1, [], 0, 0, Laser.Bottom);
        
        var service = new RoomService(input);

        var outputLaser = service.GetOutputLaser(room, laser);

        outputLaser.Should().Be(expectedLaser);
    }
}