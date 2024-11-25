using FluentAssertions;
using LaserPath.Domain;
using LaserPath.Services;
using LaserPath.Tests.DataGenerator;

namespace LaserPath.Tests;

public class RoomServiceTests
{
    [Theory]
    [ClassData(typeof(RoomServiceFullDataTestGenerator))]
    public void TestRoomServiceFull(string file, NextRoom expectedExitRoom, string expectedOutputString)
    {
        var fileService = new InputFileReaderService();
        var parsedFile = fileService.ParseInputFileText(file);
        var service = new RoomService(parsedFile);

        var room = service.GetExitRoom();

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
        
        var service = new RoomService(input);

        var outputLaser = service.GetOutputLaser(room, laser);

        outputLaser.Should().Be(expectedLaser);
    }
}