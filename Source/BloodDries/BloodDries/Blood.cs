using BloodDries.Extensions;
using RimWorld;
using UnityEngine;
using Verse;

namespace BloodDries;

public class Blood : Filth
{
    // RGBA 131, 34, 34, 180
    private static readonly Color _freshBloodColor = new(131f / 255f, 34f / 255f, 34f / 255f, 180f / 255f);
    // RGBA 35, 14, 14, 15
    private static readonly Color _driedBloodColor = new(35f / 255f, 14f / 255f, 14f / 255f, 15f / 255f);

    // we don't want to publish a whole range of shades, as that will fill up the graphic database 
    private const float PublishPercentageStep = 0.20f;  // only publish every 20% step change

    private float _percentageDried = 0;
    private float _percentageEroded = 0;
    private float _driedPublishPercentage = 0;
    private float _erodedPublishPercentage = 0;

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);

        if (!FilthMaker.TerrainAcceptsFilth(Map.terrainGrid.TerrainAt(Position), def))
        {
            // it will have been destroyed already, so do nothing.
            return;
        }

        if (respawningAfterLoad)
        {
            UpdatePublishPercentage("dryness", ref _driedPublishPercentage, _percentageDried);
            UpdatePublishPercentage("erosion", ref _erodedPublishPercentage, _percentageEroded);
        }

        Logger.LogVerbose($"{this} will disappear in {DisappearAfterTicks / 60000f} days");
    }

    public override Color DrawColor
    {
        get => _freshBloodColor.Lerp(_driedBloodColor, percentage: _driedPublishPercentage, alphaPercentage: _erodedPublishPercentage);
        set => Log.Error($"[{nameof(BloodDries)}] Cannot set instance color on non-ThingWithComps {LabelCap} at {Position}.");
    }

    public override void TickLong()
    {
        bool changed = false;
        changed |= DryMore();
        changed |= ErodeMore();
        if (_percentageEroded >= 1f)
        {
            Destroy(DestroyMode.Vanish);
        }
        else if (changed)
        {
            Notify_ColorChanged();
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref _percentageDried, "percentageDried", 0f, false);
        Scribe_Values.Look(ref _percentageEroded, "percentageEroded", 0f, false);
    }

    private bool DryMore()
    {
        if (_percentageDried == 1f)
        {
            return false;
        }

        float temperatureMultiplier = 1f;
        float belowFreezingPenalty = 0f;
        if (AmbientTemperature <= 0)
        {
            // it is frozen, apply a penalty (default: no drying at all)
            belowFreezingPenalty = BloodDriesMod.Settings.belowFreezingPenalty;
        }
        // invert the penalty to get a multiplier
        float belowFreezingMultiplier = 1f - belowFreezingPenalty;
        float standardTemperature = BloodDriesMod.Settings.standardTemperature;
        if (AmbientTemperature > standardTemperature)
        {
            // the hotter it is, the faster it dries
            temperatureMultiplier += (AmbientTemperature - standardTemperature) * 2f / standardTemperature;
        }

        // long tick = 2000 ticks.  one day in game time = 60000 ticks.
        float preMultipliers = 2000f / (60000f * BloodDriesMod.Settings.daysUntilFullyDryAtStandardTemperature);
        float percentageMoreToDry = preMultipliers * temperatureMultiplier * belowFreezingMultiplier;

        _percentageDried = Mathf.Clamp01(_percentageDried + percentageMoreToDry);

        return UpdatePublishPercentage("dryness", ref _driedPublishPercentage, _percentageDried);
    }

    private bool ErodeMore()
    {
        float percentageMoreToErode = 2000f / DisappearAfterTicks;

        _percentageEroded = Mathf.Clamp01(_percentageEroded + percentageMoreToErode);

        return UpdatePublishPercentage("erosion", ref _erodedPublishPercentage, _percentageEroded);
    }

    private bool UpdatePublishPercentage(string type, ref float publishPercentageProperty, float rawPercentage)
    {
        // work out what the publish percentage should be
        float publishPercentage = rawPercentage >= 1f ? 1f : (float)(Math.Floor(rawPercentage / PublishPercentageStep) * PublishPercentageStep);

        Logger.LogVerbose($"{this} at {Position} {type} publish value should now be {publishPercentage} (raw value {rawPercentage})");

        if (publishPercentageProperty < publishPercentage)
        {
            Logger.LogVerbose($"{this} {type} published new value {publishPercentage}");
            publishPercentageProperty = publishPercentage;
            return true;
        }

        return false;
    }
}
