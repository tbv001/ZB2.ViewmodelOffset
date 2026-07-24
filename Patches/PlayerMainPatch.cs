using HarmonyLib;
using UnityEngine;

namespace ViewmodelOffset.Patches;

[HarmonyPatch(typeof(PlayerMain))]
internal static class PlayerMainPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("UpdateSpineControlSettings")]
    private static void ApplyViewmodelOffset(PlayerMain __instance)
    {
        if (__instance == null || __instance.ForeignPlayer)
            return;

        if (__instance.cam == null || __instance.cam.CurrentMode != PlayerCamera.Mode.FirstPerson)
            return;

        var spineControl = __instance.SpawnedSkin?.spineControl;
        if (spineControl == null || spineControl.FirstPersonTweaks == null)
            return;

        var viewOffset = new Vector3(ViewmodelOffset.XOffset.Value, ViewmodelOffset.YOffset.Value,
            ViewmodelOffset.ZOffset.Value);
        if (viewOffset == Vector3.zero)
            return;

        var adsFactor = Mathf.Clamp(2f - __instance.arms.fightModeCoef, 0, 1);
        if (adsFactor <= 0f)
            return;

        var targetField = Traverse.Create(spineControl.FirstPersonTweaks).Field("target");
        var target = targetField.GetValue<FirstPersonTweak.Settings>();
        target.armsOffset += viewOffset * adsFactor;
        targetField.SetValue(target);
    }
}
