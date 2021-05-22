// ExampleRecipes
// Example mod that shows how to load multiple recipes from a config.
// 
// File:    ExampleRecipes.cs
// Project: ExampleRecipes

using System.Collections.Generic;
using System.Linq;
using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;

namespace ExampleRecipes
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class ExampleRecipes : BaseUnityPlugin
    {
        public const string PluginGUID = "dickdangerjustice.examplerecipes";
        public const string PluginName = "ExampleRecipes";
        public const string PluginVersion = "1.0.0";

        private AssetBundle EmbeddedResourceBundle;
        private readonly List<GameObject> NewPrefabs = new List<GameObject>();

        private void Awake()
        {
            LoadAssets();
            AddItems();
            ItemManager.Instance.AddRecipesFromJson("ExampleRecipes/Assets/recipes.json");
        }

        // Various forms of asset loading
        private void LoadAssets()
        {
            // Load asset bundle from embedded resources
            Jotunn.Logger.LogInfo($"Embedded resources: {string.Join(",", typeof(ExampleRecipes).Assembly.GetManifestResourceNames())}");
            EmbeddedResourceBundle = AssetUtils.LoadAssetBundleFromResources("two", typeof(ExampleRecipes).Assembly);

            var assetPaths = new List<string>()
            {
                "Assets/CustomItems/Two/Cape1.prefab",
                "Assets/CustomItems/Two/Cape2.prefab"
            };

            foreach (var assetPath in assetPaths)
            {
                NewPrefabs.Add(EmbeddedResourceBundle.LoadAsset<GameObject>(assetPath));
            }
        }

        // Implementation of assets using mocks, adding recipe's manually without the config abstraction
        private void AddItems()
        {
            if (!NewPrefabs.Any()) Jotunn.Logger.LogWarning($"Failed to load assets from bundle: {EmbeddedResourceBundle}");
            else
            {
                NewPrefabs.ForEach(newPrefab =>
                {
                    // Create and add a custom item
                    var ci = new CustomItem(newPrefab, true);
                    ItemManager.Instance.AddItem(ci);
                });
            }
            EmbeddedResourceBundle.Unload(false);
        }
    }
}