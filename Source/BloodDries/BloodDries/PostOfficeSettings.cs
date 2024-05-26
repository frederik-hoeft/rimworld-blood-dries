using System.Text.RegularExpressions;
using Verse;

namespace PostOffice;

public class PostOfficeSettings : ModSettings
{
    // general
    internal bool isActive = true;
    internal bool enableMessageSupport = true;
    internal bool enableLogging = false;
    internal bool enableVerboseLogging = false;

    // mod support
    internal bool cai5000_delayCombatMusic = true;

    // rules
    internal bool dropThreatBig = false;
    internal bool dropThreatSmall = false;
    internal bool dropNegativeEvent = false;
    internal bool dropAcceptCreepJoiner = false;
    internal bool dropEntityDiscovered = false;
    internal bool dropAcceptJoiner = false;
    internal bool dropAcceptVisitors = false;
    internal bool dropBabyToChild = false;
    internal bool dropBabyBirth = false;
    internal bool dropBossgroup = false;
    internal bool dropBundleLetter = false;
    internal bool dropChildBirthday = false;
    internal bool dropChildToAdult = false;
    internal bool dropDeath = false;
    internal bool dropChoosePawn = false;
    internal bool dropNeutralEvent = false;
    internal bool dropPositiveEvent = false;
    internal bool dropRelicHuntInstallationFound = false;
    internal bool dropRitualOutcomeNegative = false;
    internal bool dropRitualOutcomePositive = false;
    private string dropRegex = string.Empty;
    private string dropMessageRegex = string.Empty;

    internal Regex? DropRegexCompiled { get; private set; }

    internal Regex? DropMessageRegexCompiled { get; private set; }

    internal bool DropRegexIsValid { get; private set; }

    internal bool DropMessageRegexIsValid { get; private set; }

    internal string DropRegexLatestError { get; private set; } = string.Empty;

    internal string DropMessageRegexLatestError { get; private set; } = string.Empty;

    internal string DropRegex
    {
        get => dropRegex;
        set
        {
            if (value != dropRegex)
            {
                dropRegex = value;
                RecompileDropRegex();
            }
        }
    }

    internal string DropMessageRegex
    {
        get => dropMessageRegex;
        set
        {
            if (value != dropMessageRegex)
            {
                dropMessageRegex = value;
                RecompileDropMessageRegex();
            }
        }
    }

    private bool RecompileDropRegex()
    {
        try
        {
            DropRegexCompiled = string.IsNullOrEmpty(dropRegex) ? null : new Regex(dropRegex, RegexOptions.Compiled);
            DropRegexIsValid = true;
            DropRegexLatestError = string.Empty;
            return true;
        }
        catch (Exception e)
        {
            DropRegexCompiled = null;
            DropRegexIsValid = false;
            DropRegexLatestError = e.Message;
            return false;
        }
    }

    private bool RecompileDropMessageRegex()
    {
        try
        {
            DropMessageRegexCompiled = string.IsNullOrEmpty(dropMessageRegex) ? null : new Regex(dropMessageRegex, RegexOptions.Compiled);
            DropMessageRegexIsValid = true;
            DropMessageRegexLatestError = string.Empty;
            return true;
        }
        catch (Exception e)
        {
            DropMessageRegexCompiled = null;
            DropMessageRegexIsValid = false;
            DropMessageRegexLatestError = e.Message;
            return false;
        }
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref isActive, nameof(isActive), true);
        Scribe_Values.Look(ref enableMessageSupport, nameof(enableMessageSupport), true);
        Scribe_Values.Look(ref enableLogging, nameof(enableLogging), false);
        Scribe_Values.Look(ref enableVerboseLogging, nameof(enableVerboseLogging), false);

        Scribe_Values.Look(ref cai5000_delayCombatMusic, nameof(cai5000_delayCombatMusic), false);

        Scribe_Values.Look(ref dropThreatBig, nameof(dropThreatBig), defaultValue: false);
        Scribe_Values.Look(ref dropThreatSmall, nameof(dropThreatSmall), defaultValue: false);
        Scribe_Values.Look(ref dropNegativeEvent, nameof(dropNegativeEvent), defaultValue: false);
        Scribe_Values.Look(ref dropAcceptJoiner, nameof(dropAcceptJoiner), defaultValue: false);
        Scribe_Values.Look(ref dropAcceptVisitors, nameof(dropAcceptVisitors), defaultValue: false);
        Scribe_Values.Look(ref dropBabyToChild, nameof(dropBabyToChild), defaultValue: false);
        Scribe_Values.Look(ref dropBabyBirth, nameof(dropBabyBirth), defaultValue: false);
        Scribe_Values.Look(ref dropBossgroup, nameof(dropBossgroup), defaultValue: false);
        Scribe_Values.Look(ref dropBundleLetter, nameof(dropBundleLetter), defaultValue: false);
        Scribe_Values.Look(ref dropChildBirthday, nameof(dropChildBirthday), defaultValue: false);
        Scribe_Values.Look(ref dropChildToAdult, nameof(dropChildToAdult), defaultValue: false);
        Scribe_Values.Look(ref dropDeath, nameof(dropDeath), defaultValue: false);
        Scribe_Values.Look(ref dropChoosePawn, nameof(dropChoosePawn), defaultValue: false);
        Scribe_Values.Look(ref dropNeutralEvent, nameof(dropNeutralEvent), defaultValue: false);
        Scribe_Values.Look(ref dropPositiveEvent, nameof(dropPositiveEvent), defaultValue: false);
        Scribe_Values.Look(ref dropRelicHuntInstallationFound, nameof(dropRelicHuntInstallationFound), defaultValue: false);
        Scribe_Values.Look(ref dropRitualOutcomeNegative, nameof(dropRitualOutcomeNegative), defaultValue: false);
        Scribe_Values.Look(ref dropRitualOutcomePositive, nameof(dropRitualOutcomePositive), defaultValue: false);
        Scribe_Values.Look(ref dropAcceptCreepJoiner, nameof(dropAcceptCreepJoiner), defaultValue: false);
        Scribe_Values.Look(ref dropEntityDiscovered, nameof(dropEntityDiscovered), defaultValue: false);

        Scribe_Values.Look(ref dropRegex, nameof(dropRegex), string.Empty);
        Scribe_Values.Look(ref dropMessageRegex, nameof(dropMessageRegex), string.Empty);
        RecompileDropRegex();
        RecompileDropMessageRegex();
    }
}
