using UnityEngine;

/// <summary>
/// The three climbable routes and the persisted run record.
/// Storage ids are deliberate string literals rather than symbol names: they outlive class
/// renaming by the obfuscator, which is exactly what a save key has to do.
/// </summary>
public static class ShaftRoutes
{
    public const int Count = 3;
    public const int Sections = 18;

    private const string BestKeyPrefix = "kr_best_";
    private const string RunsKey = "kr_runs";
    private const string RouteKey = "kr_route";

    /// <summary>Barrier kinds each route is allowed to spawn (see BarrierPlan).</summary>
    public static int[] BarrierKinds(int routeIndex)
    {
        switch (Mathf.Clamp(routeIndex, 0, Count - 1))
        {
            case 0: return new[] { BarrierPlan.KindSweep };
            case 1: return new[] { BarrierPlan.KindSweep, BarrierPlan.KindRotor };
            default: return new[] { BarrierPlan.KindSweep, BarrierPlan.KindRotor, BarrierPlan.KindGate };
        }
    }

    public static float SpeedMultiplier(int routeIndex)
    {
        switch (Mathf.Clamp(routeIndex, 0, Count - 1))
        {
            case 0: return 1.00f;
            case 1: return 1.15f;
            default: return 1.30f;
        }
    }

    public static string Title(int routeIndex)
    {
        switch (Mathf.Clamp(routeIndex, 0, Count - 1))
        {
            case 0: return "LOW SPIRE";
            case 1: return "MID VAULT";
            default: return "HIGH CROWN";
        }
    }

    public static string Summary(int routeIndex)
    {
        switch (Mathf.Clamp(routeIndex, 0, Count - 1))
        {
            case 0: return "18 SECTIONS · SWEEP";
            case 1: return "18 SECTIONS · SWEEP + ROTOR";
            default: return "18 SECTIONS · SWEEP + ROTOR + GATE";
        }
    }

    public static string Chip(int routeIndex)
    {
        int shown = Mathf.Clamp(routeIndex, 0, Count - 1) + 1;
        return "ROUTE " + shown.ToString() + " · " + Title(routeIndex);
    }

    public static int SelectedRoute()
    {
        return Mathf.Clamp(PlayerPrefs.GetInt(RouteKey, 0), 0, Count - 1);
    }

    public static void SelectRoute(int routeIndex)
    {
        PlayerPrefs.SetInt(RouteKey, Mathf.Clamp(routeIndex, 0, Count - 1));
        PlayerPrefs.Save();
    }

    public static int Best(int routeIndex)
    {
        return Mathf.Clamp(PlayerPrefs.GetInt(BestKeyPrefix + Mathf.Clamp(routeIndex, 0, Count - 1).ToString(), 0), 0, Sections);
    }

    public static void ReportBest(int routeIndex, int section)
    {
        int clamped = Mathf.Clamp(routeIndex, 0, Count - 1);
        if (section > Best(clamped))
        {
            PlayerPrefs.SetInt(BestKeyPrefix + clamped.ToString(), Mathf.Clamp(section, 0, Sections));
            PlayerPrefs.Save();
        }
    }

    public static int Runs()
    {
        return Mathf.Max(0, PlayerPrefs.GetInt(RunsKey, 0));
    }

    /// <summary>Bumped once per started run; also the attempt counter feeding the seed.</summary>
    public static int RegisterRun()
    {
        int next = Runs() + 1;
        PlayerPrefs.SetInt(RunsKey, next);
        PlayerPrefs.Save();
        return next;
    }
}
