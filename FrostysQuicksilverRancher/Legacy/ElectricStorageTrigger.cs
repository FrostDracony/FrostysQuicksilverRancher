using MonomiPark.SlimeRancher.Regions;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FrostysQuicksilverRancher.Components
{
	class ElectricStorageTrigger : SRBehaviour, VacShootAccelerator
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002288 File Offset: 0x00000488
		public void Awake()
		{
			storage = GetComponentInParent<ElectricStorage>();
			destroyFX = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponentInChildren<DestroyAndShockOnTouching>().destroyFX;
			gameObject.layer = 15;
			GetComponent<SphereCollider>().radius = 2f;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022E8 File Offset: 0x000004E8
		public float GetVacShootSpeedFactor()
		{
			return shootAccelFactor;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002300 File Offset: 0x00000500
		public void Update()
		{
			if (Time.time < shootAccelUntil)
			{
				shootAccelFactor = Math.Min(shootAccelFactor + Time.deltaTime * 0.5f, 3f);
			}
			else
			{
				shootAccelFactor = 1f;
			}
			if (Time.time < pullAccelUntil)
			{
				pullAccelFactor = Math.Min(pullAccelFactor + Time.deltaTime * 0.5f, 1.75f);
			}
			else
			{
				pullAccelFactor = 1f;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002398 File Offset: 0x00000598
		public GameObject GetRelevantStoredPrefab()
		{
			return SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023C0 File Offset: 0x000005C0
		public void OnTriggerEnter(Collider collider)
		{
			Identifiable componentInChildren = collider.GetComponentInChildren<Identifiable>();
			Identifiable.Id id = (componentInChildren != null) ? componentInChildren.id : Identifiable.Id.NONE;
			Identifiable.Id id2 = id;
			Identifiable.Id id3 = id2;
			if (id3 - Identifiable.Id.VALLEY_AMMO_1 <= 1)
			{
				bool flag = ejected.Contains(collider.gameObject);
				if (!flag)
				{
					StoreObject(collider);
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002414 File Offset: 0x00000614
		void StoreObject(Collider collider)
		{
			SRBehaviour.SpawnAndPlayFX(destroyFX, collider.transform.position, collider.transform.rotation);
			Destroyer.DestroyActor(collider.gameObject, "ElectricStorageTrigger", true);
			storage.IncrementBasicCharge(1);
			shootAccelUntil = Time.time + 1f;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002474 File Offset: 0x00000674
		public void OnTriggerExit(Collider collider)
		{
			ejected.Remove(collider.gameObject);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000248C File Offset: 0x0000068C
		public void OnTriggerStay(Collider collider)
		{
			SiloActivator componentInParent = collider.gameObject.GetComponentInParent<SiloActivator>();
			if (componentInParent != null && componentInParent.enabled && Time.time > nextEjectTime)
			{
				if (storage.model.chargeAmount > 0f)
				{
					Vector3 normalized = (collider.gameObject.transform.position - transform.position).normalized;
					GameObject gameObject = SRBehaviour.InstantiateActor(GetRelevantStoredPrefab(), RegionRegistry.RegionSetId.UNSET, transform.position + normalized * 2f, transform.rotation, false);
					ejected.Add(gameObject);
					Destroy(gameObject.GetComponent<DestroyAndShockOnTouching>());
					gameObject.AddComponent<GravitateTowards>().SetTarget(gameObject);
					storage.IncrementBasicCharge(-1);
					SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>().ForceJoint(gameObject.GetComponent<Vacuumable>());
					nextEjectTime = Time.time + 0.25f / pullAccelFactor;
					pullAccelUntil = Time.time + 1f;
				}
			}
			if (ejected.Contains(collider.gameObject) && Vector3.Distance(transform.position, collider.transform.position) < 1.5f)
			{
				StoreObject(collider);
			}
		}

		// Token: 0x04000008 RID: 8
		ElectricStorage storage;

		// Token: 0x04000009 RID: 9
		GameObject destroyFX;

		// Token: 0x0400000A RID: 10
		HashSet<GameObject> ejected = new HashSet<GameObject>();

		// Token: 0x0400000B RID: 11
		float shootAccelFactor = 1f;

		// Token: 0x0400000C RID: 12
		float shootAccelUntil;

		// Token: 0x0400000D RID: 13
		float pullAccelFactor = 1f;

		// Token: 0x0400000E RID: 14
		float pullAccelUntil;

		// Token: 0x0400000F RID: 15
		float nextEjectTime;
	}
}
