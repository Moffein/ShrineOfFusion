using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShrineOfFusion.Fusion
{
    public class FusionRecipe
    {
        public Dictionary<ItemIndex, int> inputs;
        public ItemIndex output;

        public FusionRecipe(Dictionary<ItemIndex, int> inputs, ItemIndex output)
        {
            this.inputs = inputs;
            this.output = output;
        }

        public FusionRecipe()
        {
            inputs = new Dictionary<ItemIndex, int>();
        }

        public bool IsAvailable(Inventory inventory)
        {
            //Check if item is disabled
            if (Run.instance && !Run.instance.availableItems.Contains(output)) return false;

            //Check if inventory has input items
            if (!inventory) return false;

            foreach (var key in inputs.Keys)
            {
                if (inputs.TryGetValue(key, out int count))
                {
                    if (inventory.GetItemCount(key) < count) return false;
                }
            }
            return true;
        }

        public bool ApplyRecipe(Inventory inventory)
        {
            //Check is a bit redundant, but is here for extra safety
            if (!IsAvailable(inventory)) return false;

            foreach (var key in inputs.Keys)
            {
                if (inputs.TryGetValue(key, out int count))
                {
                    inventory.RemoveItem(key, count);
                }
            }

            inventory.GiveItem(output);

            //Print pickup message since we're directly giving to inventory
            PickupIndex pickup = PickupCatalog.FindPickupIndex(output);
            if (pickup != PickupIndex.none)
            {
                PickupDef pd = PickupCatalog.GetPickupDef(pickup);
                if (pd != null)
                {
                    CharacterMaster master = inventory.GetComponent<CharacterMaster>();
                    if (master)
                    {
                        CharacterBody body = master.GetBody();
                        if (body)
                        {
                            Chat.AddPickupMessage(body, pd.nameToken, pd.baseColor, 1);
                        }
                    }
                }
            }

            return true;
        }
    }
}
