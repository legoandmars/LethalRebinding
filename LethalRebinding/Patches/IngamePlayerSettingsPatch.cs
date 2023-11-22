using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using TMPro;
using UnityEngine.UI;

namespace LethalRebinding.Patches
{
    [HarmonyPatch(typeof(IngamePlayerSettings), nameof(IngamePlayerSettings.UpdateGameToMatchSettings))]
    internal class IngamePlayerSettingsPatch
    {
        private static List<string> _whitelistedValueActions = new List<string>()
        {
            "Sprint",
            "Interact"
        };

        // Inject into existing settings
        private static void Prefix(IngamePlayerSettings __instance)
        {
            // not ideal but its what the game does
            // could be a transpiler to avoid two calls, don't wanna write rn
            SettingsOption[] settings = Object.FindObjectsOfType<SettingsOption>(true);
            var validSettings = settings.Where(x => x != null && x.optionType == SettingsOptionType.ChangeBinding).ToList();
            var scrollSetting = settings.Where(x => x != null && x.optionType == SettingsOptionType.MasterVolume).ToList().FirstOrDefault();
            // we only want to modify settings menus with a single keybind, which should exclude:
            // - panels without push to talk
            // - panels we've already modified
            // - future updated panels with multiple bindings (the mod may be obselete at this point)
            if (validSettings.Count != 1) return;
            // used for making scrollbar look prettier
            if (scrollSetting == null) return;
            var scrollSettingBackground = scrollSetting.transform.GetChild(0).GetComponent<Image>();

            var setting = validSettings.First();
            var input = __instance.playerInput;
            var actions = input.actions.ToList();

            // create scroll rect
            var container = setting.transform.parent.parent.gameObject;
            var scrollRect = SettingsUtilities.CreateScrollRect(container, scrollSettingBackground);

            // TODO: Selective binding denying
            // You should NOT be able to bind pause to left mouse. things will be terrible
            for (int i = 0; i < actions.Count; i++)
            {
                var action = actions[i];
                if (action.type != InputActionType.Button && !_whitelistedValueActions.Contains(action.name)) continue;
                if (action.name == "SpeedCheat") continue; // probably not supposed to be public
                SettingsUtilities.GetSettingForInputAction(action, setting, scrollRect.transform, i);
            }

            Debug.Log(__instance.playerInput.actions.SaveBindingOverridesAsJson());
        }
    }
}
