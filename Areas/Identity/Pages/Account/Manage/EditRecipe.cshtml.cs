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
    public class EditRecipeModel : PageModel
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

        public EditRecipeModel(IRecipeService recipeService, IRecipeItemService recipeItemService)
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
            // validate input item name followed by amount. 
            // IsValid = ValidateItemAmountPairs();

            // we need to keep some current recipe states.
            var recipeId = Recipe.Id;
            string existedImageData = null;
            byte[] bytes = null;

            // we re-assign the Recipe and pointing it to existing Recipe. 
            var RecipeExisted = _recipeService.GetRecipeById(recipeId);

            // otherwise updating image by new uploading
            if (Recipe.ImageFile != null)
            {
                using (Stream s = Recipe.ImageFile.OpenReadStream())
                {
                    using (BinaryReader r = new BinaryReader(s))
                    {
                        bytes = r.ReadBytes((Int32)s.Length);
                    }
                }

                existedImageData = Convert.ToBase64String(bytes, 0, bytes.Length);
            }
            else
            {
                //  no mater saving ImageData
                existedImageData = RecipeExisted.ImageData;
            }

            // fetching existing recipeItems associated with current recipeId        
            RecipeExisted.Name = Recipe.Name;
            RecipeExisted.Introduction = Recipe.Introduction;
            RecipeExisted.CookingSteps = Recipe.CookingSteps;
            RecipeExisted.ImageData = existedImageData;

            // recipe already in the DB, so update it.
            _recipeService.EditRecipe(RecipeExisted);
            return RedirectToPage("./CheckMyRecipes");
        }
    }
}
