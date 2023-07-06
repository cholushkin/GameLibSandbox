using GameLib.Random;
using UnityEngine.ProBuilder;

// todo:
public class RandomPrism : RandomShape
{
    public Range Steps;
    public Range SizeX;
    public Range SizeY;
    public Range SizeZ;

    protected override ProBuilderMesh Instantiate()
    {
        var steps = _rnd.FromRangeInt(Steps);
        var sizeX = _rnd.FromRange(SizeX);
        var sizeY = _rnd.FromRange(SizeY);
        var sizeZ = _rnd.FromRange(SizeZ);

        return ShapeGenerator.GenerateCurvedStair(PivotLocation.Center, 1f, 1f, 0.5f, 0.3f, 6, true);
    }
}
