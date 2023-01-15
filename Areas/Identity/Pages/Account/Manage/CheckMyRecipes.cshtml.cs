using NewFoodie.Models;
using NewFoodie.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewFoodie.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class CheckMyRecipesModel : PageModel
    {
        private UserManager<AppUser> _manager;

        private IRecipeService _recipeService;
        public IEnumerable<Recipe> MyRecipes;
        public Recipe Recipe { get; set; }

        public CheckMyRecipesModel(UserManager<AppUser> manager, IRecipeService recipeService)
        {
            _recipeService = recipeService;
            _manager = manager;
        }

        public async Task OnGet()
        {
            AppUser user = await _manager.GetUserAsync(User);
            MyRecipes = _recipeService.GetRecipesByUser(user);
            MyRecipes = Enumerable.Reverse(MyRecipes);
        }
    }
}
