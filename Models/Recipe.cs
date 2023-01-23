using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewFoodie.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Recipe Name:")]
        public string Name { get; set; }

        [Display(Name = "Cooking Steps:")]
        public string CookingSteps { get; set; }        

        [Display(Name = "Introduction:")]
        public string? Introduction { get; set; }

        [Display(Name = "Image:")]
        public string ImageData { get; set; }

        // ORM: Object Relationship Mapping. 
        // To map the C# data model to database table, then can map the C# instance to data row.
        // Don't map this attribute into the database
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        // Foreign Keys
        public int UserId { get; set; }

        // Navigation Properties
        public AppUser User { get; set; }
        public IEnumerable<RecipeItem> RecipeItems { get; set; }
    }
}
