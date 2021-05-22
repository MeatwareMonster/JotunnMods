using System;
using System.Collections.Generic;
using System.Linq;
using Jotunn.Configs;
using Jotunn.Entities;

namespace ExampleRecipes.Models
{
    [Serializable]
    public class RecipeRequirement
    {
        public string item;
        public int amount;

        public static RequirementConfig Convert(RecipeRequirement recipeRequirement)
        {
            return new RequirementConfig()
            {
                Amount = recipeRequirement.amount,
                Item = recipeRequirement.item
            };
        }
    }

    [Serializable]
    public class ExtendedRecipe
    {
        public string name;
        public string nameValue;
        public string item;
        public string descriptionToken;
        public string description;
        public int amount;
        public string craftingStation;
        public int minStationLevel;
        public bool enabled;
        public string repairStation;
        public List<RecipeRequirement> resources = new List<RecipeRequirement>();

        public static CustomRecipe Convert(ExtendedRecipe extendedRecipe)
        {
            return new CustomRecipe(
                new RecipeConfig
                {
                    Amount = extendedRecipe.amount,
                    CraftingStation = extendedRecipe.craftingStation,
                    Enabled = extendedRecipe.enabled,
                    Item = extendedRecipe.item,
                    MinStationLevel = extendedRecipe.minStationLevel,
                    Name = extendedRecipe.name,
                    RepairStation = extendedRecipe.repairStation,
                    Requirements = extendedRecipe.resources.Select(RecipeRequirement.Convert).ToArray()
                }
            );
        }
    }
}