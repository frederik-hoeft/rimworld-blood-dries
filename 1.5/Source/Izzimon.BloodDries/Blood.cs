using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Izzimon.BloodDries;

public class Blood : Filth
{
    // to add to the def to configure later
    private static readonly int FreshBloodR = 131;
    private static readonly int FreshBloodG = 34;
    private static readonly int FreshBloodB = 34;

    private static readonly int DriedBloodR = 35;
    private static readonly int DriedBloodG = 14;
    private static readonly int DriedBloodB = 14;

    private static readonly int DefaultAlpha = 180;
    private static readonly int MinimumAlpha = 15;

    private static readonly int StandardTemperature = 20;
    private static readonly int DaysUntilFullyDryAtStandardTemperature = 3;

    private float _percentageDried = 0;
    private float _percentageEroded = 0;

    // we don't want to publish a whole range of shades, as that will fill up the graphic database 
    private static readonly float PublishPercentageStep = 0.20f;  // only publish every 20% step change
    private float _driedPublishPercentage = 0;
    private float _erodedPublishPercentage = 0;

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);

        if (!FilthMaker.TerrainAcceptsFilth(base.Map.terrainGrid.TerrainAt(base.Position), def))
        {
            // it will have been destroyed already, so do nothing.
            return;
        }

        if (respawningAfterLoad)
        {
            UpdatePublishPercentage("dryness", ref _driedPublishPercentage, _percentageDried);
            UpdatePublishPercentage("erosion", ref _erodedPublishPercentage, _percentageEroded);
        }

        Logger.Debug($"{this} will disappear in {DisappearAfterTicks / 60000f} days");
    }

    public override Color DrawColor
    {
        get
        {
            float red = GetWeightedAverage(FreshBloodR, DriedBloodR, _driedPublishPercentage) / 255;
            float green = GetWeightedAverage(FreshBloodG, DriedBloodG, _driedPublishPercentage) / 255;
            float blue = GetWeightedAverage(FreshBloodB, DriedBloodB, _driedPublishPercentage) / 255;
            float alpha = GetWeightedAverage(DefaultAlpha, MinimumAlpha, _erodedPublishPercentage) / 255;

            Color color = new(red, green, blue, alpha);

            return color;
        }
        set
        {
            Log.Error(string.Concat(new object[]
            {
                "Cannot set instance color on non-ThingWithComps ",
                LabelCap,
                " at ",
                Position,
                "."
            }));
        }
    }

    public override void TickLong()
    {
        bool changed = false;

        changed |= DryMore();

        changed |= ErodeMore();

        if (_percentageEroded == 1f)
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

        float temperatureModifier = 1f;
        if (AmbientTemperature <= 0)
        {
            return false;  // it is frozen, don't dry any more
        }
        else if (AmbientTemperature > StandardTemperature)
        {
            temperatureModifier += (AmbientTemperature - StandardTemperature) * 2 / StandardTemperature;  // add a bonus if the temperature is hot
        }

        float percentageMoreToDry = 2000f / (60000 * DaysUntilFullyDryAtStandardTemperature) * temperatureModifier;  // long tick = 2000 ticks.  one day in game time = 60000 ticks.

        _percentageDried += percentageMoreToDry;
        _percentageDried = Math.Min(_percentageDried, 1f);

        return UpdatePublishPercentage("dryness", ref _driedPublishPercentage, _percentageDried);
    }

    private bool ErodeMore()
    {
        float percentageMoreToErode = 2000f / DisappearAfterTicks;

        _percentageEroded += percentageMoreToErode;
        _percentageEroded = Math.Min(_percentageEroded, 1f);

        return UpdatePublishPercentage("erosion", ref _erodedPublishPercentage, _percentageEroded);
    }

    private bool UpdatePublishPercentage(string type, ref float publishPercentageProperty, float rawPercentage)
    {
        // work out what the publish percentage should be
        float publishPercentage = rawPercentage == 1f ? 1f : (float)(Math.Floor(rawPercentage / PublishPercentageStep) * PublishPercentageStep);

        Logger.Debug($"{this} {type} publish value should now be {publishPercentage} (raw value {rawPercentage})");

        if (publishPercentageProperty < publishPercentage)
        {
            Logger.Debug($"{this} {type} published new value {publishPercentage}");
            publishPercentageProperty = publishPercentage;
            return true;
        }

        return false;
    }

    private float GetWeightedAverage(float from, float to, float transitionPercentage)
    {
        float difference = (to - from) * transitionPercentage;
        return from + difference;
    }
}
