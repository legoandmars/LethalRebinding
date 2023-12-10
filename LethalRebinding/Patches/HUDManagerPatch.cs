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
                    InputAction action = null;
                    switch(line.text)
                    {
                        case string s when s.StartsWith("Drop"):
                            action = IngamePlayerSettings.Instance.playerInput.actions.FindAction("Discard");
                            break;
                        case string s when s.Contains("[Q]"):
                            action = IngamePlayerSettings.Instance.playerInput.actions.FindAction("ItemSecondaryUse");
                            break;
                        case string s when s.Contains("[Q/E]"):
                            var action1 =  IngamePlayerSettings.Instance.playerInput.actions.FindAction("ItemSecondaryUse");
                            var action2 = IngamePlayerSettings.Instance.playerInput.actions.FindAction("ItemTertiaryUse");
                            if (action1 == null || action2 == null) continue;
                            line.text = $"{linePrefix} [{DisplayUtilities.LocalizeKey(action1)}/{DisplayUtilities.LocalizeKey(action2)}]";
                            continue; //special case, and can move onto the next line
                            break;
                        case string s when s.Contains("[Z]"):
                            action = IngamePlayerSettings.Instance.playerInput.actions.FindAction("InspectItem");
                            break;
                        default:
                            break;

                    }
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