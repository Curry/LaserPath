using System.Text.RegularExpressions;
using LaserPath.Domain;

namespace LaserPath.Services;

public class InputFileReaderService : IInputFileReaderService
{
    // Read file only in function to make sure StreamReader is disposed immediately after reading.
    public async Task<string> ReadInputFileAsync(string inputFilePath)
    {
        using var reader = new StreamReader(inputFilePath);

        return await reader.ReadToEndAsync();;
    }

    public InputFile ParseInputFileText(string fileContent)
    {
        // Make sure file is in correct format, fail gracefully and provide information
        try
        {
            // Split by the delimiter '-1', and then remove trailing/leading "\r\n"
            var file = Regex.Split(fileContent, "-1").Select(x => x.Trim()).ToArray();
            // Split by ',' to get input width and height
            var dimension = file[0].Split(",");

            var width = int.Parse(dimension[0]);
            var height = int.Parse(dimension[1]);

            var mirrors = Regex.Split(file[1], Environment.NewLine).ToArray();

            var start = Regex.Split(file[2], "([0-9]+),([0-9]+)([VvHh]{1})").Where(y => !string.IsNullOrWhiteSpace(y))
                .ToArray();

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
        catch (Exception e)
        {
            throw new ArgumentException($"Failed to parse input file: \n{fileContent}", e);
        }
    }
}