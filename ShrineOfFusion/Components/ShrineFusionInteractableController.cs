using RoR2;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.Networking;
using ShrineOfFusion.Fusion;

namespace ShrineOfFusion.Components
{
    public class ShrineFusionInteractableController : MonoBehaviour
    {
        private PurchaseInteraction pi;
        public float cooldown = 0.5f;

        private void Awake()
        {
            pi = base.GetComponent<PurchaseInteraction>();
            if (pi)
            {
                pi.onPurchase.AddListener(new UnityAction<Interactor>(OnPurchase));
            }
        }

        private void OnPurchase(Interactor interactor)
        {
            if (pi)
            {
                pi.lastActivator = interactor;
                pi.SetUnavailableTemporarily(cooldown);
            }

            bool success = false;
            CharacterBody cb = interactor.GetComponent<CharacterBody>();
            if (cb && cb.inventory)
            {
                FusionManager.AttemptRandomFusion(cb.inventory);
            }
        }
    }
}
