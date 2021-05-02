using System;
using System.Linq;
using System.Reflection;

using MonomiPark.SlimeRancher.DataModel;

using SRML;
using SRML.SR;
using SRML.SR.Translation;
using SRML.SR.SaveSystem;

using UnityEngine;

using FrostysQuicksilverRancher.Components;
using FrostysQuicksilverRancher.Other;

namespace FrostysQuicksilverRancher
{
    public class Main : ModEntryPoint
    {
        public static AssetBundle assetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Main), "quicksilverrancher"));
        public override void PreLoad()
        {
            HarmonyInstance.PatchAll();
            SRML.Console.Console.RegisterCommand(new ChangeVacVisualCommand());
            SaveRegistry.RegisterDataParticipant<PlortUndisappearifier>();

            LookupRegistry.RegisterUpgradeEntry(Ids.MOCHI_HACK, Main.assetBundle.LoadAsset<Sprite>("upgrade_initial"), 10000);

            Ids.MOCHI_HACK.GetTranslation().SetDescriptionTranslation("Hack the gate at Mochi's Manor to make it ineffective and go in/out with your normal inventory. But you have to be careful, Mochi may or may not see when her precious quicksilver slimes disappear, and if that happens, she will attack your base, trying to take them back!").SetNameTranslation("Gate Hack");
            PersonalUpgradeRegistry.RegisterUpgradeLock(Ids.MOCHI_HACK, (PlayerState x) => x.CreateBasicLock(null, delegate
            {
                ProgressModel model = SRSingleton<SceneContext>.Instance.ProgressDirector.model;
                return model != null && model.HasProgress(ProgressDirector.ProgressType.ENTER_ZONE_MOCHI_RANCH);
            }, 0f));

        }

        public override void Load()
        {

            if (!SRModLoader.IsModPresent("quicksilver_rancher"))
            {
                GameObject prefab = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_SLIME);

                AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_PLORT));
                AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1));
                AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_3));
                AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_2));
                AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_4));
                AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, prefab);

                AmmoRegistry.RegisterRefineryResource(Identifiable.Id.QUICKSILVER_PLORT);

                //LookupRegistry.RegisterVacEntry(Identifiable.Id.QUICKSILVER_PLORT, Color.grey, SRSingleton<SceneContext>.Instance..entries.First((PediaDirector.IdEntry x) => x.id == PediaDirector.Id.QUICKSILVER_PLORT).icon);

                PlortRegistry.AddEconomyEntry(Identifiable.Id.QUICKSILVER_PLORT, 200f, 100f);

                PlortRegistry.AddPlortEntry(Identifiable.Id.QUICKSILVER_PLORT, new ProgressDirector.ProgressType[]
                {
                ProgressDirector.ProgressType.ENTER_ZONE_MOCHI_RANCH
                });

                AmmoRegistry.RegisterSiloAmmo((SiloStorage.StorageType x) => x == SiloStorage.StorageType.NON_SLIMES || x == SiloStorage.StorageType.PLORT, Identifiable.Id.QUICKSILVER_PLORT);
            }
            else
            {
                throw new Exception("Quicksilver Rancher mod (the old and original) is there, please remove it if you consider using this mod");
            }

        }
    }
}
