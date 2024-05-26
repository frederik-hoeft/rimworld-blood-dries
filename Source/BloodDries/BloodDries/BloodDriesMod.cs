using UnityEngine;
using Verse;

namespace BloodDries;

public class BloodDriesMod : Mod
{
    public static BloodDriesSettings Settings { get; private set; } = null!;

    public BloodDriesMod(ModContentPack content) : base(content)
    {
        Settings = GetSettings<BloodDriesSettings>();
    }

    private Vector2 _scrollPosition;
    private const float _minContentHeight = 256f;
    private float _knownContentHeight = _minContentHeight;
    private bool _requiresScrolling = false;

    public override void DoSettingsWindowContents(Rect canvas)
    {
        base.DoSettingsWindowContents(canvas);
        if (Settings is null)
        {
            Logger.Error("Settings was null!");
            throw new ArgumentNullException(nameof(Settings));
        }
        float scrollbarMargin = _requiresScrolling ? 25f : 0f;
        Listing_Standard list = new()
        {
            ColumnWidth = (canvas.width - scrollbarMargin - 17) / 2,
        };
        Rect content = new(canvas.x, canvas.y, canvas.width - scrollbarMargin, _knownContentHeight);
        Rect view = new(canvas.x, canvas.y, canvas.width, canvas.height);
        if (_requiresScrolling)
        {
            Widgets.BeginScrollView(view, ref _scrollPosition, content);
        }
        list.Begin(content);
        Text.Font = GameFont.Medium;
        list.Label("General settings");
        Text.Font = GameFont.Small;
        list.CheckboxLabeled("Is active", ref Settings.isActive);
        list.CheckboxLabeled("Enable logging", ref Settings.enableLogging);
        list.CheckboxLabeled("Enable verbose logging", ref Settings.enableVerboseLogging);
        list.NewColumn();
        // time settings
        Text.Font = GameFont.Medium;
        list.Label("Time until fully dried");
        Text.Font = GameFont.Small;
        list.Label($"How long it takes for blood to fully dry at the standard temperature. (default: {DaysToDisplayString(3f)})");
        list.Label($"Current value: {DaysToDisplayString(Settings.daysUntilFullyDryAtStandardTemperature)}");
        Settings.daysUntilFullyDryAtStandardTemperature = list.Slider(val: Settings.daysUntilFullyDryAtStandardTemperature, min: 1f / 48f, max: 15f);
        list.GapLine();
        // temperature settings
        Text.Font = GameFont.Medium;
        list.Label("Standard temperature");
        Text.Font = GameFont.Small;
        list.Label("The temperature at which blood dries at the default rate. If the temperature surpasses this threshold, blood will dry exponentially faster. (default: 20.0)");
        list.Label($"Current value: {Settings.standardTemperature:F1}");
        Settings.standardTemperature = list.Slider(val: Settings.standardTemperature, min: 0f, max: 50f);
        list.GapLine();
        Text.Font = GameFont.Medium;
        list.Label("Below freezing penalty");
        Text.Font = GameFont.Small;
        list.Label("The penalty applied to the drying rate of blood when the temperature is below freezing. 0 means freezing temperatures have no effect, 1 means blood never dries when frozen. (default: 1.0)");
        list.Label($"Current value: {Settings.belowFreezingPenalty:F2}");
        Settings.belowFreezingPenalty = list.Slider(val: Settings.belowFreezingPenalty, min: 0f, max: 1f);
        // FIXME: my god, this absolutely sucks... :P
        // we basically do an initial draw with a rather small height, and see if it's enough
        // - if it is, we remember the exact height of the content and resize the window to fit
        // - otherwise CurHeight will be smaller than a known epsilon, so we start at the epsilon and double it with each draw until it's big enough
        // - once we have enough space, we clamp the height to the known content height
        // if the content is too small to fit everything, CurHeight will be 23.something, (don't ask me why)
        if (list.CurHeight < _minContentHeight)
        {
            // so on each draw, we double the known content height until it's big enough
            // yes, we could also just set it to an unreasonably high value, but that seems a bit wasteful
            // we should arrive at the correct height in O(log n) iterations anyway
            _knownContentHeight = Mathf.Max(2 * _knownContentHeight, _minContentHeight);
        }
        else
        {
            // when we have enough space, we remember the height
            _knownContentHeight = list.CurHeight;
        }
        list.End();
        bool requiresScrolling = _knownContentHeight > canvas.height;
        if (_requiresScrolling)
        {
            Widgets.EndScrollView();
        }
        _requiresScrolling = requiresScrolling;
        base.DoSettingsWindowContents(canvas);
    }

    public override string SettingsCategory() => "Blood Dries";

    private static string DaysToDisplayString(float days)
    {
        TimeSpan timeSpan = TimeSpan.FromDays(days);
        return (timeSpan.Days, timeSpan.Hours) switch
        {
            (>= 15, _) => $"1 quadrum",
            (> 1, > 1) => $"{timeSpan.Days} days, {timeSpan.Hours} hours",
            (> 1, 1) => $"{timeSpan.Days} days, 1 hour",
            (> 1, 0) => $"{timeSpan.Days} days",
            (1, > 1) => $"1 day, {timeSpan.Hours} hours",
            (1, 1) => "1 day, 1 hour",
            (1, 0) => "1 day",
            (0, > 1) => $"{timeSpan.Hours} hours",
            (0, 1) => "1 hour",
            (0, 0) => "in less than an hour",
            _ => $"{timeSpan.Hours} hours",
        };
    }
}