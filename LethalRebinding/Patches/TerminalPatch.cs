using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LethalRebinding.Patches
{
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
}
