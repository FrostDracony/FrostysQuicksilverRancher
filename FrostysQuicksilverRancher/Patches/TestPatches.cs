using UnityEngine;

using Configs;
using HarmonyLib;

using MonomiPark.SlimeRancher.Regions;

using FrostysQuicksilverRancher.Other;

namespace FrostysQuicksilverRancher.Patches
{
    [HarmonyPatch(typeof(QuicksilverPlortCollector))]
    [HarmonyPatch("Update")]
    public static class QuicksilverPlortCollectorPatch
    {
        public static bool Prefix(QuicksilverPlortCollector __instance)
        {
            if (__instance.timeDirector.HasReached(__instance.timer))
            {
                bool isInMochiZone = __instance.GetComponent<RegionMember>().IsInZone(ZoneDirector.Zone.MOCHI_RANCH);
                Console.Log("isInMochiZone: " + isInMochiZone);
                bool isInValleyZone = __instance.GetComponent<RegionMember>().IsInZone(ZoneDirector.Zone.VALLEY);
                Console.Log("isInValleyZone: " + isInValleyZone);
                return isInMochiZone || isInValleyZone;
            }
            else { return false; }
        }
    }

    [HarmonyPatch(typeof(QuicksilverEnergyGenerator))]
    [HarmonyPatch("SetState")]
    public static class TestPatches
    {
        public static bool Prefix(QuicksilverEnergyGenerator __instance, QuicksilverEnergyGenerator.State state, bool enableSFX)
        {
            Console.Log("Parameters are: " + state + " and " + enableSFX);
            Destroyer.Destroy(__instance.countdownUI, "QuicksilverEnergyGenerator.SetState");
            Console.Log("CountdownUI destroyed");
            __instance.model.state = state;
            Console.Log("model.state setted");

            if (__instance.model.state == QuicksilverEnergyGenerator.State.COUNTDOWN)
            {
                Console.Log("if state is countdown");

                __instance.model.timer = new double?(__instance.timeDirector.HoursFromNow(__instance.countdownMinutes * 0.016666668f));
                Console.Log("Countdown: timer setted");

                if (enableSFX)
                {
                    Console.Log("Countdown: if enable SFX");

                    SECTR_AudioSystem.Play(__instance.onCountdownCue, __instance.transform.position, false);
                    Console.Log("Audio played");
                }

                if (SRSingleton<SceneContext>.Instance.PlayerState.HasUpgrade(Ids.MOCHI_HACK))
                {
                    VACPACK_ENUMS flag1 = Values.VACPACK;
                    switch (flag1)
                    {
                        case VACPACK_ENUMS.NIMBLE_VALLEY:
                            SRSingleton<SceneContext>.Instance.Player
                                .GetComponentInChildren<WeaponVacuum>()
                                .GetComponentInChildren<VacDisplayTimer>()
                                .SetQuicksilverEnergyGenerator(__instance);
                            break;

                        case VACPACK_ENUMS.DEFAULT:
                            break;

                        case VACPACK_ENUMS.AUTOMATIC:
                            ZoneDirector.Zone currentZone = SRSingleton<SceneContext>.Instance.PlayerZoneTracker.GetCurrentZone();

                            PlayerState.AmmoMode ammoMode = (currentZone == ZoneDirector.Zone.MOCHI_RANCH || currentZone == ZoneDirector.Zone.VALLEY) ?
                                PlayerState.AmmoMode.NIMBLE_VALLEY :
                                PlayerState.AmmoMode.DEFAULT;

                            if (ammoMode == PlayerState.AmmoMode.NIMBLE_VALLEY)
                            {
                                SRSingleton<SceneContext>.Instance.Player
                                    .GetComponentInChildren<WeaponVacuum>()
                                    .GetComponentInChildren<VacDisplayTimer>()
                                    .SetQuicksilverEnergyGenerator(__instance);
                            }

                            break;
                    }
                }
                else
                {
                    SRSingleton<SceneContext>.Instance.Player
                        .GetComponentInChildren<WeaponVacuum>()
                        .GetComponentInChildren<VacDisplayTimer>()
                        .SetQuicksilverEnergyGenerator(__instance);
                }

                Console.Log("Countdown: VacDisplayTimer generator setted");

                __instance.countdownUI = UnityEngine.Object.Instantiate<GameObject>(__instance.countdownUIPrefab);
                Console.Log("Countdown: countdownUI created");

                __instance.countdownUI.GetComponent<HUDCountdownUI>().SetCountdownTime(__instance.countdownMinutes);
                Console.Log("Countdown: Timer time setted");
            }
            else if (__instance.model.state == QuicksilverEnergyGenerator.State.ACTIVE)
            {
                Console.Log("if state is active");

                __instance.model.timer = new double?(__instance.timeDirector.HoursFromNow(__instance.activeHours));
                Console.Log("Active: Timer setted");
            }
            else if (__instance.model.state == QuicksilverEnergyGenerator.State.COOLDOWN)
            {
                Console.Log("if state is cooldown");

                __instance.model.timer = new double?(__instance.timeDirector.HoursFromNow(__instance.cooldownHours));
                Console.Log("Cooldown: timer setted");

                if (enableSFX)
                {
                    Console.Log("Cooldown: if enableSFX");

                    SECTR_AudioSystem.Play(__instance.onCooldownCue, __instance.transform.position, false);
                    Console.Log("Cooldown: Play first audio");

                    SECTR_AudioSystem.Play(__instance.onCooldownCue2D, Vector3.zero, false);
                    Console.Log("Cooldown: Play second audio");
                }
            }
            else
            {
                Console.Log("if everything is null");

                __instance.model.timer = null;
                Console.Log("Null: Timer setted");

                if (enableSFX)
                {
                    Console.Log("Null: if enableSFX");

                    SECTR_AudioSystem.Play(__instance.onInactiveCue, __instance.transform.position, false);
                    Console.Log("Null: Play audio");
                }
            }
            if (__instance.inactiveFX != null)
            {
                Console.Log("if inactive FX is null");

                __instance.inactiveFX.SetActive(__instance.model.state == QuicksilverEnergyGenerator.State.INACTIVE);
                Console.Log("inactive FX setted");
            }
            if (__instance.activeFX != null)
            {
                Console.Log("if activeFX is null");

                __instance.activeFX.SetActive(__instance.model.state == QuicksilverEnergyGenerator.State.ACTIVE);
                Console.Log("activeFX setted");

            }
            if (__instance.cooldownFX != null)
            {
                Console.Log("if cooldownFX is null");

                __instance.cooldownFX.SetActive(__instance.model.state == QuicksilverEnergyGenerator.State.COOLDOWN);
                Console.Log("cooldownFX setted");
            }
            if (__instance.onStateChanged != null)
            {
                Console.Log("if onstatechanged is null");

                __instance.onStateChanged();
                Console.Log("onstatechanged setted");
            }
            return false;
        }
    }
}
