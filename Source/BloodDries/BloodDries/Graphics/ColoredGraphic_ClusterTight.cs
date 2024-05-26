using UnityEngine;
using Verse;

namespace BloodDries.Graphics;

public class ColoredGraphic_ClusterTight : Graphic_ClusterTight
{
    private readonly ColoredGraphic_ClusterImplementation _implementation;

    public ColoredGraphic_ClusterTight() => _implementation = new ColoredGraphic_ClusterImplementation(this);

    public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo) =>
        _implementation.GetColoredVersion(newShader, newColor, newColorTwo);
}
