using GameNetcodeStuff;
using HarmonyLib;
using LethalRebinding.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LethalRebinding.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        private static List<HUDManager>? _instances = new();
        private static Dictionary<string, string> _actionNameByHudText = new Dictionary<string, string>()
        {
            { "Sprint:", "Sprint" },
            { "Scan :", "PingScan" },
        };

        public static void ApplyNewBindings(string bindings)
        {
            foreach (var instance in _instances)
            {
                if (instance == null) continue;
                instance.playerActions.LoadBindingOverridesFromJson(bindings, true);

                foreach (var line in instance.controlTipLines)
                {
                    if (line == null) continue;
                    var linePrefix = line.text.Split(':')[0] + ":";
                    if (!_actionNameByHudText.TryGetValue(linePrefix, out string actionName) && !linePrefix.StartsWith("Drop")) continue;
                    if (linePrefix.StartsWith("Drop"))
                    {
                        actionName = "Discard";
                    }

                    var action = instance.playerActions.FindAction(actionName);
                    if (action == null) continue;

                    line.text = $"{linePrefix} [{DisplayUtilities.LocalizeKey(action)}]";
                }
            }
        }

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void Postfix(HUDManager __instance)
        {
            _instances.Add(__instance);

            ApplyNewBindings(IngamePlayerSettings.Instance.settings.keyBindings);
        }

        [HarmonyPatch("ChangeControlTip")]
        [HarmonyPostfix]
        private static void PostfixControlTip()
        {
            ApplyNewBindings(IngamePlayerSettings.Instance.settings.keyBindings);
        }

        [HarmonyPatch("ChangeControlTipMultiple")]
        [HarmonyPostfix]
        private static void PostfixControlTipMultiple()
        {
            ApplyNewBindings(IngamePlayerSettings.Instance.settings.keyBindings);
        }
    }
}