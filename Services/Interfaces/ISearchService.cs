using NewFoodie.Models;

using System.Collections.Generic;

namespace NewFoodie.Services.Interfaces
{
    public interface ISearchService
    {
        //IEnumerable<Recipe> SearchRecipesByCriteria(string category, string criteria);
        IEnumerable<Recipe> SearchRecipesByCriteria(RecipeCriteria RecipeCriteria);
    }
}
