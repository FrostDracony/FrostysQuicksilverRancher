using HarmonyLib;
using SRML.Console;
using System.Collections.Generic;
using System.Collections;
using FrostysQuicksilverRancher.Components;

namespace FrostysQuicksilverRancher.Patches
{
	[HarmonyPatch(typeof(PlayerZoneTracker))]
	[HarmonyPatch("OnEntered")]
	static class ZonePatchEntered
	{
		public static void Postfix(ZoneDirector.Zone zone)
		{
			if (zone == ZoneDirector.Zone.MOCHI_RANCH)
			{
				SRSingleton<SceneContext>.Instance.PlayerState.CheckAllUpgradeLockers();
			}
		}
	}

	[HarmonyPatch(typeof(PlayerZoneTracker))]
	[HarmonyPatch("OnExited")]
	static class ZonePatchExited
	{
		public static void Postfix(ZoneDirector.Zone zone) //When the player leaves a zone
		{
			if (zone == ZoneDirector.Zone.MOCHI_RANCH) //If it was Mochi's Ranch
			{
				//Variables
				Ammo ammo = SceneContext.Instance.PlayerState.Ammo;
				//Console.Log("Slots length: " + ammo.Slots.Length + " selected idx: " + ammo.GetSelectedAmmoIdx());
                
				for (int i = 0; i < ammo.Slots.Length; i++) //Looping through all the slots of the player
                {
					Ammo.Slot slot = ammo.Slots[i]; //Curent slot
					//Console.Log("Congrats, you've just stolen " + slot.count + " " + slot.id + " !");

					/*MochiWarning mochiWarning = SceneContext.Instance.Player.GetComponent<MochiWarning>();
					if (slot.id == Identifiable.Id.QUICKSILVER_SLIME && mochiWarning.IsMochiAlarmed(slot.count)) //If slimes are in that slot and there are too many quicksilvers
                    {

						//mochiWarning.IncrementWarning(); //Start the whole thing

						*//*SceneContext.Instance.MailDirector.model.allMail.Add(mail);
						SceneContext.Instance.MailDirector.model.allMailDict[mail] = mail;
						SceneContext.Instance.MailDirector.model.MailListChanged();*//*
					}*/
				}

			}

		}
	}

}