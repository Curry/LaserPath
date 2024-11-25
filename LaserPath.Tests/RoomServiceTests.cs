using FluentAssertions;
using LaserPath.Domain;
using LaserPath.Repository;
using LaserPath.Services;
using LaserPath.Tests.DataGenerator;

namespace LaserPath.Tests;

public class RoomServiceTests
{
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
}