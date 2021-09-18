using System;
using System.Reflection;
using System.Linq;
using FrostysQuicksilverRancher.Components;
using FrostysQuicksilverRancher.Other;
using MonomiPark.SlimeRancher.DataModel;
using SRML;
using Console = SRML.Console.Console;
using SRML.SR;
using SRML.SR.SaveSystem;
using SRML.SR.Translation;
using SRML.Utils;
using UnityEngine;

namespace FrostysQuicksilverRancher
{
    public class Main : ModEntryPoint
    {
        public static AssetBundle assetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Main), "quicksilverrancher"));

        public override void PreLoad()
        {
            HarmonyInstance.PatchAll();
            SaveRegistry.RegisterDataParticipant<PlortUndisappearifier>();
            //SaveRegistry.RegisterDataParticipant<MochiWarning>();
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
                DroneRegistry.RegisterBasicTarget(Identifiable.Id.QUICKSILVER_PLORT);

                /*MailRegistry.RegisterMailEntry("quicksilver_stolen_warning_1").SetFromTranslation("Mochi Miles").SetBodyTranslation("My name is Mochi Miles, you took my quicksilvers, dont do that ever again, warning 1");
                MailRegistry.RegisterMailEntry("quicksilver_stolen_warning_2").SetFromTranslation("Mochi Miles").SetBodyTranslation("My name is Mochi Miles, you took my quicksilvers, again, dont do that ever again, warning 2");
                MailRegistry.RegisterMailEntry("quicksilver_stolen_warning_3").SetFromTranslation("Mochi Miles").SetBodyTranslation("My name is Mochi Miles, you took my quicksilvers, if you dont want to start a war, dont do that ever again, last warning");
                MailRegistry.RegisterMailEntry("quicksilver_stolen_war").SetFromTranslation("Mochi Miles").SetBodyTranslation("My name is Mochi Miles, you took my quicksilvers, prepare to loose your ranch");
                MailRegistry.RegisterMailEntry("quicksilver_stolen_ending").SetFromTranslation("Mochi Miles").SetBodyTranslation("My name is Mochi Miles, I give up");*/
                if (!SRModLoader.IsModPresent("more_vaccing"))
                {
                    LookupRegistry.RegisterVacEntry(Identifiable.Id.QUICKSILVER_SLIME, Color.grey, SRSingleton<SceneContext>.Instance.PediaDirector.entries.First((PediaDirector.IdEntry x) => x.id == (PediaDirector.Id)Enum.Parse(typeof(PediaDirector.Id), Identifiable.Id.QUICKSILVER_SLIME.ToString())).icon);
                    AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_SLIME));
                }

                Console.RegisterCommand(new ChangeVacVisualCommand());
                Console.RegisterCommand(new PrintComponentsOfGadgetCommand());

                LookupRegistry.RegisterUpgradeEntry(Ids.MOCHI_HACK, assetBundle.LoadAsset<Sprite>("upgrade_initial"), 10000);

                Ids.MOCHI_HACK.GetTranslation().SetDescriptionTranslation("Hack the gate at Mochi's Manor to make it ineffective and go in/out with your normal inventory. But you have to be careful, Mochi may or may not see when her precious quicksilver slimes disappear, and if that happens, she will attack your base, trying to take them back!").SetNameTranslation("Gate Hack");

                PersonalUpgradeRegistry.RegisterUpgradeLock(Ids.MOCHI_HACK, (PlayerState x) => x.CreateBasicLock(null, delegate
                {
                    ProgressModel model = SRSingleton<SceneContext>.Instance.ProgressDirector.model;
                    return model != null && model.HasProgress(ProgressDirector.ProgressType.UNLOCK_MOCHI_MISSIONS);
                }, 1f));

                GameObject elecStorage = assetBundle.LoadAsset<GameObject>("gadgetElectricStorage");
                GadgetDefinition gadgetDefinition = GameContext.Instance.LookupDirector.GetGadgetDefinition(Gadget.Id.EXTRACTOR_DRILL_NOVICE);
                foreach(MeshRenderer renderer in gadgetDefinition.prefab.GetComponentsInChildren<MeshRenderer>())
                {
                    Console.Log("Renderer: " + renderer);
                    foreach (Material material in renderer.sharedMaterials)
                        Console.Log("Shader: " + material.shader);
                    Console.Log(renderer.sharedMaterial.shader.name);
                }
                elecStorage.AddComponent<Gadget>().GetCopyOf(gadgetDefinition.prefab.GetComponent<Gadget>());

                PrefabUtils.ReplaceFieldsWith(elecStorage, Gadget.Id.EXTRACTOR_DRILL_NOVICE, Ids.ELECTRIC_STORAGE);
                elecStorage.AddComponent<ElectricStorage>();
                elecStorage.transform.Find("Sphere").gameObject.AddComponent<ElectricStorageTrigger>();
                LookupRegistry.RegisterGadget(new GadgetDefinition
                {
                    prefab = elecStorage,
                    id = Ids.ELECTRIC_STORAGE,
                    pediaLink = PediaDirector.Id.UTILITIES,
                    blueprintCost = 1000,
                    buyCountLimit = 10,
                    craftCosts = new GadgetDefinition.CraftCost[]
                    {
                        new GadgetDefinition.CraftCost
                        {
                            amount = 50,
                            id = Identifiable.Id.QUICKSILVER_PLORT
                        },
                        new GadgetDefinition.CraftCost
                        {
                            amount = 5,
                            id = Identifiable.Id.GOLD_PLORT
                        }
                    }
                });
                Ids.ELECTRIC_STORAGE.GetTranslation().SetNameTranslation("Electric Storage").SetDescriptionTranslation("A High-Tech machine that can store static charge.");
                SaveRegistry.RegisterSerializableGadgetModel<ElectricStorageModel>(0);
                DataModelRegistry.RegisterCustomGadgetModel(Ids.ELECTRIC_STORAGE, typeof(ElectricStorageModel));
                GadgetRegistry.RegisterBlueprintLock(Ids.ELECTRIC_STORAGE, (GadgetDirector x) => x.CreateBasicLock(Ids.ELECTRIC_STORAGE, Gadget.Id.NONE, 1));
            }
            else
            {
                throw new Exception("Quicksilver Rancher mod (the old and original) is there, please remove it if you consider using this mod");
            }

        }
        
        public override void PostLoad()
        {
            SRCallbacks.OnSaveGameLoaded += SRCallbacks_OnSaveGameLoaded;
        }

        private void SRCallbacks_OnSaveGameLoaded(SceneContext t)
        {
            //SceneContext.Instance.Player.AddComponent<MochiWarning>();
        }
    }
}
