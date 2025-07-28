global using System;
global using Logger = BloodDries.Logging.Logger;
global using Std = System;
global using System.Diagnostics.CodeAnalysis;
global using static BloodDries.Globals;
global using static BloodDries.Roslyn.BuildIntrinsics.Suppressions;

namespace BloodDries;

public static class Globals
{
    /// <summary>
    /// A typed null value to be used for parameter-less switch expressions.
    /// </summary>
    public const object? __ = null;
}