using NewFoodie.Models;
using NewFoodie.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace NewFoodie.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    [BindProperties]
    public class CreateNewRecipeModel : PageModel
    {
        public Recipe Recipe { get; set; }
        public AppUser AppUser { get; set; }
        public string Message { get; set; }

        private UserManager<AppUser> _manager;
        private IRecipeService _recipeService;
        public List<Recipe> MyRecipes;

        public CreateNewRecipeModel(UserManager<AppUser> manager, IRecipeService recipeService)
        {
            _recipeService = recipeService;
            _manager = manager;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [HttpPost]
        public IActionResult OnPost()
        {
            var currentUserId = _manager.GetUserId(User);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Recipe.UserId = Int32.Parse(currentUserId);
            _recipeService.AddRecipe(Recipe);

            return RedirectToPage("./CheckMyRecipes");
        }
    }
}
