using System.Text.RegularExpressions;

namespace LaserPath;

public record InputFile(int Width, int Height, string[] Mirrors, int StartX, int StartY, Laser StartLaser);