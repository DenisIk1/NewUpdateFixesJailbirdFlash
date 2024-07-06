using HarmonyLib;
using InventorySystem.Items.Jailbird;
using static HarmonyLib.AccessTools;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.API.Features.Pools;

namespace JailBirdFlashBack
{
    [HarmonyPatch(typeof(JailbirdHitreg), nameof(JailbirdHitreg.ServerAttack))]
    public static class Patch
    {
        public static void EnableEffect(HitboxIdentity hitboxIdentity)
        {
            Player player = Player.Get(hitboxIdentity.TargetHub);
            player.EnableEffect(EffectType.Flashed, 1, 4, true);
            Log.Debug("Effect Activated");
        }
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Pool.Get(instructions);
            int index = newInstructions.FindLastIndex(code => code.opcode == OpCodes.Ldfld && (FieldInfo)code.operand == Field(typeof(ReferenceHub), nameof(ReferenceHub.playerEffectsController))) + 3;
            /// Nah, I will call specific method, because newbie to Transpillers
            newInstructions.InsertRange(
                index,
                new CodeInstruction[]
                {
                    new CodeInstruction(OpCodes.Ldloc_S, 12),
                    new CodeInstruction(OpCodes.Call, Method(typeof(Patch), nameof(Patch.EnableEffect), new [] {typeof(HitboxIdentity)})),
                });
            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];
            ListPool<CodeInstruction>.Pool.Return(newInstructions);
        }
    }
}
