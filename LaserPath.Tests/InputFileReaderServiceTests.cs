using FluentAssertions;
using LaserPath.Domain;
using LaserPath.Services;

namespace LaserPath.Tests;

public class InputFileReaderServiceTests
{
    [Fact]
    public void TestValidFileParsesCorrectly()
    {
        var input = """
                    5,4
                    -1
                    1,2RR
                    3,2L
                    -1
                    0,2H
                    -1
                    """;

        var service = new InputFileReaderService();

        var action = () => service.ParseInputFileText(input);

        action.Should().NotThrow();

        var result = action();

        result.Should().BeEquivalentTo(new InputFile(5, 4, ["1,2RR", "3,2L"], 0, 2, Laser.Left));
    }
    
    [Fact]
    public void TestInValidFileThrowsException()
    {
        var input = """
                    5,4
                    -1
                    1,
                    """;

        var service = new InputFileReaderService();

        var action = () => service.ParseInputFileText(input);

        action.Should().Throw<ArgumentException>();
    }
}