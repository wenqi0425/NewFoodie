using NewFoodie.Models;
using System.Collections.Generic;

namespace NewFoodie.Services.Interfaces
{
    public interface IRecipeItemService
    {
        void AddRecipeItem(RecipeItem recipeItem);
        void DeleteRecipeItem(RecipeItem recipeItem);
        void EditRecipeItem(RecipeItem recipeItem);
        IEnumerable<RecipeItem> GetRecipeItemsByRecipeId(int recipeId);
        IEnumerable<Recipe> SearchRecipes(string ingredient);
    }
}
