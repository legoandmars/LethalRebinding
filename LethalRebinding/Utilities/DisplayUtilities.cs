using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LethalRebinding.Utilities
{
    internal static class DisplayUtilities
    {
        // TODO: it might be fine to just replace "LEFT" with L, same with right - not sure if this would break anything so i've just added some common key name mappings
        private static Dictionary<string, string> _keyNameReplacements = new Dictionary<string, string>()
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
            if (_keyNameReplacements.ContainsKey(newBinding)) newBinding = _keyNameReplacements[newBinding];

            return newBinding.ToUpper();
        }
    }
}
