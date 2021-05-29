﻿// ExampleRecipes
// Example mod that shows how to load multiple recipes from a config.
// 
// File:    ExampleRecipes.cs
// Project: ExampleRecipes

using System.IO;
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
    internal class Mod : BaseUnityPlugin
    {
        public const string PluginGUID = "dickdangerjustice.examplerecipes";
        public const string PluginName = "ExampleRecipes";
        public const string PluginVersion = "1.0.0";

        private const string UnityBasePath = "Assets/Prefabs";
        private AssetBundle _embeddedResourceBundle;

        private void Awake()
        {
            LoadAssetBundle();
            AddRecipes();
            UnloadAssetBundle();

            // Example of changing stats after loading the items
            // The null check is necessary in case users remove the item from the config
            var funkySword = PrefabManager.Instance.GetPrefab("FunkySword");
            if (funkySword != null)
            {
                funkySword.GetComponent<ItemDrop>().m_itemData.m_shared.m_damages.m_fire = 1000;
            }
        }

        private void LoadAssetBundle()
        {
            // Load asset bundle from embedded resources
            Jotunn.Logger.LogInfo($"Embedded resources: {string.Join(",", typeof(Mod).Assembly.GetManifestResourceNames())}");
            _embeddedResourceBundle = AssetUtils.LoadAssetBundleFromResources("testbundle", typeof(Mod).Assembly);
        }

        private void UnloadAssetBundle()
        {
            _embeddedResourceBundle.Unload(false);
        }

        private void AddRecipes()
        {
            var extendedRecipes = ExtendedRecipeManager.LoadRecipesFromJson("ExampleRecipes/Assets/recipes.json");

            extendedRecipes.ForEach(extendedRecipe =>
            {
                // Load prefab from asset bundle
                var prefab = _embeddedResourceBundle.LoadAsset<GameObject>($"{Path.Combine(UnityBasePath, extendedRecipe.item)}.prefab");

                // Create custom item
                var customItem = new CustomItem(prefab, true);

                // Edit item drop to set name and description
                var itemDrop = customItem.ItemDrop;
                itemDrop.m_itemData.m_shared.m_name = extendedRecipe.name;
                itemDrop.m_itemData.m_shared.m_description = extendedRecipe.description;

                // Add localizations for name and description
                LocalizationManager.Instance.AddToken(extendedRecipe.name, extendedRecipe.nameValue, false);
                LocalizationManager.Instance.AddToken(extendedRecipe.descriptionToken, extendedRecipe.description, false);

                // Add item with recipe
                customItem.Recipe = ExtendedRecipe.Convert(extendedRecipe);
                ItemManager.Instance.AddItem(customItem);
            });
        }
    }
}