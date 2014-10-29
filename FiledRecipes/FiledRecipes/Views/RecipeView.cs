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
            ContinueOnKeyPressed();
        }

        public void Show(IRecipe recipe)
        {
            Console.Clear();
            Header = recipe.Name; //giving Header a value, shoing the header, and a message(ContiuneOnkeyPressed)
            ShowHeaderPanel();

            Console.WriteLine("INGREDIENSER\n==================");
            foreach (var ingredients in recipe.Ingredients)
	        {
                Console.WriteLine(ingredients);
	        }

            int i = 1;
            Console.WriteLine("INSTRUKTIONER\n==================");
            foreach (var instrutions in recipe.Instructions)
            {
                Console.WriteLine("<{0}>\n{1}", i++, instrutions);
            }

        }
    }

    
}
