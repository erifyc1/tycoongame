using UnityEngine;

[CreateAssetMenu()]
public class ResourceType : ScriptableObject
{
    public string _name;
    public int value;
    public GameObject model;
    
    public ResourceType(string name, int value, GameObject model)
    {
        this._name = name;
        this.value = value;
        this.model = model;
    }

    public string GetName()
    {
        return _name;
    }

    public int GetValue()
    {
        return value;
    }

    public GameObject GetModel()
    {
        return model;
    }
}
