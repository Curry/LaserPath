using LaserPath.Domain;

namespace LaserPath.Extensions;

public static class LaserPathExtensions
{
    public static string ToLaserDirection(this Laser laser)
    {
        return laser switch
        {
            Laser.Bottom or Laser.Top => "V",
            Laser.Left or Laser.Right => "H",
            _ => throw new ArgumentOutOfRangeException(nameof(laser), laser, null)
        };
    }

    public static MirrorOrientation ToMirrorOrientation(this string orientation)
    {
        return orientation switch
        {
            "L" => MirrorOrientation.Left,
            "R" => MirrorOrientation.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
        };
    }
    
    public static MirrorReflection ToMirrorReflection(this string orientation)
    {
        return orientation switch
        {
            "L" => MirrorReflection.Left,
            "R" => MirrorReflection.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
        };
    }
}