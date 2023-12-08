using GameNetcodeStuff;
using HarmonyLib;
using LethalRebinding.Utilities;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LethalRebinding.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerBPatch
{
    private static List<PlayerControllerB> _instances = new();
    private static MethodInfo? _onEnable;
    private static MethodInfo? _onDisable;

    public static void ApplyNewBindings(string bindings)
    {
        //GameNetworkManager.Instance?.localPlayerController?.playerActions?.LoadBindingOverridesFromJson(bindings, true);
        // TODO: Other null/exist checks (or check game state) to make sure this doesn't break anything?
        foreach (var instance in _instances)
        {
            if (instance == null) continue;
            instance.playerActions.LoadBindingOverridesFromJson(bindings, true);
        }
        Debug.Log("Applying new bindings!");
        Debug.Log(bindings);
    }

    [HarmonyPatch("ConnectClientToPlayerObject")]
    [HarmonyPostfix]
    private static void Postfix(PlayerControllerB __instance)
    {
        _instances.Add(__instance);
        if (_onEnable == null) _onEnable = AccessTools.Method(typeof(PlayerControllerB), "OnEnable");
        if (_onDisable == null) _onDisable = AccessTools.Method(typeof(PlayerControllerB), "OnDisable");

        // apply bindings to actions
        // hope this doesn't break anything lol
        ApplyNewBindings(IngamePlayerSettings.Instance.settings.keyBindings);
    }

    [HarmonyPatch("SetHoverTipAndCurrentInteractTrigger")]
    [HarmonyPostfix]
    private static void PostfixCursorTip(PlayerControllerB __instance)
    {
        if (__instance.cursorTip.text.Contains("[E]")) {
            //Should be "Movement/Interact/[/Keyboard/<KEYBINDING>]"
            //for example "Movement/Interact/[/Keyboard/f]"
            var interactBinding = __instance.playerActions.FindAction("Interact");
            __instance.cursorTip.text= __instance.cursorTip.text.Replace("[E]", "[" + DisplayUtilities.LocalizeKey(interactBinding) + "]");
        }
    }

}