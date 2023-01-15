﻿using NewFoodie.Models;
using NewFoodie.Pages.Recipes;
using NewFoodie.Services.EFServices;
using NewFoodie.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewFoodie.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public SelectList SearchCategories { get; set; }

        [BindProperty]
        public RecipeCriteria RecipeCriteria { get; set; } = new RecipeCriteria();

        public IActionResult OnPost()
        {
            return RedirectToPage("/Recipes/GetRecipes", RecipeCriteria);
        }
    }
}
