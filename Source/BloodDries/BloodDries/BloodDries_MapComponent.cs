using RimWorld;
using System.Collections.Generic;
using Verse;

namespace BloodDries;

public class BloodDries_MapComponent(Map m) : MapComponent(m)
{
    public override void FinalizeInit()
    {
        base.FinalizeInit();

        if (Find.CurrentMap is not Map currentMap)
        {
            return;
        }

        ThingOwner allThings = currentMap.GetDirectlyHeldThings();
        List<Filth> bloodToTidyUp = [];

        foreach (Thing thing in allThings)
        {
            if ((thing.def == ThingDefOf.Filth_Blood || thing.def == ThingDefOf.Filth_BloodSmear) && thing is not Blood)
            {
                Logger.LogVerbose($"Found blood that is not of type {nameof(Blood)}: {thing} with def {thing.def.defName}");
                bloodToTidyUp.Add((Filth)thing);
            }
        }

        if (bloodToTidyUp.Count > 0)
        {
            Logger.Log($"Found {bloodToTidyUp.Count} instances of blood that have the wrong class. Replacing them with new copies.");
        }

        // now tidy them up
        foreach (Filth oldBlood in bloodToTidyUp)
        {
            Logger.LogVerbose($"Spawning new blood to replace {oldBlood}..");
            Filth newBlood = (Filth)ThingMaker.MakeThing(oldBlood.def, null);
            newBlood.AddSources(oldBlood.sources);
            GenSpawn.Spawn(newBlood, oldBlood.Position, map, WipeMode.Vanish);

            Logger.LogVerbose($"Destroying {oldBlood}..");
            oldBlood.Destroy(DestroyMode.Vanish);
        }
    }
}
