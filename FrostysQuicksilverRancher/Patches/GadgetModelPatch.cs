using HarmonyLib;
using MonomiPark.SlimeRancher.DataModel;
using UnityEngine;
using FrostysQuicksilverRancher.Other;
using FrostysQuicksilverRancher.Components;
namespace PAMS.HarmonyPatches
{
    [HarmonyPatch(typeof(GameModel), "CreateGadgetModel")]
    internal class Patch_GameModel_CreateGadgetModel
    {
        private static bool Prefix(GadgetModel __instance, GadgetSiteModel site, GameObject gameObj, ref GadgetModel __result)
        {
            Gadget.Id id = gameObj.GetComponent<Gadget>().id;
            bool flag = id == Ids.ELECTRIC_STORAGE;
            bool result;

            if (flag)
            {
                __result = new ElectricStorageModel(id, site.id, gameObj.transform);
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
    }
}