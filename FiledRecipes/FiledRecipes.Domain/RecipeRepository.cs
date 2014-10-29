using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FiledRecipes.Domain
{
    /// <summary>
    /// Holder for recipes.
    /// </summary>
    public class RecipeRepository : IRecipeRepository
    {
        public void Save() //visa recept - if redan finns - skriv över
        {
            using (StreamWriter writer = new StreamWriter(_path, false, Encoding.UTF8)) //shows recipe for editing, and creating a StreamWriter in UTF8 mode
            { 
                //skriv ut texten
                foreach (var recipe in _recipes)
                {
                    writer.WriteLine(SectionRecipe);//writes down to recipe.txt
                    writer.WriteLine(recipe.Name);
                    writer.WriteLine(SectionIngredients);
                    foreach (var ingredients in recipe.Ingredients)//looping and writes down all lines with ingredients
                    {
                        writer.WriteLine("{0};{1};{2}", ingredients.Amount, ingredients.Measure, ingredients.Name);
                    }
                    writer.WriteLine(SectionInstructions);
                    foreach (var instructions in recipe.Instructions)
                    {
                        writer.WriteLine(instructions);
                    }
                }
            }
        }
        public void Load()
        {
            List<IRecipe> RecipeList = new List<IRecipe>();
            RecipeReadStatus status = RecipeReadStatus.Indefinite; //status will become the next line in document
            
                using (StreamReader reader = new StreamReader(_path))
                {
                    Recipe recipe = null;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {

                        if (!string.IsNullOrWhiteSpace(line)) //if its not NullOrWhiteSpace, continue to read lines
                        {
                        
                        if (line == SectionRecipe)
                        {
                            status = RecipeReadStatus.New; //status will become the next line in document
                        }
                        else if (line == SectionIngredients)
                        {
                            status = RecipeReadStatus.Ingredient; // --:--
                        }
                        else if (line == SectionInstructions)
                        {
                            status = RecipeReadStatus.Instruction; // --:--
                        }
                        else
                        {
                            if (status == RecipeReadStatus.New)
                            {
                                recipe = new Recipe(line);//skapar ett nytt objekt med receptets namn
                                RecipeList.Add(recipe); // adderar namnet till listan
                            }
                            else if (status == RecipeReadStatus.Ingredient)
                            {
                                string[] values = line.Split(new char[] { ';' }); // Making a new array with three sections

                                if (values.Length != 3) //If the array() is not equal to 3, throw new exeption
                                {
                                    throw new FileFormatException();
                                }
                                Ingredient ingredient = new Ingredient();
                                ingredient.Amount = values[0];
                                ingredient.Measure = values[1];
                                ingredient.Name = values[2];

                                recipe.Add(ingredient); //adds a ingredient object to the recipelist with ingredients
                            }
                            else if (status == RecipeReadStatus.Instruction)
                            {
                                recipe.Add(line);
                            }
                            else
                            {
                                throw new FileFormatException();
                            }
                        }
                        }
                    }
                }
                RecipeList.TrimExcess();

            _recipes = RecipeList.OrderBy(recipe => recipe.Name).ToList(); //sorting the list with recipe on the recipes name
            IsModified = false;
            OnRecipesChanged(EventArgs.Empty);//Recipe have been read


            
        }
        /// <summary>
        /// Represents the recipe section.
        /// </summary>
        private const string SectionRecipe = "[Recept]";
        
        /// <summary>
        /// Represents the ingredients section.
        /// </summary>
        private const string SectionIngredients = "[Ingredienser]";

        /// <summary>
        /// Represents the instructions section.
        /// </summary>
        private const string SectionInstructions = "[Instruktioner]";

        /// <summary>
        /// Occurs after changes to the underlying collection of recipes.
        /// </summary>
        public event EventHandler RecipesChangedEvent;

        /// <summary>
        /// Specifies how the next line read from the file will be interpreted.
        /// </summary>
        private enum RecipeReadStatus { Indefinite, New, Ingredient, Instruction };

        /// <summary>
        /// Collection of recipes.
        /// </summary>
        private List<IRecipe> _recipes;

        /// <summary>
        /// The fully qualified path and name of the file with recipes.
        /// </summary>
        private string _path;
        //"recipes.txt"

        /// <summary>
        /// Indicates whether the collection of recipes has been modified since it was last saved.
        /// </summary>
        public bool IsModified { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the RecipeRepository class.
        /// </summary>
        /// <param name="path">The path and name of the file with recipes.</param>
        public RecipeRepository(string path)
        {
            // Throws an exception if the path is invalid.
            _path = Path.GetFullPath(path);

            _recipes = new List<IRecipe>();
        }

        /// <summary>
        /// Returns a collection of recipes.
        /// </summary>
        /// <returns>A IEnumerable&lt;Recipe&gt; containing all the recipes.</returns>
        public virtual IEnumerable<IRecipe> GetAll()
        {
            // Deep copy the objects to avoid privacy leaks.
            return _recipes.Select(r => (IRecipe)r.Clone());
        }

        /// <summary>
        /// Returns a recipe.
        /// </summary>
        /// <param name="index">The zero-based index of the recipe to get.</param>
        /// <returns>The recipe at the specified index.</returns>
        public virtual IRecipe GetAt(int index)
        {
            // Deep copy the object to avoid privacy leak.
            return (IRecipe)_recipes[index].Clone();
        }

        /// <summary>
        /// Deletes a recipe.
        /// </summary>
        /// <param name="recipe">The recipe to delete. The value can be null.</param>
        public virtual void Delete(IRecipe recipe)
        {
            // If it's a copy of a recipe...
            if (!_recipes.Contains(recipe))
            {
                // ...try to find the original!
                recipe = _recipes.Find(r => r.Equals(recipe));
            }
            _recipes.Remove(recipe);
            IsModified = true;
            OnRecipesChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Deletes a recipe.
        /// </summary>
        /// <param name="index">The zero-based index of the recipe to delete.</param>
        public virtual void Delete(int index)
        {
            Delete(_recipes[index]);
        }

        /// <summary>
        /// Raises the RecipesChanged event.
        /// </summary>
        /// <param name="e">The EventArgs that contains the event data.</param>
        protected virtual void OnRecipesChanged(EventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler handler = RecipesChangedEvent;

            // Event will be null if there are no subscribers. 
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }
    }
}
