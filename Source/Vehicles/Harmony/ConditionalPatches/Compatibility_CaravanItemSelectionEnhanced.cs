using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using SmashTools;
using SmashTools.Patching;
using Verse;

namespace Vehicles.Compatibility;

internal class Compatibility_CaravanItemSelectionEnhanced : ConditionalVehiclePatch
{
	private const string HarmonyId = "kopp.rimworld.caravanitemselectionenhanced";

	public override string PackageId => ModPackageIds.CaravanItemSelectionEnhanced;

	public override PatchSequence PatchAt => PatchSequence.Disabled;

	public override void PatchAll(ModMetaData mod)
	{
		Type formCaravanPatchType =
			GenTypes.GetTypeInAnyAssembly("CaravanItemSelectionEnhanced.Dialog_FormCaravan_Postfix");

		HarmonyPatcher.Patch(AccessTools.Method(formCaravanPatchType, "Transpiler"),
			prefix: new HarmonyMethod(AccessTools.Method(typeof(Compatibility_CaravanItemSelectionEnhanced),
				nameof(RemoveTranspiler))));
		HarmonyPatcher.Patch(AccessTools.Method(formCaravanPatchType, "Transpiler2"),
			prefix: new HarmonyMethod(AccessTools.Method(typeof(Compatibility_CaravanItemSelectionEnhanced),
				nameof(RemoveTranspiler))));
		HarmonyPatcher.Patch(AccessTools.Method(typeof(Dialog_FormCaravan), nameof(Dialog_FormCaravan.DoWindowContents)),
			transpiler: new HarmonyMethod(AccessTools.Method(typeof(Compatibility_CaravanItemSelectionEnhanced),
				nameof(SkipDrawingTabs))));

		Type splitCaravanPatchType =
			GenTypes.GetTypeInAnyAssembly("CaravanItemSelectionEnhanced.Dialog_SplitCaravan_Postfix");
		HarmonyPatcher.Patch(AccessTools.Method(splitCaravanPatchType, "Transpiler"),
			prefix: new HarmonyMethod(AccessTools.Method(typeof(Compatibility_CaravanItemSelectionEnhanced),
				nameof(RemoveTranspiler))));
		HarmonyPatcher.Patch(AccessTools.Method(splitCaravanPatchType, "Transpiler2"),
			prefix: new HarmonyMethod(AccessTools.Method(typeof(Compatibility_CaravanItemSelectionEnhanced),
				nameof(RemoveTranspiler))));
		//HarmonyPatcher.Patch(AccessTools.Method(typeof(Dialog_SplitCaravan), nameof(Dialog_SplitCaravan.DoWindowContents)),
		//	prefix: new HarmonyMethod(AccessTools.Method(typeof(Compatibility_CaravanItemSelectionEnhanced),
		//		nameof(SkipDrawingTabs))));
	}

	private static IEnumerable<CodeInstruction> SkipDrawingTabs(IEnumerable<CodeInstruction> instructions)
	{
		List<CodeInstruction> instructionList = instructions.ToList();

		MethodInfo drawTabsMethod = typeof(TabDrawer).GetMethods(BindingFlags.Static | BindingFlags.Public)
		 .Where(method => method.Name == nameof(TabDrawer.DrawTabs))
		 .FirstOrDefault(method => method.GetParameters().Length == 3)?
		 .MakeGenericMethod(typeof(TabRecord));

		for (int i = 0; i < instructionList.Count; i++)
		{
			CodeInstruction instruction = instructionList[i];

			if (!instructionList.OutOfBounds(i + 2) && instructionList[i + 2].Calls(drawTabsMethod))
			{
				// Skips:
				// ldsfld List`1<TabRecord> tabsList
				// ldc.r4 200
				// call TabDrawer::DrawTabs
				i += 3;
				instruction = instructionList[i];
			}
			yield return instruction;
		}
	}

	private static bool RemoveTranspiler(out IEnumerable<CodeInstruction> __result,
		IEnumerable<CodeInstruction> instructions)
	{
		__result = instructions;
		return false;
	}
}