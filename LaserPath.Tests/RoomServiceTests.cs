using FluentAssertions;
using LaserPath.Domain;
using LaserPath.Repository;
using LaserPath.Services;
using LaserPath.Tests.DataGenerator;

namespace LaserPath.Tests;

public class RoomServiceTests
{
    // Tests sample input files
    [Theory]
    [ClassData(typeof(RoomServiceFullDataGenerator))]
    public void TestRoomServiceFull(string file, NextRoom expectedExitRoom, string expectedOutputString)
    {
        var fileService = new InputFileReaderService();
        var parsedFile = fileService.ParseInputFileText(file);
        var repository = new LaserRepository(parsedFile);
        var service = new RoomService(repository);

        var room = service.CalculateExitRoom();

        var output = service.GetConsoleOutput(room);

        room.Should().BeEquivalentTo(expectedExitRoom);
        
        output.Should().BeEquivalentTo(expectedOutputString);
    }

    // Verifies all combination of orientation,reflection with input laser direction returns valid output laser direction
    [Theory]
    [ClassData(typeof(OutputLaserPathDataGenerator))]
    public void TestOutputLaserPath(Laser laser, MirrorOrientation orientation, MirrorReflection reflection,
        Laser expectedLaser)
    {
        var room = new Room(0, 0)
        {
            Orientation = orientation,
            Reflection = reflection
        };

        var input = new InputFile(1, 1, [], 0, 0, Laser.Bottom);
        
        var repository = new LaserRepository(input);
        
        var service = new RoomService(repository);

        var outputLaser = service.GetOutputLaser(room, laser);

        outputLaser.Should().Be(expectedLaser);
    }

    // Verifies mirrors set up properly with given text
    [Theory]
    [ClassData(typeof(SetupMirrorsDataGenerator))]
    public void TestSetupMirrors(string mirror,
        (int x, int y, MirrorOrientation orientation, MirrorReflection reflection) expectedMirror)
    {
        var input = new InputFile(2, 3, [mirror], 0, 0, Laser.Bottom);
        
        var repository = new LaserRepository(input);
        
        var service = new RoomService(repository);
        
        service.SetupMirrors();
        
        var room = repository.GetRoom(expectedMirror.x, expectedMirror.y);
        room.Orientation.Should().Be(expectedMirror.orientation);
        room.Reflection.Should().Be(expectedMirror.reflection);
    }

    // Verifies laser gets next room or exit properly, based on 2x2 grid, combination of all 4 rooms and all 4 laser directions
    [Theory]
    [ClassData(typeof(NextRoomDataGenerator))]
    public void TestGetNextRoom(int x, int y, Laser inputLaser, bool expectedIsNotEdge, NextRoom expectedNextRoom)
    {
        var input = new InputFile(2, 2, [], x, y, inputLaser);
        
        var repository = new LaserRepository(input);
        var service = new RoomService(repository);

        var isNotEdge = service.GetNextRoom(x, y, inputLaser, out var nextRoom);

        isNotEdge.Should().Be(expectedIsNotEdge);
        
        nextRoom.Should().BeEquivalentTo(expectedNextRoom);
    }
}