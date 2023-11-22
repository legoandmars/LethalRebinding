// Copyright 2023 zatrit (https://github.com/zatrit/mousebind/tree/main)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

using static UnityEngine.InputSystem.InputActionRebindingExtensions;

namespace MouseBind.Patches;

[HarmonyPatch(typeof(IngamePlayerSettings), "RebindKey")]
public class AllowMouseBindings
{
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        => codes.Where(code => {
            var isLdstrMouse = code.opcode == OpCodes.Ldstr && code.OperandIs("Mouse");
            var isExcludeCall = code.Calls(typeof(RebindingOperation).GetMethod("WithControlsExcluding"));

            return !isLdstrMouse && !isExcludeCall;
        });
}