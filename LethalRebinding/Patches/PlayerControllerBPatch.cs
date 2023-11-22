using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LethalRebinding.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        private static List<PlayerControllerB>? _instances = new();
        private static MethodInfo? _onEnable;
        private static MethodInfo? _onDisable;

        public static void ApplyNewBindings(string bindings)
        {
            // TODO: Other null/exist checks (or check game state) to make sure this doesn't break anything?
            foreach (var instance in _instances)
            {
                if (instance == null) continue;
                instance.playerActions.LoadBindingOverridesFromJson(bindings, true);
            }
            Debug.Log("Applying new bindings!");
            Debug.Log(bindings);
        }

        [HarmonyPatch("Awake")]
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
                string[] interactBinding = __instance.playerActions.FindAction("Interact").ToString().Split('/');
                string newBinding = interactBinding[interactBinding.Length-1].Substring(0,1).ToUpper();
                __instance.cursorTip.text= __instance.cursorTip.text.Replace("[E]", "[" + newBinding + "]");
            }
        }

    }
}
