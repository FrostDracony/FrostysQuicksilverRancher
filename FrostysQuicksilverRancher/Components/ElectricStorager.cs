using UnityEngine;
using SRML.SR;
using SRML.Console;
using MonomiPark.SlimeRancher.DataModel;

namespace FrostysQuicksilverRancher.Components
{
    //Taking old Quicksilver generator's code and reusing it
    public class ElectricStorager : Gadget, GadgetModel.Participant, GadgetInteractor
    {
        //The variables
        public ElectricStorageModel storage;
        GameObject parent;
        GameObject cooldownFX;
        GameObject cooldownFXInstance;
        Material defaultMaterial;
        Material shockedMaterial;
        Renderer renderer;
        int maxCharge = 10;
        //This will run as soon as the gadget is placed
        public override void Awake()
        {
            base.Awake();
            //Console.Log("ElectricStorager loading");
            gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            parent = gameObject.transform.parent.gameObject;
            defaultMaterial = (renderer = gameObject.FindChild("default").GetComponent<MeshRenderer>()).material;
            ReactToShock component1 = SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.QUICKSILVER_SLIME).GetComponent<ReactToShock>();
            shockedMaterial = defaultMaterial;
            cooldownFX = component1.cooldownFX;
        }

        public void UpdateVisuals()
        {
            //Console.Log("UpdateVisuals");
            if (storage.chargeAmount > 0.0)
            {
                if (!cooldownFXInstance)
                {
                    //Console.Log("chargeamount is over 0");
                    cooldownFXInstance = SRBehaviour.SpawnAndPlayFX(cooldownFX, gameObject);
                    //Console.Log("setting particles");
                    cooldownFXInstance.transform.position += new Vector3(0, 2.75f, 0);
                }
                //Console.Log("Setting chargedmaterial");
                renderer.material = shockedMaterial;
            }
            else
            {
                //Console.Log("charge amount is 0");
                if (cooldownFXInstance)
                {
                    //Console.Log("removing particles");
                    SRBehaviour.RecycleAndStopFX(cooldownFXInstance);
                    cooldownFXInstance = null;
                }
                //Console.Log("setting normal material");
                renderer.material = defaultMaterial;
            }

            //Console.Log("storage.chargeAmount >= maxCharge " + (storage.chargeAmount >= maxCharge));
            //Console.Log("parent.FindChild(\"charged_rings\") " + (parent.FindChild("charged_rings") != null));

            if (storage.chargeAmount >= maxCharge && parent.FindChild("charged_rings").activeSelf == false)
            {
                //Console.Log("TmeToMakeYellowRings");
                parent.FindChild("normal_rings").SetActive(false);
                parent.FindChild("charged_rings").SetActive(true);
            }

            //Console.Log("storage.chargeAmount < maxCharge " + (storage.chargeAmount < maxCharge));
            if (storage.chargeAmount < maxCharge && parent.FindChild("charged_rings").activeSelf == true)
            {
                //Console.Log("");
                parent.FindChild("normal_rings").SetActive(true);
                parent.FindChild("charged_rings").SetActive(false);
            }
        }

        public bool CanInteract()
        {
            if(storage.chargeAmount > 0 && storage.chargeAmount <= maxCharge)
                return true;
            return false;
        }

        public void InitModel(GadgetModel model)
        {
            /*storage = model as ElectricStorageModel;
            //Console.Log("Model initted with " + storage.chargeAmount + " shots");*/
        }

        public void OnInteract()
        {
            //var g = InstantiateActor(GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1), SceneContext.Instance.PlayerState.model.currRegionSetId);
            SceneContext.Instance.PlayerState?.Ammo.MaybeAddToSlot(Identifiable.Id.VALLEY_AMMO_1, GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.VALLEY_AMMO_1).GetComponent<Identifiable>());
            //DestroyImmediate(g);
            //Console.Log("Amount before removing: " + storage.chargeAmount);
            storage.chargeAmount--;
            //Console.Log("Amount after removing: " + storage.chargeAmount);
            UpdateVisuals();
        }

        public void SetModel(GadgetModel model)
        {
            storage = model as ElectricStorageModel;
            UpdateVisuals();
            //Console.Log("Model setted with " + storage.chargeAmount + " shots");
        }

        void OnCollisionEnter(Collision other)
		{
            bool hasId = other.gameObject.GetComponent<Identifiable>() != null;
            if(hasId)
            {
                if (storage.chargeAmount < maxCharge) {
                    if (other.gameObject.GetComponent<Identifiable>().id == Identifiable.Id.VALLEY_AMMO_1)
                    {
                        //Console.Log("Amount before adding: " + storage.chargeAmount);
                        storage.chargeAmount++;
                        //Console.Log("Amount after adding: " + storage.chargeAmount);
                        UpdateVisuals();
                    } 
                }
            }
		}


	}
 
}
