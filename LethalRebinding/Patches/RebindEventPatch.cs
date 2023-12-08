using HarmonyLib;

namespace LethalRebinding.Patches;

[HarmonyPatch(typeof(IngamePlayerSettings), nameof(IngamePlayerSettings.ResetSettingsToDefault))]
internal class RebindEventPatch
{
    private static void Postfix(IngamePlayerSettings __instance)
    {
        PlayerControllerBPatch.ApplyNewBindings(__instance.settings.keyBindings);
        HUDManagerPatch.ApplyNewBindings(__instance.settings.keyBindings);
    }
}

[HarmonyPatch(typeof(IngamePlayerSettings), nameof(IngamePlayerSettings.SaveChangedSettings))]
internal class RebindEventPatch2
{
    private static void Postfix(IngamePlayerSettings __instance)
    {
        PlayerControllerBPatch.ApplyNewBindings(__instance.settings.keyBindings);
        HUDManagerPatch.ApplyNewBindings(__instance.settings.keyBindings);
    }
}