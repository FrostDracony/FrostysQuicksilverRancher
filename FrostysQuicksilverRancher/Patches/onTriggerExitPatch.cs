using Configs;
using HarmonyLib;
using UnityEngine;
using FrostysQuicksilverRancher.Other;

namespace FrostysQuicksilverRancher.Patches
{
	[HarmonyPatch(typeof(AmmoModeTrigger))]
	[HarmonyPatch("OnTriggerExit")]
	public static class OnTriggerExitPatch
	{
		public static bool Prefix(AmmoModeTrigger __instance)
		{
			bool flag = __instance.playerState.HasUpgrade(Ids.MOCHI_HACK);
			if (flag)
			{
				if (Values.VACPACK == VACPACK_ENUMS.AUTOMATIC)
					Object.FindObjectOfType<VacDisplayChanger>().SetDisplayMode(PlayerState.AmmoMode.NIMBLE_VALLEY);
				return false;
			}
			else { return true; }
		}
	}
}
