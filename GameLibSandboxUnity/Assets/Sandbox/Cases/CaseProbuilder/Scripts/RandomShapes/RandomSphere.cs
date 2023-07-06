using GameLib.Random;
using UnityEngine;
using UnityEngine.ProBuilder;

public class RandomSphere : RandomShape
{
    public Range Radius;
    public Range Subdivisions;

    protected override ProBuilderMesh Instantiate()
    {
        var radius = _rnd.FromRange(Radius);
        var subdivisions = _rnd.FromRangeInt(Subdivisions);

        return ShapeGenerator.GenerateIcosahedron(PivotLocation.Center, radius, subdivisions);
    }
}
