using R2API;
using RoR2;
using ShrineOfFusion.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ShrineOfFusion.Modules
{
    public class Assets
    {
        public static GameObject ShrineOfFusionPrefab;
        public static InteractableSpawnCard ShrineOfFusionSpawnCard;
        public static DirectorCard ShrineOfFusionDirectorCard;
        public static AssetBundle assetBundle;

        internal static void Init()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ShrineOfFusion.shrineoffusionbundle"))
            {
                assetBundle = AssetBundle.LoadFromStream(stream);
            }

            BuildPrefab();
            BuildCards();
        }

        private static void BuildPrefab()
        {
            GameObject go = assetBundle.LoadAsset<GameObject>("ShrineOfFusionInteractable");
            go.AddComponent<NetworkIdentity>();

            ChildLocator cl = go.GetComponent<ChildLocator>();
            Transform modelTransform = cl.FindChild("Model");

            Highlight highlight = go.AddComponent<Highlight>();
            highlight.targetRenderer = modelTransform.GetComponent<MeshRenderer>();
            highlight.strength = 1f;
            highlight.highlightColor = Highlight.HighlightColor.interactive;
            highlight.isOn = false;

            EntityLocator el = go.AddComponent<EntityLocator>();
            el.entity = go;

            PurchaseInteraction pi = go.AddComponent<PurchaseInteraction>();
            pi.cost = 0;
            pi.costType = CostTypeIndex.None;
            pi.displayNameToken = "SHRINE_FUSION_NAME";
            pi.contextToken = "SHRINE_FUSION_CONTEXT";
            pi.setUnavailableOnTeleporterActivated = true;
            pi.isShrine = true;
            pi.isGoldShrine = false;
            pi.ignoreSpherecastForInteractability = false;
            pi.available = true;

            go.AddComponent<ShrineFusionInteractableController>();

            PrefabAPI.RegisterNetworkPrefab(go);    //This auto adds it to the contentpack
            ShrineOfFusionPrefab = go;
        }

        private static void BuildCards()
        {
            if (!ShrineOfFusionPrefab)
            {
                Debug.LogError("Shrine of Fusion: Could not build spawncard because prefab is null. Mod will not work!");
                return;
            }

            ShrineOfFusionSpawnCard = ScriptableObject.CreateInstance<InteractableSpawnCard>();
            ShrineOfFusionSpawnCard.maxSpawnsPerStage = 1;
            ShrineOfFusionSpawnCard.occupyPosition = true;
            ShrineOfFusionSpawnCard.prefab = ShrineOfFusionPrefab;
            ShrineOfFusionSpawnCard.slightlyRandomizeOrientation = true;
            ShrineOfFusionSpawnCard.requiredFlags = RoR2.Navigation.NodeFlags.None;
            ShrineOfFusionSpawnCard.orientToFloor = true;
            ShrineOfFusionSpawnCard.hullSize = HullClassification.Human;
            ShrineOfFusionSpawnCard.sendOverNetwork = false;
            ShrineOfFusionSpawnCard.directorCreditCost = ShrineOfFusionPlugin.directorCost;
            ShrineOfFusionSpawnCard.name = "iscShrineFusion";
            (ShrineOfFusionSpawnCard as ScriptableObject).name = ShrineOfFusionSpawnCard.name;

            ShrineOfFusionDirectorCard = new DirectorCard()
            {
                spawnCard = ShrineOfFusionSpawnCard,
                selectionWeight = ShrineOfFusionPlugin.selectionWeight
            };

            DirectorAPI.Helpers.AddNewInteractable(new DirectorAPI.DirectorCardHolder
            {
                Card = Modules.Assets.ShrineOfFusionDirectorCard,
                InteractableCategory = DirectorAPI.InteractableCategory.Shrines
            });
        }
    }
}
