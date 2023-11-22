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
    [HarmonyPatch(typeof(PlayerControllerB), "Awake")]
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

        private static void Postfix(PlayerControllerB __instance)
        {
            _instances.Add(__instance);
            if (_onEnable == null) _onEnable = AccessTools.Method(typeof(PlayerControllerB), "OnEnable");
            if (_onDisable == null) _onDisable = AccessTools.Method(typeof(PlayerControllerB), "OnDisable");

            // apply bindings to actions
            // hope this doesn't break anything lol
            ApplyNewBindings(IngamePlayerSettings.Instance.settings.keyBindings);
        }
    }
}