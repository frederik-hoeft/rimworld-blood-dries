using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Izzimon.BloodDries;

public class BloodDries_MapComponent(Map m) : MapComponent(m)
{
    public static bool DebuggingEnabled = false;

    public override void FinalizeInit()
    {
        base.FinalizeInit();

        if (Find.CurrentMap != null)
        {
            ThingOwner allThings = Find.CurrentMap.GetDirectlyHeldThings();
            List<Filth> bloodToTidyUp = [];

            for (int i = 0; i < allThings.Count; i++)
            {
                Thing thing = allThings[i];

                if (thing.def.defName == "Filth_Blood")
                {
                    if (thing is not Blood)
                    {
                        bloodToTidyUp.Add((Filth)thing);
                    }
                }
            }

            if (bloodToTidyUp.Count > 0)
            {
                Log.Message($"Found {bloodToTidyUp.Count} instances of blood that have the wrong class.  Replacing them with new copies.");
            }

            // now tidy them up
            foreach (Filth oldBlood in bloodToTidyUp)
            {
                Logger.Debug($"Spawning new blood to replace {oldBlood}..");
                Filth newBlood = (Filth)ThingMaker.MakeThing(oldBlood.def, null);
                newBlood.AddSources(oldBlood.sources);
                GenSpawn.Spawn(newBlood, oldBlood.Position, map, WipeMode.Vanish);

                Logger.Debug($"Destroying {oldBlood}..");
                oldBlood.Destroy(DestroyMode.Vanish);
            }
        }
    }
}
