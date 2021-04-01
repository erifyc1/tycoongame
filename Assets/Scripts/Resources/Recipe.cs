using UnityEngine;

[CreateAssetMenu()]
public class Recipe : ScriptableObject
{
    public ResourceType[] inputs;
    public ResourceType[] outputs;

    public Recipe(ResourceType[] inputs, ResourceType[] outputs)
    {
        this.inputs = inputs;
        this.outputs = outputs;
    }

    public Recipe(Recipe recipe)
    {
        inputs = recipe.inputs;
        outputs = recipe.outputs;
    }

    public ResourceType[] GetInputs()
    {
        return inputs;
    }

    public ResourceType[] GetOutputs()
    {
        return outputs;
    }

    public bool HasInput(ResourceType input)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i] == input) return true;
        }
        return false;
    }
}

