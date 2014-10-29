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
        public void Show(IEnumerable<IRecipe> recipes) //
        {
            foreach (var recipe in recipes)
            {
                Show(recipe);
                ContinueOnKeyPressed();
            }
            
        }

        public void Show(IRecipe recipe)
        {
            Console.Clear();
            Header = recipe.Name; //giving Header a value, showing the header
            ShowHeaderPanel();

            Console.WriteLine("\nINGREDIENSER\n==================");
            foreach (var ingredients in recipe.Ingredients) //making a loop that writes all ingredients
	        {
                Console.WriteLine(ingredients);
	        }

            int i = 1;
            Console.WriteLine("\nINSTRUKTIONER\n==================");
            foreach (var instrutions in recipe.Instructions) //making another loop
            {
                Console.WriteLine("\n<{0}>\n{1}", i++, instrutions);
            }

        }
    }

    
}
