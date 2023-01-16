using NewFoodie.Models;
using NewFoodie.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace NewFoodie.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    [BindProperties]
    public class EditRecipeItemsModel : PageModel
    {
        public Recipe Recipe { get; set; }  
        public RecipeItem RecipeItem1 { get; set; }
        public RecipeItem RecipeItem2 { get; set; }
        public RecipeItem RecipeItem3 { get; set; }
        public RecipeItem RecipeItem4 { get; set; }
        public RecipeItem RecipeItem5 { get; set; }

        private IRecipeService _recipeService;
        private IRecipeItemService _recipeItemService;

        public IEnumerable<RecipeItem> RecipeItemsOfOneRecipe { get; set; }

        public EditRecipeItemsModel(IRecipeService recipeService, IRecipeItemService recipeItemService)
        {
            _recipeService = recipeService;
            _recipeItemService = recipeItemService;
        }
        public IActionResult OnGet(int recipeId)
        {
            Recipe = _recipeService.GetRecipeById(recipeId);
            if (Recipe == null)
            {
                return null;
            }

            RecipeItemsOfOneRecipe = _recipeItemService.GetRecipeItemsByRecipeId(recipeId);

            if (RecipeItemsOfOneRecipe.Count() != 0)
            {
                for (int i = 0; i < RecipeItemsOfOneRecipe.Count(); i++)
                {
                    if (i == 0) { RecipeItem1 = RecipeItemsOfOneRecipe.ElementAt(i); }
                    if (i == 1) { RecipeItem2 = RecipeItemsOfOneRecipe.ElementAt(i); }
                    if (i == 2) { RecipeItem3 = RecipeItemsOfOneRecipe.ElementAt(i); }
                    if (i == 3) { RecipeItem4 = RecipeItemsOfOneRecipe.ElementAt(i); }
                    if (i == 4) { RecipeItem5 = RecipeItemsOfOneRecipe.ElementAt(i); }
                }
            }

            return Page();
        }

        [HttpPost]
        public IActionResult OnPost()
        {
            var recipeId = Recipe.Id;
            
            // we re-assign the Recipe and pointing it to existing Recipe. 
            var RecipeExisted = _recipeService.GetRecipeById(recipeId);

            // fetching existing recipeItems associated with current recipeId
            IEnumerable<RecipeItem> recipeItemsExisted = _recipeItemService.GetRecipeItemsByRecipeId(recipeId);

            // if existing item size is zero, then we think the user is creating a new recipe.
            // a recipe should have items size>0

            Boolean isNewRecipe = recipeItemsExisted.Count() == 0;

            if (isNewRecipe)
            {
                string[] itemNames = new string[] { RecipeItem1.Name, RecipeItem2.Name, RecipeItem3.Name, RecipeItem4.Name, RecipeItem5.Name };
                string[] amounts = new string[] { RecipeItem1.Amount, RecipeItem2.Amount, RecipeItem3.Amount, RecipeItem4.Amount, RecipeItem5.Amount };

                // populating 5 binding fields into RecipeItem List. 
                List<RecipeItem> recipeItemList = new List<RecipeItem>();

                for (int i = 0; i < itemNames.Length; i++)
                {
                    if (itemNames[i] != null && itemNames[i].Length > 0 && amounts[i] != null && amounts[i].Length > 0)
                    {
                        RecipeItem item = new RecipeItem();
                        item.Amount = amounts[i];
                        item.Name = itemNames[i];
                        item.RecipeId = Recipe.Id;
                        recipeItemList.Add(item);
                        _recipeItemService.AddRecipeItem(item);
                    }
                }

                Recipe.RecipeItems = recipeItemList;
            }
            else
            {
                List<RecipeItem> newItems = new List<RecipeItem>() { RecipeItem1, RecipeItem2, RecipeItem3, RecipeItem4, RecipeItem5 };
                
                // for existed items. user may updat them from frontend, fx: delete it, or re-modify amount; on such a case, we need update its db entry. 
                updateIngredientList(oldItems: new List<RecipeItem>(recipeItemsExisted), newItems, _recipeItemService);

                // existed item names
                var itemNameExisted = recipeItemsExisted.Select(recipe => recipe.Name).ToList();
                //if found newly created items, then saving them
                newItems.ForEach(item => saveNewlyAddedItem(Recipe, item, itemNameExisted, _recipeItemService));
            }

            return Page();
        }

        private void saveNewlyAddedItem(Recipe recipe, RecipeItem recipeItem, List<string> itemNamesExisted, IRecipeItemService recipeItemService)
        {
            if (recipeItem != null && recipeItem.Name != null && recipeItem.Name.Length > 0 && !itemNamesExisted.Contains(recipeItem.Name))
            {
                RecipeItem item = new RecipeItem();
                item.Amount = recipeItem.Amount;
                item.Name = recipeItem.Name;
                item.RecipeId = Recipe.Id;
                recipeItemService.AddRecipeItem(item);
            }
        }

        // the size of fullItems is five. 
        // the size of oldItem is less than or equal to five.
        private void updateIngredientList(List<RecipeItem> oldItems, List<RecipeItem> fullItems, IRecipeItemService recipeItemService)
        {
            // fullItems ref. to newly binded values from Front-end;
            // oldItems ref. to items fetched from DB.
            // itemsForEditRefToPesisted ref. to the items that maybe updated by user again.
            List<RecipeItem> itemsForEditRefToPesisted = fullItems.Take(oldItems.Count()).ToList();
            for (int i = 0; i < itemsForEditRefToPesisted.Count; i++)
            {
                // if both fields having been cleaned up, then delete this item from db.
                if (itemsForEditRefToPesisted[i].Name == null && itemsForEditRefToPesisted[i].Amount == null)
                {
                    recipeItemService.DeleteRecipeItem(oldItems[i]); continue;
                }
                else
                {
                    oldItems[i].Name = itemsForEditRefToPesisted[i].Name;
                    oldItems[i].Amount = itemsForEditRefToPesisted[i].Amount == null ? "" : itemsForEditRefToPesisted[i].Amount;
                    recipeItemService.EditRecipeItem(oldItems[i]);
                };
            }
        }
    }
}
