using HarmonyLib;

namespace ViewmodelOffset.Patches;

[HarmonyPatch(typeof(PlayerCamera))]
internal static class PlayerCameraPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerCamera.UpdateCamera))]
    private static void HandleViewmodelFlip(PlayerCamera __instance)
    {
        var playerMain = __instance.playerMain;
        if (playerMain == null || playerMain.ForeignPlayer)
            return;

        var arms = playerMain.arms;
        if (arms == null)
            return;

        if (__instance.CurrentMode != PlayerCamera.Mode.FirstPerson)
        {
            if (arms.transform.localScale.x < 0f)
            {
                Flip(arms);
            }

            return;
        }

        var shouldFlip = ViewmodelOffset.Flip.Value;
        if (arms.transform.localScale.x > 0f && shouldFlip ||
            arms.transform.localScale.x < 0f && !shouldFlip)
        {
            Flip(arms);
        }
    }

    private static void Flip(PlayerArms arms)
    {
        var curScale = arms.transform.localScale;
        curScale.x *= -1f;
        arms.transform.localScale = curScale;
    }
}
