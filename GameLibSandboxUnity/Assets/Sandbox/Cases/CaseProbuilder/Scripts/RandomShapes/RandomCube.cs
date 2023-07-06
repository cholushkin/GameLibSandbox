using GameLib.Random;
using UnityEngine;
using UnityEngine.ProBuilder;

public class RandomCube : RandomShape
{
    public Range SizeX;
    public Range SizeY;
    public Range SizeZ;

    protected override ProBuilderMesh Instantiate()
    {
        var sizeX = _rnd.FromRange(SizeX);
        var sizeY = _rnd.FromRange(SizeY);
        var sizeZ = _rnd.FromRange(SizeZ);

        return ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(sizeX, sizeY, sizeZ));
    }
}
