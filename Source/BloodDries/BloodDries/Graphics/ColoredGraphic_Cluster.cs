using UnityEngine;
using Verse;

namespace BloodDries.Graphics;

public class ColoredGraphic_Cluster : Graphic_Cluster
{
    private readonly ColoredGraphic_ClusterImplementation _implementation;

    public ColoredGraphic_Cluster() => _implementation = new ColoredGraphic_ClusterImplementation(this);

    public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo) =>
        _implementation.GetColoredVersion(newShader, newColor, newColorTwo);
}
