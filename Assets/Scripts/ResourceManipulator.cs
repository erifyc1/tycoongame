using System.Collections.Generic;
using UnityEngine;

public class ResourceManipulator : MonoBehaviour
{
    public List<Recipe> recipes { get; set; } = new List<Recipe>();
    public Recipe activeRecipe { get; set; } = null;
    public Dictionary<ResourceType, int> storedResources { get; set; } = new Dictionary<ResourceType, int>();

    public ResourceType[] InputResource(GameObject input) // returns null if recipe is not completed
    {
        if (activeRecipe == null) return null;


        ResourceType type;
        if (input.TryGetComponent(typeof(ResourceItem), out Component comp))
        {
            type = comp.GetComponent<ResourceItem>().GetResourceType();
            Destroy(input); // after resource is counted (in recipe) or not it is deleted

            if (type != null && activeRecipe.HasInput(type))
            {
                if (storedResources.ContainsKey(type))
                {
                    storedResources[type]++;
                }
                else
                {
                    storedResources.Add(type, 1);
                }

                return CheckRecipeCompletion();


            }
            else
            {
                Debug.Log("no input found in active recipe so resource is deleted");
            }


        }
        else 
        { 
            Debug.Log("No resourceitem component in inputted resource"); 
        }
        return null;
    }

    public ResourceType[] CheckRecipeCompletion() // returns null if recipe is not completed
    {
        Dictionary<ResourceType, int> newStored = new Dictionary<ResourceType, int>(storedResources);
        ResourceType[] inputs = activeRecipe.GetInputs();
        bool failedRequirement = false;

        foreach (ResourceType type in inputs)
        {
            if (newStored.ContainsKey(type) && newStored[type] > 0)
            {
                newStored[type]--;
                if (newStored[type] == 0) newStored.Remove(type);
            }
            else
            {
                failedRequirement = true;
                break; // either does not contain key or key == 0 (does not contain enough to finish recipe)
            }
        }

        if (failedRequirement)
        {
            return null;
        }
        else
        {
            storedResources = newStored;
            return activeRecipe.GetOutputs();
        }
    }



}
