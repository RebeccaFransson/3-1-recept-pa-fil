using FiledRecipes.Domain;
using FiledRecipes.App.Mvp;
using FiledRecipes.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiledRecipes.Views
{
    /// <summary>
    /// 
    /// </summary>
    public class RecipeView : ViewBase, IRecipeView
    {
        public void Show(IEnumerable<IRecipe> recipes)
        {

        }

        public void Show(IRecipe recipe)
        {
            Header = recipe.Name;
            ShowHeaderPanel();

        }
    }

    
}
