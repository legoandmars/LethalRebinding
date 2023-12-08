using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace LethalRebinding.Utilities
{
    internal static class DisplayUtilities
    {
        // TODO: it might be fine to just replace "LEFT" with L, same with right - not sure if this would break anything so i've just added some common key name mappings
        private static Dictionary<string, string> _keyNameReplacements = new()
        {
            { "LEFTBUTTON", "LMB" },
            { "RIGHTBUTTON", "RMB" },
            { "LEFTCTRL", "LCTRL" },
            { "RIGHTCTRL", "LCTRL" },
            { "LEFTSHIFT", "LSHIFT" },
            { "RIGHTSHIFT", "RSHIFT" },
            { "LEFTALT", "ALT" },
            { "RIGHTALT", "RALT" },
        };

        // gets a prompt display key for an inputaction's binding
        // replaces some longer strings with more reasonable ones, such as Left Button -> LMB
        internal static string LocalizeKey(InputAction action)
        {
            string[] interactBinding = action.ToString().Split('/');
            // get the name of the binding, make it uppercase, and remove the brackets
            string newBinding = interactBinding[interactBinding.Length - 1].ToUpper().Replace("[", "").Replace("]", "");
            if (_keyNameReplacements.TryGetValue(newBinding, out var replacement)) newBinding = replacement;

            return newBinding.ToUpper();
        }
    }
}
