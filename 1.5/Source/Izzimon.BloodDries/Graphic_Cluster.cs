
using UnityEngine;
using Verse;

namespace Izzimon.BloodDries;

public class Graphic_Cluster : Verse.Graphic_Cluster
{
    public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
    {
        Logger.Debug($"GetColoredVersion called with color {newColor}");

        Graphic_Cluster coloredVersion = new();
        GraphicRequest graphicRequest = new()
        {
            path = path,
            maskPath = maskPath,
            color = newColor,
            colorTwo = newColorTwo,
            drawSize = drawSize,
            graphicData = data,
            shader = newShader
        };

        coloredVersion.Init(graphicRequest);

        return coloredVersion;
    }
}
