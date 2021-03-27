using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : ResourceManipulator
{
    [SerializeField] List<Recipe> _recipes = new List<Recipe>();
    void Start()
    {
        recipes = _recipes;
        activeRecipe = recipes[0];
    }

}
