using System.Collections;
using LaserPath.Domain;

namespace LaserPath.Tests.DataGenerator;

public class RoomServiceFullDataTestGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return
        [
            """
            5,4
            -1
            1,2RR
            3,2L
            -1
            1,0V
            -1
            """,
            new NextRoom(3, 0, Laser.Bottom),
            """
            Width: 5, Height: 4
            StartX: 1, StartY: 0, Laser: V
            EndX: 3, EndY: 0, Laser: V
            """
        ];
        yield return
        [
            """
            5,4
            -1
            1,2RR
            3,2L
            -1
            0,2H
            -1
            """,
            new NextRoom(3, 0, Laser.Bottom),
            """
            Width: 5, Height: 4
            StartX: 0, StartY: 2, Laser: H
            EndX: 3, EndY: 0, Laser: V
            """
        ];
        yield return
        [
            """
            5,4
            -1
            1,2RR
            3,2L
            3,1R
            1,1RL
            -1
            0,2H
            -1
            """,
            new NextRoom(0, 1, Laser.Left),
            """
            Width: 5, Height: 4
            StartX: 0, StartY: 2, Laser: H
            EndX: 0, EndY: 1, Laser: H
            """
        ];
        yield return
        [
            """
            5,4
            -1
            1,0R
            1,2LR
            1,3RR
            4,3L
            -1
            0,0H
            -1
            """,
            new NextRoom(4, 0, Laser.Bottom),
            """
            Width: 5, Height: 4
            StartX: 0, StartY: 0, Laser: H
            EndX: 4, EndY: 0, Laser: V
            """
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}