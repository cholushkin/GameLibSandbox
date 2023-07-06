using GameLib.Random;
using UnityEngine.ProBuilder;

public class RandomCylinder : RandomShape
{
    public Range Radius;
    public Range AxisDivisions;
    public Range Height;
    public Range HeightCuts;

    protected override ProBuilderMesh Instantiate()
    {
        return ShapeGenerator.GenerateCylinder(
            PivotLocation.Center, 
            _rnd.FromRangeInt(AxisDivisions),
            _rnd.FromRange(Radius),
            _rnd.FromRange(Height),
            _rnd.FromRangeInt(HeightCuts),
            -1);
    }
}