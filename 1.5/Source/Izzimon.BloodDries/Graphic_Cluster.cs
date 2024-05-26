using UnityEngine;
using Verse;

namespace Izzimon.BloodDries;

/*
CloneColored not implemented on this subclass of Graphic: Verse.Graphic_ClusterTight
UnityEngine.StackTraceUtility:ExtractStackTrace ()
Verse.Log:Error (string)
Verse.Log:ErrorOnce (string,int)
Verse.Graphic:GetColoredVersion (UnityEngine.Shader,UnityEngine.Color,UnityEngine.Color)
Verse.GraphicData:GraphicColoredFor (Verse.Thing)
Verse.Thing:get_DefaultGraphic ()
Verse.Thing:get_Graphic ()
Verse.Thing:Print (Verse.SectionLayer)
Verse.SectionLayer_ThingsGeneral:TakePrintFrom (Verse.Thing)
Verse.SectionLayer_Things:Regenerate ()
Verse.Section:TryUpdate (Verse.CellRect)
Verse.MapDrawer:MapMeshDrawerUpdate_First ()
Verse.Map:MapUpdate ()
Verse.Game:UpdatePlay ()
Verse.Root_Play:Update ()
*/

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
