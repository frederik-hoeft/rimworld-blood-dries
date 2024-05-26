using Verse;

namespace BloodDries;

public class BloodDriesSettings : ModSettings
{
    // general
    internal bool enableLogging = false;
    internal bool enableVerboseLogging = false;

    // drying
    internal float standardTemperature = 20f;
    internal float daysUntilFullyDryAtStandardTemperature = 3f;
    internal float belowFreezingPenalty = 1f;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref enableLogging, nameof(enableLogging), false);
        Scribe_Values.Look(ref enableVerboseLogging, nameof(enableVerboseLogging), false);

        Scribe_Values.Look(ref standardTemperature, nameof(standardTemperature), 20f);
        Scribe_Values.Look(ref daysUntilFullyDryAtStandardTemperature, nameof(daysUntilFullyDryAtStandardTemperature), 3f);
        Scribe_Values.Look(ref belowFreezingPenalty, nameof(belowFreezingPenalty), 1f);
    }
}
