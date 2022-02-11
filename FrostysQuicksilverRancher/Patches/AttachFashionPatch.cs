using UnityEngine;
using HarmonyLib;
using FrostysQuicksilverRancher.Other;

namespace FrostysQuicksilverRancher.Patches
{
	[HarmonyPatch(typeof(AttachFashions))]
	[HarmonyPatch("Awake")]
	public static class AttachFashionPatch
	{
		public static void Postfix(AttachFashions __instance)
		{
			if (__instance.gameObject.GetComponent<Identifiable>() != null)
			{
				SRML.Console.Console.Log("GameObject " + __instance.gameObject + " has an Id");
				if (__instance.gameObject.GetComponent<Identifiable>().id == Identifiable.Id.TARR_SLIME)
				{
					SRML.Console.Console.Log("AttachFashionPatch Awake");
					__instance.Attach(SRML.Utils.PrefabUtils.CopyPrefab(GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.ROYAL_FASHION)).GetComponent<Fashion>());
					Object.Destroy(__instance);
				}


			}

		}
	}
}
