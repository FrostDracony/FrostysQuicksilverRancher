/*using HarmonyLib;

namespace FrostysQuicksilverRancher.Patches
{
	[HarmonyPatch(typeof(PlayerZoneTracker))]
	[HarmonyPatch("OnEntered")]
	internal static class ZonePatch
	{
		public static void Postfix(ZoneDirector.Zone zone)
		{
			if (zone == ZoneDirector.Zone.MOCHI_RANCH)
			{
				SRSingleton<SceneContext>.Instance.PlayerState.CheckAllUpgradeLockers();
			}
		}
	}
}
*/