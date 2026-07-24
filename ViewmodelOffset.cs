using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using System;

namespace ViewmodelOffset;

[BepInPlugin(PluginGuid, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class ViewmodelOffset : BaseUnityPlugin
{
    internal new static ManualLogSource Logger;
    internal const string PluginGuid = "com.theblackvoid.viewmodeloffset";
    private readonly Harmony _harmony = new(PluginGuid);
    public static ConfigEntry<float> XOffset;
    public static ConfigEntry<float> YOffset;
    public static ConfigEntry<float> ZOffset;
    public static ConfigEntry<bool> Flip;

    private void Awake()
    {
        Logger = base.Logger;
        try
        {
            Configure();
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("Successfully loaded!");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to load: {ex}");
        }
    }

    private void Configure()
    {
        XOffset = Config.Bind("Offset", "X (Right/Left)", -0.05f,
            new ConfigDescription("X viewmodel offset. Positive = right, negative = left.",
                new AcceptableValueRange<float>(-0.5f, 0.5f)));
        YOffset = Config.Bind("Offset", "Y (Up/Down)", -0.1f,
            new ConfigDescription("Y viewmodel offset. Positive = up, negative = down.",
                new AcceptableValueRange<float>(-0.5f, 0.5f)));
        ZOffset = Config.Bind("Offset", "Z (Forward/Backward)", -0.05f,
            new ConfigDescription("Z viewmodel offset. Positive = forward, negative = backward.",
                new AcceptableValueRange<float>(-0.5f, 0.5f)));
        Flip = Config.Bind("Offset", "Flip", false,
            new ConfigDescription("Whether the viewmodel should be flipped (mirrored) or not."));
    }
}
