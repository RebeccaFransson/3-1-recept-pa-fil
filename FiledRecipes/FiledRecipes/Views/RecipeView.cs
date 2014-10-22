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
        public void Load()
        {
            List<String> Recipe = new List<string>();
            using(StreamReader reader = new StreamReader("recipes.txt"))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        //continue read document
                    }
                }
            }
        }
    }

    
}
