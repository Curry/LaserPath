using System.Collections;
using LaserPath.Domain;

namespace LaserPath.Tests.DataGenerator;

public class RoomServiceFullDataGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Verify Exit Bottom
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
            Dimensions: Width: 5, Height: 4
            Laser Start: (1, 0), Orientation: V
            Laser Exit: (3, 0), Orientation: V
            """
        ];
        // Verify Exit Top
        yield return
        [
            """
            5,4
            -1
            1,2RR
            3,2RL
            -1
            1,0V
            -1
            """,
            new NextRoom(3, 3, Laser.Top),
            """
            Dimensions: Width: 5, Height: 4
            Laser Start: (1, 0), Orientation: V
            Laser Exit: (3, 3), Orientation: V
            """
        ];
        // Verify exit Right
        yield return
        [
            """
            5,4
            -1
            1,2RR
            -1
            1,0V
            -1
            """,
            new NextRoom(4, 2, Laser.Right),
            """
            Dimensions: Width: 5, Height: 4
            Laser Start: (1, 0), Orientation: V
            Laser Exit: (4, 2), Orientation: H
            """
        ];
        // Verify Exit Bottom
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
            Dimensions: Width: 5, Height: 4
            Laser Start: (0, 2), Orientation: H
            Laser Exit: (3, 0), Orientation: V
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
            Dimensions: Width: 5, Height: 4
            Laser Start: (0, 2), Orientation: H
            Laser Exit: (0, 1), Orientation: H
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
            Dimensions: Width: 5, Height: 4
            Laser Start: (0, 0), Orientation: H
            Laser Exit: (4, 0), Orientation: V
            """
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}