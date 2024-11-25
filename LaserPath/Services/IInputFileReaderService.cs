using LaserPath.Domain;

namespace LaserPath.Services;

public interface IInputFileReaderService
{
    public Task<string> ReadInputFileAsync(string inputFilePath);
    InputFile ParseInputFileText(string fileContent);
}