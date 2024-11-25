using System.Collections;
using LaserPath.Domain;

namespace LaserPath.Tests.DataGenerator;

public class NextRoomDataGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // 0, 0
        yield return
        [
            0,
            0,
            Laser.Left,
            true,
            new NextRoom(1, 0, Laser.Left),
        ];
        yield return
        [
            0,
            0,
            Laser.Right,
            false,
            new NextRoom(0, 0, Laser.Left),
        ];
        yield return
        [
            0,
            0,
            Laser.Top,
            false,
            new NextRoom(0, 0, Laser.Bottom),
        ];
        yield return
        [
            0,
            0,
            Laser.Bottom,
            true,
            new NextRoom(0, 1, Laser.Bottom),
        ];
        
        // 1, 0
        yield return
        [
            1,
            0,
            Laser.Left,
            false,
            new NextRoom(1, 0, Laser.Right),
        ];
        yield return
        [
            1,
            0,
            Laser.Right,
            true,
            new NextRoom(0, 0, Laser.Right),
        ];
        yield return
        [
            1,
            0,
            Laser.Top,
            false,
            new NextRoom(1, 0, Laser.Bottom),
        ];
        yield return
        [
            1,
            0,
            Laser.Bottom,
            true,
            new NextRoom(1, 1, Laser.Bottom),
        ];
        
        // 0, 1
        yield return
        [
            0,
            1,
            Laser.Left,
            true,
            new NextRoom(1, 1, Laser.Left),
        ];
        yield return
        [
            0,
            1,
            Laser.Right,
            false,
            new NextRoom(0, 1, Laser.Left),
        ];
        yield return
        [
            0,
            1,
            Laser.Top,
            true,
            new NextRoom(0, 0, Laser.Top),
        ];
        yield return
        [
            0,
            1,
            Laser.Bottom,
            false,
            new NextRoom(0, 1, Laser.Top),
        ];
        
        // 1, 1
        yield return
        [
            1,
            1,
            Laser.Left,
            false,
            new NextRoom(1, 1, Laser.Right),
        ];
        yield return
        [
            1,
            1,
            Laser.Right,
            true,
            new NextRoom(0, 1, Laser.Right),
        ];
        yield return
        [
            1,
            1,
            Laser.Top,
            true,
            new NextRoom(1, 0, Laser.Top),
        ];
        yield return
        [
            1,
            1,
            Laser.Bottom,
            false,
            new NextRoom(1, 1, Laser.Top),
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}