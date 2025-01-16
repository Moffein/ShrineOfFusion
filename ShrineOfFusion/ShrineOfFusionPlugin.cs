using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using ShrineOfFusion.Fusion;
using System;
using System.Collections.Generic;

namespace ShrineOfFusion
{
    [BepInPlugin("com.Moffein.ShrineOfFusion", "Shrine Of Fusion", "0.0.1")]
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency(R2API.ContentManagement.R2APIContentManager.PluginGUID)]
    [BepInDependency(R2API.PrefabAPI.PluginGUID)]
    [BepInDependency(R2API.LanguageAPI.PluginGUID)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class ShrineOfFusionPlugin : BaseUnityPlugin
    {
        public static int directorCost;
        public static int selectionWeight;

        private void Awake()
        {
            directorCost = Config.Bind("General", "Director Cost", 10, "Director Cost of the interactable.").Value;
            selectionWeight = Config.Bind("General", "Selection Weight", 1, "Likelihood of the interactable being selected.").Value;

            Modules.Assets.Init();
            RoR2.Run.onRunStartGlobal += FusionManager.OnRunStartGlobal;
            LanguageAPI.Add("SHRINE_FUSION_NAME", "Shrine of Fusion");
            LanguageAPI.Add("SHRINE_FUSION_CONTEXT", "Fuse Items");

            RoR2Application.onLoad += OnLoad;
        }

        //Create some preset fusion recipes to test
        private void OnLoad()
        {
            FusionManager.allFusionRecipes.Add(new FusionRecipe
                {
                    inputs = new Dictionary<ItemIndex, int>
                    {
                        {RoR2Content.Items.StickyBomb.itemIndex, 2},
                        {RoR2Content.Items.Firework.itemIndex, 1}
                    },
                    output = RoR2Content.Items.Missile.itemIndex
                }
            );

            FusionManager.allFusionRecipes.Add(new FusionRecipe
            {
                inputs = new Dictionary<ItemIndex, int>
                {
                    {RoR2Content.Items.Missile.itemIndex, 3}
                },
                output = DLC1Content.Items.MoreMissile.itemIndex
            }
            );

            FusionManager.allFusionRecipes.Add(new FusionRecipe
            {
                inputs = new Dictionary<ItemIndex, int>
                {
                    {DLC2Content.Items.LowerHealthHigherDamage.itemIndex, 1},
                    {RoR2Content.Items.IgniteOnKill.itemIndex, 1}
                },
                output = RoR2Content.Items.ExplodeOnDeath.itemIndex
            }
            );
        }
    }
}
