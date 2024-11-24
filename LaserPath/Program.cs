// See https://aka.ms/new-console-template for more information

using LaserPath.Services;

var inputService = new InputFileReaderService();

var fileContent = await inputService.ReadInputFileAsync("LaserPath.txt");
var parsedFile = inputService.ParseInputFileText(fileContent);

var service = new RoomService(parsedFile);

var exitRoom = service.GetExitRoom();

Console.WriteLine($"{exitRoom.X}, {exitRoom.Y}, {exitRoom.Laser.ToString()}");
