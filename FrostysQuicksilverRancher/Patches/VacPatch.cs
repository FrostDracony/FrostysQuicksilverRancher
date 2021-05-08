using Configs;
using HarmonyLib;
using FrostysQuicksilverRancher.Other;

namespace FrostysQuicksilverRancher.Patches
{
    [HarmonyPatch(typeof(VacDisplayChanger))]
    [HarmonyPatch("Awake")]
    public static class VacPatch
    {
        public static void Postfix(VacDisplayChanger __instance)
        {
            Console.Log("Awake with Vacpack enum: " + Values.VACPACK);
            bool flag = __instance.playerState.HasUpgrade(Ids.MOCHI_HACK);
            if (flag)
            {
                VACPACK_ENUMS flag1 = Values.VACPACK;
                switch (flag1)
                {
                    case VACPACK_ENUMS.NIMBLE_VALLEY:
                        __instance.SetDisplayMode(PlayerState.AmmoMode.NIMBLE_VALLEY);
                        break;

                    case VACPACK_ENUMS.DEFAULT:
                        __instance.SetDisplayMode(PlayerState.AmmoMode.DEFAULT);
                        break;

                    case VACPACK_ENUMS.AUTOMATIC:
                        ZoneDirector.Zone currentZone = SRSingleton<SceneContext>.Instance.PlayerZoneTracker.GetCurrentZone();
                        PlayerState.AmmoMode ammoMode = (currentZone == ZoneDirector.Zone.MOCHI_RANCH || currentZone == ZoneDirector.Zone.VALLEY)
                            ? PlayerState.AmmoMode.NIMBLE_VALLEY :
                            PlayerState.AmmoMode.DEFAULT;
                        __instance.SetDisplayMode(ammoMode);
                        break;
                }
            }
        }
    }
}
