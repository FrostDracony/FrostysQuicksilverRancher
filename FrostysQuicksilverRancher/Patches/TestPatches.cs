using UnityEngine;

using Configs;
using SRML.Console;
using HarmonyLib;

using MonomiPark.SlimeRancher.Regions;

using FrostysQuicksilverRancher.Other;

namespace FrostysQuicksilverRancher.Patches
{
	[HarmonyPatch(typeof(QuicksilverPlortCollector))]
	[HarmonyPatch("Update")]
	public static class QuicksilverPlortCollectorUpdatePatch
	{
		public static bool Prefix(QuicksilverPlortCollector __instance)
        {
			//Console.Log("Yay");
			if (__instance.timeDirector.HasReached(__instance.timer))
            {
				//Console.Log("Yay");
				if (SRSingleton<SceneContext>.Instance.PlayerState.HasUpgrade(Ids.MOCHI_HACK))
				{
					bool isInMochiZone = __instance.GetComponent<RegionMember>().IsInZone(ZoneDirector.Zone.MOCHI_RANCH);
					//Console.Log("isInMochiZone: " + isInMochiZone);
					bool isInValleyZone = __instance.GetComponent<RegionMember>().IsInZone(ZoneDirector.Zone.VALLEY);
					//Console.Log("isInValleyZone: " + isInValleyZone);

					if (isInMochiZone || isInValleyZone)
					{
						Ammo ammo = SceneContext.Instance.PlayerState.ammoDict[PlayerState.AmmoMode.DEFAULT];
						Identifiable component = __instance.GetComponent<Identifiable>();
						//Console.Log("Test to see if the game allows the addition");
						if (ammo.MaybeAddToSlot(component.id, component))
						{
							//Console.Log("SUCCESS!");
							if (__instance.destroyFX != null)
							{
								SRBehaviour.SpawnAndPlayFX(__instance.destroyFX, __instance.transform.position, Quaternion.identity);
							}
							SECTR_AudioSystem.Play(__instance.onCollectionCue, __instance.transform.position, false);
							Destroyer.DestroyActor(__instance.gameObject, "QuicksilverPlortCollector.Update", false);
							__instance.pediaDirector.MaybeShowPopup(component.id);
						}
						return true;
					}
				} else { return false; }
			}

			return true;
		}
    }

