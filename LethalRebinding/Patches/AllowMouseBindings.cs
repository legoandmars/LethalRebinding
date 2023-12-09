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
using System.Reflection.Emit;
using HarmonyLib;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

namespace LethalRebinding.Patches;

[HarmonyPatch(typeof(IngamePlayerSettings), "RebindKey")]
public class AllowMouseBindings
{
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        => codes.Manipulator(
            instruction => {
                var isLdstrMouse = instruction.opcode == OpCodes.Ldstr && instruction.OperandIs("Mouse");
                var isExcludeCall = instruction.Calls(typeof(RebindingOperation).GetMethod("WithControlsExcluding"));

                return isLdstrMouse || isExcludeCall;
            },
            instruction =>
            {
                instruction.opcode = OpCodes.Nop;
                instruction.operand = null;
            }
        );
}