using LaserPath.Repository;
using LaserPath.Services;

if (args.Length != 1)
    throw new ArgumentException("Usage: LaserPath <input-file>");

var path = args[0];

if (!Path.Exists(path))
    throw new ArgumentException("Invalid input file path");

var inputService = new InputFileReaderService();

var fileContent = await inputService.ReadInputFileAsync(path);

var parsedFile = inputService.ParseInputFileText(fileContent);

var repository = new LaserRepository(parsedFile);

var service = new RoomService(repository);

var exitRoom = service.CalculateExitRoom();

Console.WriteLine(service.GetConsoleOutput(exitRoom));
