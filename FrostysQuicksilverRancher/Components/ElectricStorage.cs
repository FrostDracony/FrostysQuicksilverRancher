using UnityEngine;
using SRML.Console;
using MonomiPark.SlimeRancher.DataModel;
using DG.Tweening;

namespace FrostysQuicksilverRancher.Components
{
    public class ElectricStorage : SRBehaviour, GadgetModel.Participant, GadgetInteractor
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void Awake()
		{
			sphere = transform.Find("Sphere").gameObject;
			defaultMaterial = (renderer = sphere.GetComponent<Renderer>()).material;
			ReactToShock component = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_SLIME).GetComponent<ReactToShock>();
			shockedMaterial = defaultMaterial;
			cooldownFX = component.cooldownFX;
			SiloSlotUI component2 = Instantiate(SRSingleton<GameContext>.Instance.LookupDirector.GetGadgetDefinition(Gadget.Id.WARP_DEPOT_BLUE).prefab.transform.Find("warpdepot/SiloSlotUI 0").gameObject, Vector3.zero, Quaternion.AngleAxis(180f, Vector3.up), sphere.transform).GetComponent<SiloSlotUI>();
			component2.gameObject.SetActive(true);
			component2.slotIcon.enabled = true;
			component2.bar.barColor = Color.red;
			component2.frontFrameIcon.sprite = component2.frontFilled;
			component2.backFrameIcon.sprite = component2.backFilled;
			Destroy(component2);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002178 File Offset: 0x00000378
		public void UpdateVisuals()
		{
			bool flag = model.basicChargeAmount > 0f;
			if (flag)
			{
				bool flag2 = !cooldownFXInstance;
				if (flag2)
				{
					cooldownFXInstance = SpawnAndPlayFX(cooldownFX, sphere);
				}
				renderer.material = shockedMaterial;
			}
			else
			{
				bool flag3 = cooldownFXInstance;
				if (flag3)
				{
					RecycleAndStopFX(cooldownFXInstance);
					cooldownFXInstance = null;
				}
				renderer.material = defaultMaterial;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002214 File Offset: 0x00000414
		public void IncrementBasicCharge(int amount = 1)
		{
			model.basicChargeAmount += amount;
			bool flag = model.basicChargeAmount < 0f;
			if (flag)
			{
				model.basicChargeAmount = 0f;
			}
			UpdateVisuals();
		}

		public void DecrementBasicCharge(int amount = 1)
        {
			model.basicChargeAmount -= amount;
			bool flag = model.basicChargeAmount < 0f;
			if (flag)
			{
				model.basicChargeAmount = 0f;
			}
			UpdateVisuals();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002263 File Offset: 0x00000463
		public void InitModel(GadgetModel model)
		{
		}


		// Token: 0x06000005 RID: 5 RVA: 0x00002266 File Offset: 0x00000466
		public void SetModel(GadgetModel model)
		{
			this.model = (model as ElectricStorageModel);
			UpdateVisuals();
		}

        public void OnInteract()
        {
			if (model.basicChargeAmount <= 0)
				return;
			Console.Log("Charge: ");
			DecrementBasicCharge();
			Expel(GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1));
			//InstantiateActor(GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1), SceneContext.Instance.RegionRegistry.GetCurrentRegionSetId(), transform.Find("Sphere").position + new Vector3(0, 0, -3), Quaternion.identity);
        }  

        public bool CanInteract()
        {
			if (model.basicChargeAmount > 0)
				return true;
			return false;
        }

		void Expel(GameObject toExpel)
        {
			WeaponVacuum weaponVacuum = FindObjectOfType<WeaponVacuum>();
			
			GameObject gameObject = InstantiateActor(toExpel, weaponVacuum.regionRegistry.GetCurrentRegionSetId(), transform.Find("Sphere").position + new Vector3(0, 0, -3), Quaternion.identity, false);
			gameObject.transform.LookAt(weaponVacuum.transform);
			PhysicsUtil.RestoreFreezeRotationConstraints(gameObject);
			
			gameObject.transform.DOScale(gameObject.transform.localScale, 0.1f).From(gameObject.transform.localScale * 0.2f, true).SetEase(Ease.Linear);
			gameObject.GetComponent<Vacuumable>().Launch(Vacuumable.LaunchSource.PLAYER);
		}

        // Token: 0x04000001 RID: 1
        GameObject sphere;

		// Token: 0x04000002 RID: 2
		Material shockedMaterial;

		// Token: 0x04000003 RID: 3
		Material defaultMaterial;

		// Token: 0x04000004 RID: 4
		GameObject cooldownFX;

		// Token: 0x04000005 RID: 5
		Renderer renderer;

		// Token: 0x04000006 RID: 6
		public ElectricStorageModel model;

		// Token: 0x04000007 RID: 7
		GameObject cooldownFXInstance;
	}
}
