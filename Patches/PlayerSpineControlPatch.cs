using HarmonyLib;

namespace ViewmodelOffset.Patches;

[HarmonyPatch(typeof(PlayerSpineControl))]
internal static class PlayerSpineControlPatch
{
    [HarmonyPrefix]
    [HarmonyPatch("CorrectSpine")]
    private static void FixSpine(PlayerSpineControl __instance, ref float ___deviationY)
    {
        var playerMain = __instance.GetComponentInParent<PlayerMain>();
        if (playerMain != null && !playerMain.ForeignPlayer &&
            PlayerCamera.instance?.CurrentMode == PlayerCamera.Mode.FirstPerson && ViewmodelOffset.Flip.Value)
        {
            ___deviationY = -___deviationY;
        }
    }
}
