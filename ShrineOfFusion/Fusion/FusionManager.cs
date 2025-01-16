using RoR2;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ShrineOfFusion.Fusion
{
    public static class FusionManager
    {
        public static List<FusionRecipe> allFusionRecipes = new List<FusionRecipe>();
        public static Xoroshiro128Plus rng;

        internal static void OnRunStartGlobal(Run run)
        {
            rng = new Xoroshiro128Plus(run.seed);
        }

        public static bool AttemptRandomFusion(Inventory inventory)
        {
            var availableRecipes = allFusionRecipes.Where(recipe => recipe.IsAvailable(inventory)).ToList();
            int count = availableRecipes.Count();
            if (count <= 0)
            {
                Debug.LogWarning("Shrine of Fusion: No available recipes.");
                return false;
            }

            FusionRecipe selected = availableRecipes[rng.RangeInt(0, count)];
            return selected.ApplyRecipe(inventory);
        }
    }
}
