using System.Collections.Generic;
using ExampleRecipes.Models;
using Jotunn.Utils;

namespace ExampleRecipes.Services
{
    internal class ExtendedRecipeManager
    {
        public static List<ExtendedRecipe> LoadRecipesFromJson(string recipesPath)
        {
            var json = AssetUtils.LoadText(recipesPath);
            return SimpleJson.SimpleJson.DeserializeObject<List<ExtendedRecipe>>(json);
        }
    }
}
