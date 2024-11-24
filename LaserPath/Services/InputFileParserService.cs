using System.Text.RegularExpressions;

namespace LaserPath.Services;

public class InputFileReaderService : IInputFileReaderService
{
    public async Task<string> ReadInputFileAsync(string inputFilePath)
    {
        using var reader = new StreamReader(inputFilePath);

        var file = await reader.ReadToEndAsync();

        return file;
    }

    public InputFile ParseInputFileText(string fileContent)
    {
        var file = Regex.Split(fileContent, "-1").Select(x => x.Trim()).ToArray();
        var dimension = file[0].Split(",");
        
        var width = int.Parse(dimension[0]);
        var height = int.Parse(dimension[1]);

        var mirrors = Regex.Split(file[1], Environment.NewLine).ToArray();
        
        var start = Regex.Split(file[2], "([0-9]+),([0-9]+)([VvHh]{1})").Where(y => !string.IsNullOrWhiteSpace(y)).ToArray();
        
        var startX = int.Parse(start[0]);
        var startY = int.Parse(start[1]);
        
        var laser = start[2] switch
        {
            "V" when startY == 0 => Laser.Bottom,
            "V" when startY == height - 1 => Laser.Top,
            "H" when startX == 0 => Laser.Left,
            "H" when startX == width - 1 => Laser.Right,
            _ => throw new ArgumentException("Invalid Laser Start")
        };
        
        return new InputFile(width, height, mirrors, startX, startY, laser);
    }
}