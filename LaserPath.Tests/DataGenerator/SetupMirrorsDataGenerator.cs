
using System.Collections;
using LaserPath.Domain;

namespace LaserPath.Tests.DataGenerator;

public class SetupMirrorsDataGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return
        [
            "0,0L",
            (0, 0, MirrorOrientation.Left, MirrorReflection.Both)
        ];
        yield return
        [
            "0,1LR",
            (0, 1, MirrorOrientation.Left, MirrorReflection.Right)
        ];
        yield return
        [
            "0,2LL",
            (0, 2, MirrorOrientation.Left, MirrorReflection.Left)
        ];
        yield return
        [
            "1,0R",
            (1, 0, MirrorOrientation.Right, MirrorReflection.Both)
        ];
        yield return
        [
            "1,1RR",
            (1, 1, MirrorOrientation.Right, MirrorReflection.Right)
        ];
        yield return
        [
            "1,2RL",
            (1, 2, MirrorOrientation.Right, MirrorReflection.Left)
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}