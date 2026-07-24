using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;
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
    public static Vector3 ViewOffset = Vector3.zero;
    public static bool ShouldFlip;

    private void Awake()
    {
        Logger = base.Logger;
        try
        {
            ConfigEntry<float> offsetX = Config.Bind("Offset", "X (Right/Left)", -0.05f,
                new ConfigDescription("X viewmodel offset. Positive = right, negative = left.",
                    new AcceptableValueRange<float>(-0.5f, 0.5f)));
            ConfigEntry<float> offsetY = Config.Bind("Offset", "Y (Up/Down)", -0.1f,
                new ConfigDescription("Y viewmodel offset. Positive = up, negative = down.",
                    new AcceptableValueRange<float>(-0.5f, 0.5f)));
            ConfigEntry<float> offsetZ = Config.Bind("Offset", "Z (Forward/Backward)", -0.05f,
                new ConfigDescription("Z viewmodel offset. Positive = forward, negative = backward.",
                    new AcceptableValueRange<float>(-0.5f, 0.5f)));
            ConfigEntry<bool> flip = Config.Bind("Offset", "Flip", false,
                new ConfigDescription("Whether the viewmodel should be flipped (mirrored) or not."));

            ViewOffset = new Vector3(offsetX.Value, offsetY.Value, offsetZ.Value);
            ShouldFlip = flip.Value;

            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("Successfully loaded!");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to load: {ex}");
        }
    }
}
