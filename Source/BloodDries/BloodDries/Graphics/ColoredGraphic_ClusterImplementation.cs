using UnityEngine;
using Verse;

namespace BloodDries.Graphics;

internal readonly struct ColoredGraphic_ClusterImplementation(Graphic_Cluster parentCluster)
{
    public Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
    {
        Logger.LogVerbose($"{nameof(ColoredGraphic_ClusterImplementation)}.{nameof(GetColoredVersion)} called with color {newColor} for cluster type {parentCluster.GetType().Name}");

        ColoredGraphic_Cluster coloredVersion = new();
        GraphicRequest graphicRequest = new()
        {
            path = parentCluster.path,
            maskPath = parentCluster.maskPath,
            color = newColor,
            colorTwo = newColorTwo,
            drawSize = parentCluster.drawSize,
            graphicData = parentCluster.data,
            shader = newShader
        };

        coloredVersion.Init(graphicRequest);

        return coloredVersion;
    }
}