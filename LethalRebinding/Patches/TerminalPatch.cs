using GameNetcodeStuff;
using HarmonyLib;
using System.Threading.Tasks;

namespace LethalRebinding.Patches;

[HarmonyPatch(typeof(Terminal))]
internal class TerminalPatch
{
    [HarmonyPatch(nameof(Terminal.QuitTerminal))]
    [HarmonyPostfix]
    private static void EndPostfix(Terminal __instance)
    {
        PlayerControllerB localPlayerController = GameNetworkManager.Instance.localPlayerController;
        localPlayerController.inTerminalMenu = true;
        DelayEnablingPausing(localPlayerController);
    }

    // dude WHAT
    // WHY IS THIS NECESSARY???????????????
    private static async void DelayEnablingPausing(PlayerControllerB localPlayerController)
    {
        await Task.Delay(100);
        localPlayerController.inTerminalMenu = false;
    }
}