using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalRebinding.Patches
{
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
}