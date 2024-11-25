// See https://aka.ms/new-console-template for more information

using LaserPath.Services;

if (args.Length != 1)
    throw new ArgumentException("Usage: LaserPath <input-file>");

var path = args[0];

if (!Path.Exists(path))
    throw new ArgumentException("Invalid input file path");

var inputService = new InputFileReaderService();

var fileContent = await inputService.ReadInputFileAsync(path);

var parsedFile = inputService.ParseInputFileText(fileContent);

var service = new RoomService(parsedFile);

var exitRoom = service.GetExitRoom();

Console.WriteLine(service.GetConsoleOutput(exitRoom));
