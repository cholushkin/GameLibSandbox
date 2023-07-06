using GameLib.Random;
using UnityEngine;
using UnityEngine.ProBuilder;

public class RandomStair: RandomShape
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

        return ShapeGenerator.GenerateStair(PivotLocation.Center, new Vector3(sizeX, sizeY, sizeZ), steps, true);
    }
}
