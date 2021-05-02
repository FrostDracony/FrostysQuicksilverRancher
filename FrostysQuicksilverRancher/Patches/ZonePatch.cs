/*using HarmonyLib;

namespace FrostysQuicksilverRancher.Patches
{
	[HarmonyPatch(typeof(PlayerZoneTracker))]
	[HarmonyPatch("OnEntered")]
	internal static class ZonePatch
	{
		public static void Postfix(ZoneDirector.Zone zone)
		{
			bool flag = zone == ZoneDirector.Zone.MOCHI_RANCH;
			if (flag)
			{
				SRSingleton<SceneContext>.Instance.PlayerState.CheckAllUpgradeLockers();
			}
		}
	}
}
*/