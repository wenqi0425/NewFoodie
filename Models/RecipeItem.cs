﻿using System.ComponentModel.DataAnnotations;

namespace NewFoodie.Models
{
    public class RecipeItem
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Ingredient Name:")]
        public string Name { get; set; }

        [Display(Name = "Amount:")]
        public string Amount { get; set; }

        // Foreign Keys
        public int RecipeId { get; set; }

        // Navigation Properties
        public Recipe Recipe { get; set; }
    }
}
