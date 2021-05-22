// ExampleRecipes
// Example mod that shows how to load multiple recipes from a config.
// 
// File:    ExampleRecipes.cs
// Project: ExampleRecipes

using BepInEx;
using ExampleRecipes.Models;
using ExampleRecipes.Services;
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

        private void Awake()
        {
            LoadAssetBundle();
            AddRecipes();
            UnloadAssetBundle();
        }

        private void LoadAssetBundle()
        {
            // Load asset bundle from embedded resources
            Jotunn.Logger.LogInfo($"Embedded resources: {string.Join(",", typeof(ExampleRecipes).Assembly.GetManifestResourceNames())}");
            EmbeddedResourceBundle = AssetUtils.LoadAssetBundleFromResources("testbundle", typeof(ExampleRecipes).Assembly);
        }

        private void UnloadAssetBundle()
        {
            EmbeddedResourceBundle.Unload(false);
        }

        private void AddRecipes()
        {
            var extendedRecipes = ExtendedRecipeManager.LoadRecipesFromJson("ExampleRecipes/Assets/recipes.json");

            extendedRecipes.ForEach(extendedRecipe =>
            {
                var prefab = EmbeddedResourceBundle.LoadAsset<GameObject>($"Assets/Prefabs/{extendedRecipe.item}.prefab");

                var customItem = new CustomItem(prefab, true);

                LocalizationManager.Instance.AddToken(extendedRecipe.name, extendedRecipe.nameValue, false);
                LocalizationManager.Instance.AddToken(extendedRecipe.descriptionToken, extendedRecipe.description, false);

                ItemManager.Instance.AddItem(customItem);

                ItemManager.Instance.AddRecipe(ExtendedRecipe.Convert(extendedRecipe));
            });
        }
    }
}