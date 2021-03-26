using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : ResourceManipulator
{
    [SerializeField] List<Recipe> _recipes = new List<Recipe>();
    // Start is called before the first frame update
    void Start()
    {
        recipes = _recipes;
        activeRecipe = recipes[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