	[HarmonyPatch(typeof(QuicksilverEnergyGenerator))]
    [HarmonyPatch("SetState")]
    public static class QuicksilverEnergyGeneratorSetStatePatch
	{
		public static bool Prefix(QuicksilverEnergyGenerator __instance, QuicksilverEnergyGenerator.State state, bool enableSFX)
		{
			//Console.Log("Parameters are: " + state + " and " + enableSFX);
			if(state == QuicksilverEnergyGenerator.State.INACTIVE && enableSFX == false)
				return true;


			//Console.Log("if state is: " + (state == QuicksilverEnergyGenerator.State.COOLDOWN) + " and " + (Values.VACPACK == VACPACK_ENUMS.DEFAULT));
			//Console.Log("Lets see if game can find VacDisplayChanger");
			//Console.Log(Object.FindObjectOfType<VacDisplayChanger>().name);
			//Console.Log("Ok game can find VacDisplayChanger");
			/*if (state == QuicksilverEnergyGenerator.State.COOLDOWN && Values.VACPACK == VACPACK_ENUMS.DEFAULT)
				//Console.Log("TimeToSwitchToDefault"); SceneContext.Instance.Player.GetComponentInChildren<WeaponVacuum>().GetComponentInChildren<VacDisplayChanger>().SetDisplayMode(PlayerState.AmmoMode.DEFAULT);

			//Console.Log("QSR pls work for once");

			//Console.Log("if state is: " + (state == QuicksilverEnergyGenerator.State.COUNTDOWN) + " and " + (Values.VACPACK == VACPACK_ENUMS.DEFAULT));
			if (state == QuicksilverEnergyGenerator.State.COUNTDOWN && Values.VACPACK == VACPACK_ENUMS.DEFAULT)
				//Console.Log("TimeToSwitchToNimble"); SceneContext.Instance.Player.GetComponentInChildren<WeaponVacuum>().GetComponentInChildren<VacDisplayChanger>().SetDisplayMode(PlayerState.AmmoMode.NIMBLE_VALLEY);*/


			if(__instance.countdownUI != null)
				Destroyer.Destroy(__instance.countdownUI, "QuicksilverEnergyGenerator.SetState");
			//Console.Log("CountdownUI destroyed");
			__instance.model.state = state;
			//Console.Log("model.state setted");

			if (__instance.model.state == QuicksilverEnergyGenerator.State.COUNTDOWN)
			{
				//Console.Log("if state is countdown");
				if(Values.VACPACK == VACPACK_ENUMS.DEFAULT)
                {
					//Console.Log("TimeToSwitchToNimble");
					SceneContext.Instance.Player.GetComponentInChildren<WeaponVacuum>().GetComponentInChildren<VacDisplayChanger>().SetDisplayMode(PlayerState.AmmoMode.NIMBLE_VALLEY);
				}

				__instance.model.timer = new double?(__instance.timeDirector.HoursFromNow(__instance.countdownMinutes * 0.016666668f));
				////Console.Log("Countdown: timer setted");

				if (enableSFX)
				{
					//Console.Log("Countdown: if enable SFX");

					SECTR_AudioSystem.Play(__instance.onCountdownCue, __instance.transform.position, false);
					//Console.Log("Audio played");

				}
				bool flag = SRSingleton<SceneContext>.Instance.PlayerState.HasUpgrade(Ids.MOCHI_HACK);
				//Console.Log("User has mochi upgrade: " + flag);
				if (flag)
				{
					VACPACK_ENUMS flag1 = Values.VACPACK;
					//Console.Log("User has " + flag1 + " upgrade");
					switch (flag1)
					{
						case VACPACK_ENUMS.NIMBLE_VALLEY:
							//Console.Log("NimbleValley");
							//Console.Log("WeaponVauum " + SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>().gameObject.activeInHierarchy);
							//Console.Log("VacDisplayTimer " + SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>().GetComponentInChildren<VacDisplayTimer>().gameObject.activeInHierarchy);
							
							GameObject.Find("SimplePlayer/FPSCamera/VacuumTransform/Vacuum/model_vac_v3_prefab(Clone)/Scaler/bone_vac/Vac Display").SetActive(true);
							SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>().GetComponentInChildren<VacDisplayTimer>().SetQuicksilverEnergyGenerator(__instance);
							break;

						case VACPACK_ENUMS.DEFAULT:
							//Console.Log("Default");
							//Console.Log("WeaponVauum " + SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>().gameObject.activeInHierarchy);
							//Console.Log("VacDisplayTimer " + SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>().GetComponentInChildren<VacDisplayTimer>().gameObject.activeInHierarchy);

							GameObject.Find("SimplePlayer/FPSCamera/VacuumTransform/Vacuum/model_vac_v3_prefab(Clone)/Scaler/bone_vac/Vac Display").SetActive(true);
							SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>().GetComponentInChildren<VacDisplayTimer>().SetQuicksilverEnergyGenerator(__instance);
							break;

						case VACPACK_ENUMS.AUTOMATIC:
							//Console.Log("Automatic");
							//Console.Log("WeaponVauum " + SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>().gameObject.activeInHierarchy);
							//Console.Log("VacDisplayTimer " + SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>().GetComponentInChildren<VacDisplayTimer>().gameObject.activeInHierarchy);

							ZoneDirector.Zone currentZone = SRSingleton<SceneContext>.Instance.PlayerZoneTracker.GetCurrentZone();
							PlayerState.AmmoMode ammoMode = (currentZone == ZoneDirector.Zone.MOCHI_RANCH || currentZone == ZoneDirector.Zone.VALLEY) ? PlayerState.AmmoMode.NIMBLE_VALLEY : PlayerState.AmmoMode.DEFAULT;
							if (ammoMode == PlayerState.AmmoMode.NIMBLE_VALLEY)
                            {
								GameObject.Find("SimplePlayer/FPSCamera/VacuumTransform/Vacuum/model_vac_v3_prefab(Clone)/Scaler/bone_vac/Vac Display").SetActive(true);
								SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>().GetComponentInChildren<VacDisplayTimer>().SetQuicksilverEnergyGenerator(__instance);
                            }
							break;
					}
				}
				else { /*Console.Log("Vanilla");*/ SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>().GetComponentInChildren<VacDisplayTimer>().SetQuicksilverEnergyGenerator(__instance); }

				//Console.Log("Countdown: VacDisplayTimer generator setted");

				__instance.countdownUI = Object.Instantiate(__instance.countdownUIPrefab);
				//Console.Log("Countdown: countdownUI created");

				__instance.countdownUI.GetComponent<HUDCountdownUI>().SetCountdownTime(__instance.countdownMinutes);
				//Console.Log("Countdown: Timer time setted");

			}
			else if (__instance.model.state == QuicksilverEnergyGenerator.State.ACTIVE)
			{
				//Console.Log("if state is active");

				__instance.model.timer = new double?(__instance.timeDirector.HoursFromNow(__instance.activeHours));
				//Console.Log("Active: Timer setted");

			}
			else if (__instance.model.state == QuicksilverEnergyGenerator.State.COOLDOWN)
			{
				//Console.Log("if state is cooldown");
				if (Values.VACPACK == VACPACK_ENUMS.DEFAULT)
				{
					//Console.Log("TimeToSwitchToDefault");
					SceneContext.Instance.Player.GetComponentInChildren<WeaponVacuum>().GetComponentInChildren<VacDisplayChanger>().SetDisplayMode(PlayerState.AmmoMode.DEFAULT);
				}

				__instance.model.timer = new double?(__instance.timeDirector.HoursFromNow(__instance.cooldownHours));
				//Console.Log("Cooldown: timer setted");

				if (enableSFX)
				{
					//Console.Log("Cooldown: if enableSFX");

					SECTR_AudioSystem.Play(__instance.onCooldownCue, __instance.transform.position, false);
					//Console.Log("Cooldown: Play first audio");

					SECTR_AudioSystem.Play(__instance.onCooldownCue2D, Vector3.zero, false);
					//Console.Log("Cooldown: Play second audio");

				}

			}
			else
			{
				//Console.Log("if everything is null");

				__instance.model.timer = null;
				//Console.Log("Null: Timer setted");

				if (enableSFX)
				{
					//Console.Log("Null: if enableSFX");

					SECTR_AudioSystem.Play(__instance.onInactiveCue, __instance.transform.position, false);
					//Console.Log("Null: Play audio");

				}
			}
			if (__instance.inactiveFX != null)
			{
				//Console.Log("if inactive FX is null");

				__instance.inactiveFX.SetActive(__instance.model.state == QuicksilverEnergyGenerator.State.INACTIVE);
				//Console.Log("inactive FX setted");

			}
			if (__instance.activeFX != null)
			{
				//Console.Log("if activeFX is null");

				__instance.activeFX.SetActive(__instance.model.state == QuicksilverEnergyGenerator.State.ACTIVE);
				//Console.Log("activeFX setted");

			}
			if (__instance.cooldownFX != null)
			{
				//Console.Log("if cooldownFX is null");

				__instance.cooldownFX.SetActive(__instance.model.state == QuicksilverEnergyGenerator.State.COOLDOWN);
				//Console.Log("cooldownFX setted");

			}
			if (__instance.onStateChanged != null)
			{
				//Console.Log("if onstatechanged is null");

				__instance.onStateChanged();
				//Console.Log("onstatechanged setted");

			}

			return false;

		}
    }
}
