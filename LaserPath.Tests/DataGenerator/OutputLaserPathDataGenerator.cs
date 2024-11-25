using System.Collections;
using LaserPath.Domain;

namespace LaserPath.Tests.DataGenerator;

public class OutputLaserPathDataGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Laser Left
        yield return
        [
            Laser.Left,
            MirrorOrientation.Left,
            MirrorReflection.Both,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.Left,
            MirrorReflection.Left,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.Left,
            MirrorReflection.Right,
            Laser.Right
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.Right,
            MirrorReflection.Both,
            Laser.Top
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.Right,
            MirrorReflection.Left,
            Laser.Top
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.Right,
            MirrorReflection.Right,
            Laser.Right
        ];
        yield return
        [
            Laser.Left,
            MirrorOrientation.None,
            MirrorReflection.None,
            Laser.Right
        ];
        
        // Laser Right
        yield return
        [
            Laser.Right,
            MirrorOrientation.Left,
            MirrorReflection.Both,
            Laser.Top
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.Left,
            MirrorReflection.Left,
            Laser.Left
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.Left,
            MirrorReflection.Right,
            Laser.Top
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.Right,
            MirrorReflection.Both,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.Right,
            MirrorReflection.Left,
            Laser.Left
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.Right,
            MirrorReflection.Right,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Right,
            MirrorOrientation.None,
            MirrorReflection.None,
            Laser.Left
        ];
        
        // Laser Top
        yield return
        [
            Laser.Top,
            MirrorOrientation.Left,
            MirrorReflection.Both,
            Laser.Right
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.Left,
            MirrorReflection.Left,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.Left,
            MirrorReflection.Right,
            Laser.Right
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.Right,
            MirrorReflection.Both,
            Laser.Left
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.Right,
            MirrorReflection.Left,
            Laser.Left
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.Right,
            MirrorReflection.Right,
            Laser.Bottom
        ];
        yield return
        [
            Laser.Top,
            MirrorOrientation.None,
            MirrorReflection.None,
            Laser.Bottom
        ];
        
        // Laser Bottom
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Left,
            MirrorReflection.Both,
            Laser.Left
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Left,
            MirrorReflection.Left,
            Laser.Left
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Left,
            MirrorReflection.Right,
            Laser.Top
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Right,
            MirrorReflection.Both,
            Laser.Right
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Right,
            MirrorReflection.Left,
            Laser.Top
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.Right,
            MirrorReflection.Right,
            Laser.Right
        ];
        yield return
        [
            Laser.Bottom,
            MirrorOrientation.None,
            MirrorReflection.None,
            Laser.Top
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}